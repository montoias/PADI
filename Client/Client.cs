using CommonTypes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text.RegularExpressions;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace Client
{
    class Client : MarshalByRefObject, IClientPuppet, IClientMetadataServer
    {
        private OrderedDictionary fileRegisters = new OrderedDictionary();
        private Dictionary<int, FileData> byteRegisters = new Dictionary<int, FileData>();
        private Dictionary<string, int> fileVersions = new Dictionary<string, int>();
        private int currentFileRegister = 0;

        private static IMetadataServerClient[] metadataServer = new IMetadataServerClient[3];
        private static string[] metadataLocation = new string[3];
        private static IMetadataServerClient primaryMetadata;
        private static string port;

        public delegate void WriteDelegate(IDataServerClient dataServer, string localFilename, byte[] file);
        public delegate FileData ReadDelegate(IDataServerClient dataServer, string localFilename);

        static void Main(string[] args)
        {
            port = args[0];
            TcpChannel channel = (TcpChannel)Helper.GetChannel(Convert.ToInt32(port), true);
            ChannelServices.RegisterChannel(channel, true);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(Client),
                "Client",
                WellKnownObjectMode.Singleton);

            //TODO: getPrimaryMetadata -> try any available metadata, and ask if its primary
            setMetadataLocation(args);
            getMetadataServer();
            primaryMetadata = metadataServer[0];

            System.Console.WriteLine("Client " + (Convert.ToInt32(port) - 8000) + " was launched!...");
            while (true)
            {
                System.Console.ReadLine();
                System.Console.WriteLine("Client " + (Convert.ToInt32(port) - 8000));
            }

        }

        /***************************
        ********* METADATA *********
        ****************************/

        public MetadataInfo create(string filename, int numDataServers, int readQuorum, int writeQuorum)
        {
            System.Console.WriteLine("Creating the file:" + filename);
            MetadataInfo info = null;
            try
            {
                info = primaryMetadata.create(filename, numDataServers, readQuorum, writeQuorum);
            }
            catch (FileAlreadyExistsException)
            {
                System.Console.WriteLine("File " + filename + " already exists!");
                return null;

            }
            return info;
        }

        public void delete(string filename)
        {
            System.Console.WriteLine("Deleting the file: " + filename);
            try
            {
                primaryMetadata.delete(filename);
            }
            catch (FileDoesNotExistException)
            {
                System.Console.WriteLine("File " + filename + " does not exist!");
            }
            catch (CannotDeleteFileException)
            {
                System.Console.WriteLine("The file " + filename + " cannot be deleted because is being used!");
            }
        }

        public MetadataInfo open(string filename)
        {
            System.Console.WriteLine("Opening the file: " + filename);
            try
            {
                MetadataInfo info = primaryMetadata.open(filename, 8000 - Convert.ToInt32(port));
                fileRegisters.Insert((currentFileRegister++) % 10, filename, info);
                return info;
            }
            catch (FileAlreadyOpenedException)
            {
                System.Console.WriteLine("The file " + filename + " is already open!");
                return null;
            }
            catch (FileDoesNotExistException)
            {
                System.Console.WriteLine("The file " + filename + " does not exist!");
                return null;
            }
        }

        public void close(string filename)
        {
            System.Console.WriteLine("Closing the file: " + filename);
            try
            {
                primaryMetadata.close(filename, 8000 - Convert.ToInt32(port));
            }
            catch (FileNotOpenedException)
            {
                System.Console.WriteLine("The file " + filename + " wasn't open!");
            }
            catch (FileDoesNotExistException)
            {
                System.Console.WriteLine("The file " + filename + " does not exist!");
            }
        }

        /******************************
        ********* DATA SERVER *********
        *******************************/

        private FileData readAsync(IDataServerClient dataServer, string localFilename)
        {
            return dataServer.read(localFilename);
        }

        public FileData read(int fileRegister, string semantics, int byteRegister)
        {
            System.Console.WriteLine("Reading metadata from file register " + fileRegister + " to byte register " + byteRegister);
            FileData fileData = readOnly(fileRegister, semantics);

            byteRegisters[byteRegister] = fileData;

            return fileData;
        }

        /*
         * This is a different function than read, in order to be able to use the 'copy' function
         * without changing the registers.
         */
        private FileData readOnly(int fileRegister, string semantics)
        {
            MetadataInfo metadata = (MetadataInfo)fileRegisters[fileRegister];
            ReadDelegate readDelegate = new ReadDelegate(readAsync);
            List<IAsyncResult> results = new List<IAsyncResult>();
            FileData fileData;

            foreach (string dsInfo in metadata.dataServers)
            {
                string serverLocation = metadata.getDataServerLocation(dsInfo);
                string localFilename = metadata.getLocalFilename(dsInfo);

                System.Console.WriteLine("Reading from dataServer " + (Convert.ToInt32(serverLocation) - 9000 ));

                IDataServerClient dataServer = (IDataServerClient)Activator.GetObject(
                typeof(IDataServerClient),
                "tcp://localhost:" + serverLocation + "/DataServer");

                results.Add(readDelegate.BeginInvoke(dataServer, localFilename, null, null));
            }

            fileData = readQuorum(metadata, results, semantics);
            //TODO: fileVersions[metadata.filename] = fileData.version;
            return fileData;
        }

        private FileData readQuorum(MetadataInfo metadata, List<IAsyncResult> results, string semantics)
        {
            Dictionary<int, FileData> versionResults = new Dictionary<int, FileData>();
            Dictionary<int, int> versionCounter = new Dictionary<int, int>();
            FileData fileData;
            int numResults = 0;
            int maxVersion = 0;

            while (numResults < metadata.writeQuorum && results.Count > 0)
            {
                Thread.Sleep(1000);
                System.Console.WriteLine("Waiting for quorum");
                for (int i = results.Count - 1; i > -1; i--)
                {
                    if (results[i].IsCompleted)
                    {
                        try
                        {
                            FileData tempFile = ((ReadDelegate)((AsyncResult)results[i]).AsyncDelegate).EndInvoke(results[i]);
                            if (versionCounter.ContainsKey(tempFile.version))
                            {
                                versionCounter[tempFile.version]++;
                            }
                            else
                            {
                                versionResults.Add(tempFile.version, tempFile);
                                versionCounter.Add(tempFile.version, 1);
                            }
                            numResults++;
                        }
                        catch (Exception)
                        {
                            //Possibly send request to next available server
                        }
                        results.RemoveAt(i);
                    }
                }
            }

            if (numResults < metadata.writeQuorum) //TODO: Print error message -> No servers available (retry?)
                return null;

            if (semantics.Equals("default"))
            {
                int maxResults = 0;
                foreach (int version in versionCounter.Keys)
                {
                    System.Console.WriteLine("Version:" + version);
                    if (versionCounter[version] > maxResults)
                    {
                        maxResults = versionCounter[version];
                        maxVersion = version;
                    }
                }
            }
            else
            {
                //TODO: Store previously read version of a file
                //Return most recent file that has a quorum
            }

            System.Console.WriteLine("Max Version is: " + maxVersion);
            fileData = versionResults[maxVersion];

            System.Console.WriteLine("File Read. \r\nVersion " + fileData.version + ": " + Utils.byteArrayToString(fileData.file));
            return fileData;
        }


        private void writeAsync(IDataServerClient dataServer, string localFilename, byte[] file)
        {
            dataServer.write(localFilename, file);
        }

        public void write(int fileRegister, string textFile)
        {
            System.Console.WriteLine("Writing to DS where metadata is from file register " + fileRegister);

            MetadataInfo metadata = (MetadataInfo)fileRegisters[fileRegister];
            WriteDelegate writeDelegate = new WriteDelegate(writeAsync);
            List<IAsyncResult> results = new List<IAsyncResult>();
            int numResults = 0;

            foreach (string dsInfo in metadata.dataServers)
            {
                string serverLocation = metadata.getDataServerLocation(dsInfo);
                string localFilename = metadata.getLocalFilename(dsInfo);

                System.Console.WriteLine("Writing to dataServer " + (Convert.ToInt32(serverLocation) - 9000));

                IDataServerClient dataServer = (IDataServerClient)Activator.GetObject(typeof(IDataServerClient),
                    "tcp://localhost:" + serverLocation + "/DataServer");

                results.Add(writeDelegate.BeginInvoke(dataServer, localFilename, Utils.stringToByteArray(textFile), null, null));
            }

            writeQuorum(metadata, results, numResults);
        }

        public void write(int fileRegister, int byteRegister)
        {
            System.Console.WriteLine("Writing to DS where metadata is from file register " + fileRegister);
            System.Console.WriteLine("Contents to write were from byte register " + byteRegister);

            MetadataInfo metadata = (MetadataInfo)fileRegisters[fileRegister];
            WriteDelegate writeDelegate = new WriteDelegate(writeAsync);
            FileData fileData = byteRegisters[byteRegister];

            List<IAsyncResult> results = new List<IAsyncResult>();
            int numResults = 0;

            foreach (string dsInfo in metadata.dataServers)
            {
                string serverLocation = metadata.getDataServerLocation(dsInfo);
                string localFilename = metadata.getLocalFilename(dsInfo);

                System.Console.WriteLine("Writing to dataServer " + (Convert.ToInt32(serverLocation) - 9000));

                IDataServerClient dataServer = (IDataServerClient)Activator.GetObject(
                typeof(IDataServerClient),
                "tcp://localhost:" + serverLocation + "/DataServer");

                results.Add(writeDelegate.BeginInvoke(dataServer, localFilename, fileData.file, null, null));
            }

            writeQuorum(metadata, results, numResults);
        }

        private void writeQuorum(MetadataInfo metadata, List<IAsyncResult> results, int numResults)
        {
            while (numResults < metadata.writeQuorum)
            {
                Thread.Sleep(1000);
                System.Console.WriteLine("Waiting for quorum");
                for (int i = results.Count - 1; i > -1; i--)
                {
                    if (results[i].IsCompleted)
                    {
                        try
                        {
                            ((WriteDelegate)((AsyncResult)results[i]).AsyncDelegate).EndInvoke(results[i]);
                            results.RemoveAt(i);
                            numResults++;
                        }
                        catch (Exception) //Specify exception
                        {
                            //Possibly send request to next available server
                        }
                    }
                }

                if (numResults < metadata.writeQuorum) //TODO: Print error message -> No servers available (retry?)
                    System.Console.WriteLine("Error in writeQuorum: not enough results"); 
            }

            System.Console.WriteLine("File " + metadata.filename + " written.");
        }

        /**************************
         ********* CLIENT *********
         **************************/

        private static void getMetadataServer()
        {
            metadataServer[0] = (IMetadataServerClient)Activator.GetObject(
               typeof(IMetadataServerClient),
               "tcp://localhost:" + metadataLocation[0] + "/MetadataServer");

            metadataServer[1] = (IMetadataServerClient)Activator.GetObject(
               typeof(IMetadataServerClient),
               "tcp://localhost:" + metadataLocation[1] + "/MetadataServer");

            metadataServer[2] = (IMetadataServerClient)Activator.GetObject(
               typeof(IMetadataServerClient),
               "tcp://localhost:" + metadataLocation[2] + "/MetadataServer");
        }

        private static void setMetadataLocation(string[] args)
        {
            metadataLocation[0] = args[1];
            metadataLocation[1] = args[2];
            metadataLocation[2] = args[3];
        }

        //TODO: print current metadata server
        public string dump()
        {
            string toReturn = "";

            System.Console.WriteLine("Dumping Client information");
            toReturn += "FileRegisters\r\n";

            object[] keys = new object[fileRegisters.Keys.Count];
            fileRegisters.Keys.CopyTo(keys, 0);
            for (int i = 0; i < fileRegisters.Keys.Count; i++)
            {
                toReturn += "FileRegister: " + i + "\r\n" + fileRegisters[i] + "\r\n";
            }

            toReturn += "\r\n\r\nByteRegisters\r\n";

            foreach (KeyValuePair<int, FileData> fileData in byteRegisters)
            {
                toReturn += "ByteRegister: " + fileData.Key + "\r\n";
                toReturn += "Version:" + fileData.Value.version + "\r\n";
                toReturn += Utils.byteArrayToString(fileData.Value.file) + "\r\n";
            }

            System.Console.WriteLine(toReturn);
            return toReturn;
        }

        public void copy(int fileRegister1, string semantics, int fileRegister2, string salt)
        {
            FileData fileData = readOnly(fileRegister1, semantics);
            write(fileRegister2, Utils.byteArrayToString(fileData.file) + salt);
        }

        public void exescript(string[] scriptInstructions)
        {
            foreach (string instruction in scriptInstructions)
                if (!instruction[0].Equals('#'))
                    interpretInstruction(instruction);
        }

        private void interpretInstruction(string command)
        {
            string[] parameters = Regex.Split(command, ", ");
            string[] processInst = parameters[0].Split(' ');
            string instruction = processInst[0];
            string[] processInfo = processInst[1].Split('-');
            int processNumber = Convert.ToInt32(processInfo[1]) - 1;

            switch (instruction)
            {
                case "WRITE":
                    try
                    {
                        int registerIndex = Convert.ToInt32(parameters[2]);
                        write(Convert.ToInt32(parameters[1]), registerIndex);
                    }
                    catch (FormatException)
                    {
                        string textFile = parameters[2].Replace("\"", "");
                        write(Convert.ToInt32(parameters[1]), textFile);
                    }

                    break;
                case "READ":
                    read(Convert.ToInt32(parameters[1]), parameters[2], Convert.ToInt32(parameters[3]));
                    break;
                case "OPEN":
                    open(parameters[1]);
                    break;
                case "CLOSE":
                    close(parameters[1]);
                    break;
                case "CREATE":
                    create(parameters[1], Convert.ToInt32(parameters[2]), Convert.ToInt32(parameters[3]), Convert.ToInt32(parameters[4]));
                    break;
                case "DELETE":
                    delete(parameters[1]);
                    break;
                case "COPY":
                    string salt = parameters[4].Replace("\"", "");
                    copy(Convert.ToInt32(parameters[1]), parameters[2], Convert.ToInt32(parameters[3]), salt);
                    break;
            }
        }

        /*
         * This function is invoked by the metadata server, when a new data server becomes available 
         * and the client requested for a file that didn't have enough servers.
         */
        public void updateMetadata(string filename, MetadataInfo m)
        {
            if (fileRegisters.Contains(filename))
                fileRegisters[filename] = m;
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
