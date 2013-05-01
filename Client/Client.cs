﻿using CommonTypes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text.RegularExpressions;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Net.Sockets;

namespace Client
{
    class Client : MarshalByRefObject, IClientPuppet, IClientMetadataServer
    {
        private OrderedDictionary fileRegisters = new OrderedDictionary();
        private Dictionary<int, FileData> byteRegisters = new Dictionary<int, FileData>();
        private Dictionary<string, int> fileVersions = new Dictionary<string, int>();
        private int currentFileRegister = 0;

        private static string[] metadataLocations = new string[3];
        private static IMetadataServerClient primaryMetadata;
        private static string port;
        private static int clientID;

        public delegate void WriteDelegate(IDataServerClient dataServer, string localFilename, byte[] file);
        public delegate FileData ReadDelegate(IDataServerClient dataServer, string localFilename);

        static void Main(string[] args)
        {
            port = args[0];
            clientID = 8000 - Convert.ToInt32(port);

            TcpChannel channel = (TcpChannel)Helper.GetChannel(Convert.ToInt32(port), true);
            ChannelServices.RegisterChannel(channel, true);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(Client),
                "Client",
                WellKnownObjectMode.Singleton);

            setMetadataLocation(args);
            getPrimaryMetadata();

            System.Console.WriteLine("Client " + clientID + " was launched!...");
            while (true)
            {
                System.Console.ReadLine();
                System.Console.WriteLine("Client " + clientID);
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
            catch (SocketException)
            {
                System.Console.WriteLine("Primary metadata was down. Looking for a new one.");
                getPrimaryMetadata();
                Thread.Sleep(1000);
                return create(filename, numDataServers, readQuorum, writeQuorum);
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
            catch (SocketException)
            {
                System.Console.WriteLine("Primary metadata was down. Looking for a new one.");
                getPrimaryMetadata();
                Thread.Sleep(1000);
                delete(filename);
            }
        }

        public MetadataInfo open(string filename)
        {
            System.Console.WriteLine("Opening the file: " + filename);
            try
            {
                MetadataInfo info = primaryMetadata.open(filename, clientID);
                if(!fileRegisters.Contains(filename))
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
            catch (SocketException)
            {
                System.Console.WriteLine("Primary metadata was down. Looking for a new one.");
                getPrimaryMetadata();
                Thread.Sleep(1000);
                return open(filename);
            }
        }

        public void close(string filename)
        {
            System.Console.WriteLine("Closing the file: " + filename);
            try
            {
                primaryMetadata.close(filename, clientID);
            }
            catch (FileNotOpenedException)
            {
                System.Console.WriteLine("The file " + filename + " wasn't open!");
            }
            catch (FileDoesNotExistException)
            {
                System.Console.WriteLine("The file " + filename + " does not exist!");
            }
            catch (SocketException)
            {
                System.Console.WriteLine("Primary metadata was down. Looking for a new one.");
                getPrimaryMetadata();
                Thread.Sleep(1000);
                close(filename);
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

            if (fileData == null)
                return null;

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

            if (metadata.dataServers.Count < metadata.numDataServers)
            {
                System.Console.WriteLine("There aren't enough servers to execute the request!");
                return null;
            }

            foreach (string dsInfo in metadata.dataServers)
            {
                string serverLocation = metadata.getDataServerLocation(dsInfo);
                string localFilename = metadata.getLocalFilename(dsInfo);

                System.Console.WriteLine("Reading from dataServer " + (Convert.ToInt32(serverLocation) - 9000));

                IDataServerClient dataServer = (IDataServerClient)Activator.GetObject(
                typeof(IDataServerClient),
                "tcp://localhost:" + serverLocation + "/DataServer");

                results.Add(readDelegate.BeginInvoke(dataServer, localFilename, null, null));
            }

            fileData = readQuorum(metadata, results, semantics);
            if (fileData == null)
                return null;

            if (semantics.Equals("monotonic") && fileVersions.ContainsKey(metadata.filename) && (fileVersions[metadata.filename] >= fileData.version))
            {
                System.Console.WriteLine("Monotonic read was requested: The file obtained was older than the one read before!");
                return null;
            }

            fileVersions[metadata.filename] = fileData.version;
            System.Console.WriteLine("File Read. \r\nVersion " + fileData.version + ": " + Utils.byteArrayToString(fileData.file));
            return fileData;
        }

        private FileData readQuorum(MetadataInfo metadata, List<IAsyncResult> results, string semantics)
        {
            Dictionary<int, FileData> versionResults = new Dictionary<int, FileData>();
            Dictionary<int, int> versionCounter = new Dictionary<int, int>();
            int numResults = 0;
            int maxResults = 0;
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
                                versionCounter[tempFile.version]++;
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
            {
                System.Console.WriteLine("Error in writeQuorum: not enough results");
                return null;
            }

            foreach (int version in versionCounter.Keys)
            {
                System.Console.WriteLine("Version:" + version);
                if (versionCounter[version] > maxResults)
                {
                    maxResults = versionCounter[version];
                    maxVersion = version;
                }
            }

            System.Console.WriteLine("Max Version is: " + maxVersion);

            return versionResults[maxVersion];
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

            if (metadata.dataServers.Count < metadata.numDataServers)
            {
                System.Console.WriteLine("There aren't enough servers to execute the request!");
                return;
            }


            foreach (string dsInfo in metadata.dataServers)
            {
                string serverLocation = metadata.getDataServerLocation(dsInfo);
                string localFilename = metadata.getLocalFilename(dsInfo);

                System.Console.WriteLine("Writing to dataServer " + (Convert.ToInt32(serverLocation) - 9000));

                IDataServerClient dataServer = (IDataServerClient)Activator.GetObject(typeof(IDataServerClient),
                    "tcp://localhost:" + serverLocation + "/DataServer");

                results.Add(writeDelegate.BeginInvoke(dataServer, localFilename, Utils.stringToByteArray(textFile), null, null));
            }

            writeQuorum(metadata, results);
        }

        public void write(int fileRegister, int byteRegister)
        {
            System.Console.WriteLine("Writing to DS where metadata is from file register " + fileRegister);
            System.Console.WriteLine("Contents to write were from byte register " + byteRegister);

            MetadataInfo metadata = (MetadataInfo)fileRegisters[fileRegister];
            FileData fileData = byteRegisters[byteRegister];
            WriteDelegate writeDelegate = new WriteDelegate(writeAsync);
            List<IAsyncResult> results = new List<IAsyncResult>();

            if (metadata.dataServers.Count < metadata.numDataServers)
            {
                System.Console.WriteLine("There aren't enough servers to execute the request!");
                return;
            }

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

            writeQuorum(metadata, results);
        }

        private void writeQuorum(MetadataInfo metadata, List<IAsyncResult> results)
        {
            int numResults = 0;

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

        private static void getPrimaryMetadata()
        {
            System.Console.WriteLine("Finding available metadatas...");

            foreach (string location in metadataLocations)
            {
                try
                {
                    IMetadataServerClient replica = getMetadataServer(location);
                    string primaryServerLocation = replica.getPrimaryMetadataLocation(); //hack : triggering an exception
                    primaryMetadata = primaryServerLocation.Equals(location) ? replica : getMetadataServer(primaryServerLocation);
                    return;
                }
                catch (SocketException)
                {
                    //ignore, means the server is down
                }
            }

            Thread.Sleep(1000); //FIXME: Why do I have this?
        }

        private static IMetadataServerClient getMetadataServer(string location)
        {
            IMetadataServerClient replica = (IMetadataServerClient)Activator.GetObject(
                typeof(IMetadataServerClient),
                "tcp://localhost:" + location + "/MetadataServer");
            return replica;
        }

        private static void setMetadataLocation(string[] args)
        {
            metadataLocations[0] = args[1];
            metadataLocations[1] = args[2];
            metadataLocations[2] = args[3];
        }

        public string dump()
        {
            string toReturn = "";

            System.Console.WriteLine("Dumping Client information");
            toReturn += "Primary Metadata: " + primaryMetadata.getPrimaryMetadataLocation() + "\r\n";
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
