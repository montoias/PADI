using CommonTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Windows.Forms;

namespace MetadataServer
{
    class MetadataServer : MarshalByRefObject, IMetadataServerClientServer, IMetadataServerPuppet, IMetadataServerDataServer, IMetadataServer
    {
        private static string fileFolder;
        private static string port;

        private Dictionary<string, List<int>> openedFiles = new Dictionary<string, List<int>>();
        private Dictionary<string, MetadataInfo> queueMetadata = new Dictionary<string, MetadataInfo>();

        private Dictionary<string, MetadataInfo> metadataTable = new Dictionary<string, MetadataInfo>();

        private Dictionary<string, IDataServerMetadataServer> dataServersList = new Dictionary<string, IDataServerMetadataServer>();
        private static List<IMetadataServer> backupReplicas = new List<IMetadataServer>();

        private bool ignoringMessages = false;

        //TODO: pass metadatalist on process create
        private static string[] metadataLocations = { "8081", "8082", "8083" };

        static string log = "";

        static void Main(string[] args)
        {
            port = args[0];
            TcpChannel channel = (TcpChannel)Helper.GetChannel(Convert.ToInt32(args[0]), true);
            ChannelServices.RegisterChannel(channel, true);

            fileFolder = Path.Combine(Application.StartupPath, "Files_" + port);
            createFolderFile();

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(MetadataServer),
                "MetadataServer",
                WellKnownObjectMode.Singleton);

            //Should try connect first with metadataservers at other locations
            //If no connection was available, then it is the primary server
            //If it is the primary server, it doesn't need to notify the other servers
            if (findAvailableMetadatas(port))
            {
                //TODO: find primary server
                System.Console.WriteLine("I'm not the primary...");
                IMetadataServer replica = (IMetadataServer)Activator.GetObject(
                typeof(IMetadataServer),
                "tcp://localhost:8081/MetadataServer");

                //TODO: Add other available replicas
                backupReplicas.Add(replica);
                notifyPrimaryMetadata(port);
            }

            System.Console.WriteLine("Metadata " + (Convert.ToInt32(port) - 8081) + " was launched!...");
            while (true)
            {
                System.Console.ReadLine();
                System.Console.WriteLine("Metadata " + (Convert.ToInt32(port) - 8081));
            }
        }

        /**************************
         ********* CLIENT *********
         **************************/

        public MetadataInfo create(string filename, int numDataServers, int readQuorum, int writeQuorum)
        {
            string path = Path.Combine(fileFolder, filename);
            MetadataInfo metadata;

            string inst = "CREATE" + "," + filename + "," + numDataServers + "," + readQuorum + "," + writeQuorum;
            sendInstruction(inst);
            log += inst + "-";


            if (!metadataTable.ContainsKey(filename) && !File.Exists(path))
            {
                System.Console.WriteLine("Creating file: " + filename);

                int tempNumServers = (numDataServers > dataServersList.Count) ? dataServersList.Count : numDataServers;
                ArrayList dataServersArray = new ArrayList(dataServersList.Keys);
                List<string> locations = new List<string>();

                //TODO: Distribution algorithm
                for (int i = 0; i < tempNumServers; i++)
                {
                    locations.Add((string)dataServersArray[i] + "," + generateLocalFileName());
                }

                metadata = new MetadataInfo(filename, numDataServers, readQuorum, writeQuorum, locations);
                if (numDataServers > dataServersList.Count)
                {
                    queueMetadata.Add(filename, metadata);
                    metadata = new MetadataInfo(filename, tempNumServers, tempNumServers, tempNumServers, locations);
                }

                metadataTable.Add(filename, metadata);
                Utils.serializeObject<MetadataInfo>(metadata, path);

                return metadata;
            }
            else
            {
                System.Console.WriteLine("File already exists:" + filename);
                throw new FileAlreadyExistsException();
            }
        }

        public void delete(string filename)
        {
            string path = Path.Combine(fileFolder, filename);
            string inst = "DELETE" + "," + filename;
            sendInstruction(inst);
            log += inst + "-";

            if (File.Exists(path))
            {
                if (!openedFiles.ContainsKey(filename))
                {
                    System.Console.WriteLine("Deleting file:" + filename);
                    metadataTable.Remove(filename);
                    File.Delete(path);
                }
                else
                {
                    throw new CannotDeleteFileException();
                }
            }
            else
            {
                throw new FileDoesNotExistException();
            }
        }

        public MetadataInfo open(string filename, int location)
        {
            string path = Path.Combine(fileFolder, filename);
            MetadataInfo metadata;
            string inst = "OPEN" + "," + filename + "," + location;

            if (openedFiles.ContainsKey(filename) && openedFiles[filename].Contains(location))
                throw new FileAlreadyOpenedException();

            sendInstruction(inst);
            log += inst + "-";

            if (metadataTable.ContainsKey(filename))
            {
                System.Console.WriteLine("File in cache:" + filename);
                metadata = metadataTable[filename];
            }
            else
            {
                if (File.Exists(path))
                {
                    System.Console.WriteLine("Opening file from disk:" + filename);
                    metadata = Utils.deserializeObject<MetadataInfo>(path);
                    metadataTable.Add(filename, metadata);
                }

                else
                {
                    System.Console.WriteLine("File doesn't exist:" + filename);
                    throw new FileDoesNotExistException();
                }
            }

            if (!openedFiles.ContainsKey(filename))
            {
                List<int> clientList = new List<int>();
                clientList.Add(location);
                openedFiles.Add(filename, clientList);
            }
            else
                openedFiles[filename].Add(location);

            return metadata;
        }

        public void close(string filename, int location)
        {
            string inst = "CLOSE" + "," + filename;
            sendInstruction(inst);
            log += inst + "-";

            if (!openedFiles.ContainsKey(filename) || !openedFiles[filename].Contains(location))
                throw new FileNotOpenedException();

            if (openedFiles[filename].Count == 1)
                openedFiles.Remove(filename);
        }

        /*******************************
         ********* DATA SERVER *********
         *******************************/

        public void register(string location)
        {
            System.Console.WriteLine("Registering new data server at tcp://localhost:" + location + "/DataServer");
            string inst = "REGISTER" + "," + location;
            sendInstruction(inst);
            log += inst + "-";

            dataServersList.Add(location, (IDataServerMetadataServer)Activator.GetObject(
               typeof(IDataServerMetadataServer),
               "tcp://localhost:" + location + "/DataServer"));

            //processMetadataQueue(location);
        }

        private void processMetadataQueue(string location)
        {
            /* string pair = location + "," + generateLocalFileName();

             foreach (string filename in queueMetadata.Keys)
             {
                 System.Console.WriteLine("processing files in the queue:" + filename);
                 string path = Path.Combine(fileFolder, filename);
                 MetadataInfo metadata = Utils.deserializeObject<MetadataInfo>(path);
                 metadata.dataServers.Add(pair);
                 Utils.serializeObject<MetadataInfo>(metadata, path);
                 List<int> clientIDs = null;
                 try
                 {
                     clientIDs = openedFiles[filename];
                 }
                 catch (Exception e)
                 {
                     System.Console.WriteLine(e.StackTrace);
                 }
                 //TODO: Inneficient, store objects at registering
                 foreach (int clientID in clientIDs)
                 {
                     int loc = clientID + 8000;
                     System.Console.WriteLine("updating clientID :" + loc);
                     IClientServerMetadataServer client = (IClientServerMetadataServer)Activator.GetObject(
                            typeof(IClientServerMetadataServer), "tcp://localhost:" + loc + "/DataServer");

                     client.updateMetadata(filename, metadata);
                 }
             }*/
        }

        /********************************
        ********* PUPPET MASTER *********
        *********************************/

        public void fail()
        {
            System.Console.WriteLine("Now ignoring messages.");
            ignoringMessages = true;
        }

        public void recover()
        {
            System.Console.WriteLine("Accepting messages again");
            ignoringMessages = false;
        }

        private void checkFailure()
        {
            System.Console.WriteLine("Checking for failure... It is " + ignoringMessages);
            if (ignoringMessages)
            {
                throw new RemotingException();
            }
        }

        public string dump()
        {
            int numCacheFiles = metadataTable.Count;
            string contents = "";
            /*
            System.Console.WriteLine("Dumping metadata table");
            contents += "METADATA CACHE\r\n";

            if (numCacheFiles != 0)
            {
                foreach (KeyValuePair<string, MetadataInfo> entry in metadataTable)
                {
                    contents += entry.Value + "\r\n";
                }
                contents += numCacheFiles + " files in cache.\r\n";
            }
            else
            {
                contents += "Empty cache\r\n";
            }

            contents += "METADATA FOLDER\r\n";

            foreach (string filename in Directory.GetFiles(fileFolder))
                contents += filename + "\r\n";*/

            return contents;
        }

        /****************************
         ********* METADATA *********
         ****************************/

        private static void createFolderFile()
        {
            bool folderExists = Directory.Exists(fileFolder);

            if (!folderExists)
                Directory.CreateDirectory(fileFolder);
        }

        private string generateLocalFileName()
        {
            long i = 1;

            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }

            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /*
         * Checks if any other metadata is primary.
         * TODO: Could be used to find the primary data, instead of
         * returning a null.
         */
        private static bool findAvailableMetadatas(string port)
        {
            bool found = true;
            System.Console.WriteLine("Finding available metadatas...");

            foreach (string location in metadataLocations)
            {
                if (!port.Equals(location))
                {
                    try
                    {
                        IMetadataServer replica = (IMetadataServer)Activator.GetObject(
                            typeof(IMetadataServer),
                            "tcp://localhost:" + location + "/MetadataServer");

                        replica.receiveInstruction(""); //triggering an exception
                    }
                    catch (Exception)
                    {   //TODO: specify exception
                        found = false;
                    }
                    if (found)
                        return found;
                }
            }
            System.Console.WriteLine("I'm the primary!!");
            return found;

        }

        private static void notifyPrimaryMetadata(string notifier)
        {
            //FIXME: Should be the primary metadata
            System.Console.WriteLine("Notifying the primary metadata that I'm up!");
            backupReplicas[0].requestLog(notifier);
        }

        /*
         * The metadata which is specified by "notifier" sends a notification
         * to the primary metadata, telling that it is online. The primary data
         * sends a log to the notifier, in order to maintain consistency.
         */
        public void requestLog(string notifier)
        {
            System.Console.WriteLine("Receiving a request from metadata @ " + notifier);
            IMetadataServer replica = (IMetadataServer)Activator.GetObject(
               typeof(IMetadataServer),
               "tcp://localhost:" + notifier + "/MetadataServer");

            //Should be a dictionary between location and object
            backupReplicas.Add(replica);
            replica.receiveLog(log);
        }

        /*
         * Upon a request, the primary metadata sends a log, that is immediately
         * executed.
         */
        public void receiveLog(string log)
        {
            executeInstructions(log);
        }

        public void checkState()
        {
            throw new NotImplementedException();
        }

        public void sendInstruction(string instruction)
        {
            //TODO: Check primary
            if (port.Equals("8080"))
            {
                foreach (IMetadataServer replica in backupReplicas)
                    replica.receiveInstruction(instruction);
            }
        }
        private void executeInstructions(string log)
        {
            string[] instructions = log.Split('-');
            foreach (string instruction in instructions)
            {
                try
                {
                    interpretInstruction(instruction);
                }
                catch (Exception) { }//ignore
            }
        }

        public void receiveInstruction(string instruction)
        {
            interpretInstruction(instruction);
        }

        private void interpretInstruction(string command)
        {
            string[] parameters = command.Split(',');

            switch (parameters[0])
            {
                case "OPEN":
                    open(parameters[1], Convert.ToInt32(parameters[2]));
                    break;
                case "CLOSE":
                    close(parameters[1], Convert.ToInt32(parameters[2]));
                    break;
                case "CREATE":
                    create(parameters[1], Convert.ToInt32(parameters[2]), Convert.ToInt32(parameters[3]), Convert.ToInt32(parameters[4]));
                    break;
                case "DELETE":
                    delete(parameters[1]);
                    break;
                case "REGISTER":
                    register(parameters[1]);
                    break;
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
