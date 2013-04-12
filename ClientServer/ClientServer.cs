using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.IO;
using System.Collections.Specialized;

using CommonTypes;
using System.Text.RegularExpressions;
using System.Runtime.Remoting.Messaging;


namespace ClientServer
{
    class ClientServer : MarshalByRefObject, IClientServerPuppet
    {
        private OrderedDictionary fileRegisters = new OrderedDictionary();
        private Dictionary<int, FileData> byteRegisters = new Dictionary<int, FileData>();

        private static IMetadataServerClientServer[] metadataServer = new IMetadataServerClientServer[3];
        private static string[] metadataLocation = new string[3];
        private static IMetadataServerClientServer primaryMetadata;

        private int currentFileRegister = 0;

        public delegate void WriteResult(IDataServerClientServer dataServer, string localFilename,  byte[] file);
        public static WriteResult wr;

        public delegate FileData ReadResult(IDataServerClientServer dataServer, string localFilename);
        public static ReadResult rr;

        static void Main(string[] args)
        {
            TcpChannel channel = (TcpChannel)Helper.GetChannel(Convert.ToInt32(args[0]), true);
            ChannelServices.RegisterChannel(channel, true);

            wr = new WriteResult(getWriteResult);
            setMetadataLocation(args);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(ClientServer),
                "ClientServer",
                WellKnownObjectMode.Singleton);

            metadataServer[0] = (IMetadataServerClientServer)Activator.GetObject(
               typeof(IMetadataServerClientServer),
               "tcp://localhost:" + metadataLocation[0] + "/MetadataServer");
            primaryMetadata = metadataServer[0];

            System.Console.WriteLine("press <enter> to exit...");
            System.Console.ReadLine();

        }

        private static void setMetadataLocation(string[] args)
        {
            metadataLocation[0] = args[1];
            metadataLocation[1] = args[2];
            metadataLocation[2] = args[3];
        }

        private void getMetadataServer()
        {
            metadataServer[0] = (IMetadataServerClientServer)Activator.GetObject(
               typeof(IMetadataServerClientServer),
               "tcp://localhost:" + metadataLocation[0] + "/MetadataServer");

            metadataServer[1] = (IMetadataServerClientServer)Activator.GetObject(
               typeof(IMetadataServerClientServer),
               "tcp://localhost:" + metadataLocation[1] + "/MetadataServer");

            metadataServer[2] = (IMetadataServerClientServer)Activator.GetObject(
               typeof(IMetadataServerClientServer),
               "tcp://localhost:" + metadataLocation[2] + "/MetadataServer");

        }

        public MetadataInfo create(string filename, int numDataServers, int readQuorum, int writeQuorum)
        {
            System.Console.WriteLine("Creating the file:" + filename);
            if (!fileRegisters.Contains(filename)) //This verification may avoid one more message sent through the network.
            {
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
            else 
            {
                System.Console.WriteLine("File " + filename + " already exists!");
                return null;
            }
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
            if (!fileRegisters.Contains(filename))
            {
                
                MetadataInfo info = primaryMetadata.open(filename);
                fileRegisters.Insert((currentFileRegister++)%10, filename, info);
                return info;
            }
            else 
            {
                System.Console.WriteLine("The file " + filename + " is already open!");
                return null;
            }
        }

        public void close(string filename)
        {
            System.Console.WriteLine("Closing the file: " + filename);
            if (fileRegisters.Contains(filename))
                primaryMetadata.close(filename);
            else
                System.Console.WriteLine("The file " + filename + " wasn't open!");
        }

        public FileData read(int fileRegister, string semantics, int byteRegister)
        {
            System.Console.WriteLine("Reading file from metadata info @ register: " + fileRegister);
            FileData fileData = readOnly(fileRegister, semantics);
            
            byteRegisters[byteRegister] = fileData;

            return fileData;
             
        }

        private FileData readOnly(int fileRegister, string semantics)
        {
            FileData fileData = null;
            MetadataInfo metadata = (MetadataInfo)fileRegisters[fileRegister];
            List<IAsyncResult> results = new List<IAsyncResult>();
            Dictionary<int, FileData> versions = new Dictionary<int, FileData>(); 
            Dictionary<int, int> count = new Dictionary<int,int>();

            //int numResults = 0;

            foreach (string dsInfo in metadata.dataServers)
            {
                System.Console.WriteLine("Reading from dataServer at port " + metadata.getDataServerLocation(dsInfo));
                IDataServerClientServer dataServer = (IDataServerClientServer)Activator.GetObject(
                typeof(IDataServerClientServer),
                "tcp://localhost:" + metadata.getDataServerLocation(dsInfo) + "/DataServer");
                string localFilename = metadata.getLocalFilename(dsInfo);
                fileData = dataServer.read(metadata.getLocalFilename(dsInfo));
                //results.Add(rr.BeginInvoke(dataServer, localFilename, null, null));
            }

            /*while (numResults < metadata.writeQuorum)
            {
                foreach (IAsyncResult result in results)
                {
                    if (result.IsCompleted)
                    {
                        result.
                        results.Remove(result);
                        numResults++;
                    }
                }
            }
            */
            return fileData;
        }

        public void write(int fileRegister, string textFile)
        {
            System.Console.WriteLine("Writing file from metadata info @ register: " + fileRegister);

            MetadataInfo metadata = (MetadataInfo)fileRegisters[fileRegister];
            List<IAsyncResult> results = new List<IAsyncResult>();
            int numResults = 0;

            foreach (string dsInfo in metadata.dataServers)
            {
                IDataServerClientServer dataServer = (IDataServerClientServer)Activator.GetObject(
                typeof(IDataServerClientServer),
                "tcp://localhost:" + metadata.getDataServerLocation(dsInfo) + "/DataServer");
                string localFilename = metadata.getLocalFilename(dsInfo);

                results.Add(wr.BeginInvoke(dataServer, localFilename, Utils.stringToByteArray(textFile), null, null));
            }

            while (numResults < metadata.writeQuorum)
            {
                foreach (IAsyncResult result in results)
                {
                    if (result.IsCompleted)
                    {
                        results.Remove(result);
                        numResults++;
                    }
                }
            }
        }

        public void write(int fileRegister, int byteRegister)
        {
            System.Console.WriteLine("Writing file from metadata info @ register: " + fileRegister);

            MetadataInfo metadata = (MetadataInfo)fileRegisters[fileRegister];
            FileData fileData = byteRegisters[byteRegister];
            List<IAsyncResult> results = new List<IAsyncResult>();
            int numResults = 0;

            foreach (string dsInfo in metadata.dataServers)
            {
                IDataServerClientServer dataServer = (IDataServerClientServer)Activator.GetObject(
                typeof(IDataServerClientServer),
                "tcp://localhost:" + metadata.getDataServerLocation(dsInfo) + "/DataServer");
                string localFilename = metadata.getLocalFilename(dsInfo);

                results.Add(wr.BeginInvoke(dataServer, localFilename, fileData.file, null, null));                
            }

            while (numResults < metadata.writeQuorum)
            {
                foreach (IAsyncResult result in results)
                {
                    if (result.IsCompleted)
                    {
                        results.Remove(result);
                        numResults++;
                    }
                }
            }
        }

        private static void getWriteResult(IDataServerClientServer dataServer, string localFilename, byte[] file)
        {
            dataServer.write(localFilename, file);
        }

        private static FileData getReadResult(IDataServerClientServer dataServer, string localFilename)
        {
            return dataServer.read(localFilename);
        }
       
        //TODO: print current metadata server
        public string dump()
        {
            string toReturn = "";

            System.Console.WriteLine("Dumping Client information");
            toReturn += "  FileRegisters\r\n";

            object[] keys = new object[fileRegisters.Keys.Count];
            fileRegisters.Keys.CopyTo(keys, 0);
            for (int i = 0; i < fileRegisters.Keys.Count; i++)
            {
                toReturn += "    FileRegister: " + i + "\r\n" + fileRegisters[i] + "\r\n";  
            }

            toReturn += "  ByteRegisters\r\n";

            foreach (KeyValuePair<int, FileData> fileData in byteRegisters)
            {
                toReturn += "    ByteRegister: " + fileData.Key + "\r\n" + fileData.Value + "\r\n";  
            }

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
            {
                if (!instruction[0].Equals('#'))
                {
                    interpretInstruction(instruction);
                }
            }
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
    }
}
