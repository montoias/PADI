using CommonTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;

namespace MetadataServer
{
    class MetadataServer : MarshalByRefObject, IMetadataServerClient, IMetadataServerPuppet, IMetadataServerDataServer, IMetadataServer
    {
        private static string fileFolder;
        private static string metadataState;
        private static string port;
        private static int metadataID;

        private static System.Threading.Timer timer = new System.Threading.Timer(Tick, null, Timeout.Infinite, Timeout.Infinite);
        private static long tickerPeriod = 10 * 1000;

        private Dictionary<string, List<int>> openedFiles = new Dictionary<string, List<int>>();
        private Dictionary<string, MetadataInfo> queueMetadata = new Dictionary<string, MetadataInfo>();
        private Dictionary<string, MetadataInfo> metadataTable = new Dictionary<string, MetadataInfo>();

        private Dictionary<string, IDataServerMetadataServer> dataServersList = new Dictionary<string, IDataServerMetadataServer>();
        private static Dictionary<string, IMetadataServer> backupReplicas = new Dictionary<string, IMetadataServer>();

        private bool ignoringMessages = false;

        //TODO: pass metadatalist on process create
        private static string[] metadataLocations = { "8081", "8082", "8083" };

        private static List<string> log = new List<string>();

        public static string primaryServerLocation;
        private static IMetadataServer primaryServer;

        static void Main(string[] args)
        {
            port = args[0];
            metadataID = Convert.ToInt32(port) - 8081;
            TcpChannel channel = (TcpChannel)Helper.GetChannel(Convert.ToInt32(args[0]), true);
            ChannelServices.RegisterChannel(channel, true);

            fileFolder = Path.Combine(Application.StartupPath, "Files_" + port);
            metadataState = Path.Combine(Application.StartupPath, "State_" + port);
            Utils.createFolderFile(fileFolder);
            Utils.createFolderFile(metadataState);
            metadataState = Path.Combine(metadataState, "metadata_state");

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(MetadataServer),
                "MetadataServer",
                WellKnownObjectMode.Singleton);

            if (findAvailableMetadatas(port))
            {
                System.Console.WriteLine("I'm not the primary...");
                notifyPrimaryMetadata(port);
                timer.Change(tickerPeriod, tickerPeriod);
            }

            System.Console.WriteLine("Metadata " + metadataID + " was launched!...");

            while (true)
            {
                System.Console.ReadLine();
                System.Console.WriteLine("Metadata " + metadataID);
            }
        }

        /**************************
         ********* CLIENT *********
         **************************/

        public MetadataInfo create(string filename, int numDataServers, int readQuorum, int writeQuorum)
        {
            isAlive();

            string path = Path.Combine(fileFolder, filename);
            MetadataInfo metadata;

            string instruction = "CREATE" + "," + filename + "," + numDataServers + "," + readQuorum + "," + writeQuorum;
            sendInstruction(instruction);
            log.Add(instruction);


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
                    System.Console.WriteLine("Adding to data server queue: " + filename);
                    queueMetadata.Add(filename, metadata);
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
            isAlive();

            string path = Path.Combine(fileFolder, filename);
            string instruction = "DELETE" + "," + filename;
            sendInstruction(instruction);
            log.Add(instruction);

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
            isAlive();

            string path = Path.Combine(fileFolder, filename);
            MetadataInfo metadata;
            string instruction = "OPEN" + "," + filename + "," + location;

            if (openedFiles.ContainsKey(filename) && openedFiles[filename].Contains(location))
                throw new FileAlreadyOpenedException();

            sendInstruction(instruction);
            log.Add(instruction);

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
            isAlive();

            string instruction = "CLOSE" + "," + filename;
            sendInstruction(instruction);
            log.Add(instruction);

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
            isAlive();

            System.Console.WriteLine("Registering new data server at tcp://localhost:" + location + "/DataServer");
            string instruction = "REGISTER" + "," + location;
            sendInstruction(instruction);
            log.Add(instruction);

            dataServersList.Add(location, (IDataServerMetadataServer)Activator.GetObject(
               typeof(IDataServerMetadataServer),
               "tcp://localhost:" + location + "/DataServer"));

            processMetadataQueue(location);
        }

        private void processMetadataQueue(string location)
        {
            string pair = location + "," + generateLocalFileName();
            ArrayList fileList = new ArrayList(queueMetadata.Keys);
            foreach (string filename in fileList)
            {
                System.Console.WriteLine("processing files in the queue: " + filename);
                List<int> clientIDs = openedFiles[filename];

                string path = Path.Combine(fileFolder, filename);
                MetadataInfo metadata = Utils.deserializeObject<MetadataInfo>(path);
                metadata.dataServers.Add(pair);
                Utils.serializeObject<MetadataInfo>(metadata, path);

                //TODO: Inneficient, store objects at registering
                foreach (int clientID in clientIDs)
                {
                    int loc = clientID + 8000;
                    System.Console.WriteLine("updating client at port :" + loc);
                    IClientMetadataServer client = (IClientMetadataServer)Activator.GetObject(
                           typeof(IClientMetadataServer), "tcp://localhost:" + loc + "/Client");

                    client.updateMetadata(filename, metadata);
                }

                if (queueMetadata[filename].numDataServers == dataServersList.Count)
                    queueMetadata.Remove(filename);
            }
        }

        /********************************
        ********* PUPPET MASTER *********
        *********************************/

        public void fail()
        {
            System.Console.WriteLine("Now ignoring messages.");
            ignoringMessages = true;
            backupReplicas.Clear();
            timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public void recover()
        {
            System.Console.WriteLine("Accepting messages again");

            ignoringMessages = false;

            if (findAvailableMetadatas(port))
            {
                System.Console.WriteLine("I'm not the primary...");
                notifyPrimaryMetadata(port);
                timer.Change(tickerPeriod, tickerPeriod);
            }

        }

        public void isAlive()
        {
            System.Console.WriteLine("Checking for failure... It is " + ignoringMessages);

            if (ignoringMessages)
                throw new SocketException();
        }

        public string dump()
        {
            int numCacheFiles = metadataTable.Count;
            string contents = "";

            contents += "Primary metadata @ " + primaryServerLocation + "\r\n";
            contents += "Known active replicas \r\n";
            foreach (KeyValuePair<string, IMetadataServer> entry in backupReplicas)
            {
                contents += entry.Key + "\r\n";
            }

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
                contents += filename + "\r\n";

            System.Console.WriteLine(contents);
            return contents;
        }

        /****************************
         ********* METADATA *********
         ****************************/


        private static void Tick(object state)
        {
            try
            {
                System.Console.WriteLine("Sending an heartbeat to the primary metadata.");
                primaryServer.isAlive();
            }
            catch (SocketException)
            {
                System.Console.WriteLine("Primary metadata is down. Determining a new one");
                timer.Change(Timeout.Infinite, Timeout.Infinite);
                backupReplicas.Remove(primaryServerLocation);

                System.Console.WriteLine("BR Available: " + backupReplicas.Count);

                if (backupReplicas.Count > 0)
                {
                    List<string> list = new List<string>(backupReplicas.Keys);
                    try
                    {

                        if (metadataID > backupReplicas[list[0]].getMetadataID())
                        {
                            primaryServer = backupReplicas[list[0]];
                            primaryServerLocation = list[0];
                            timer.Change(tickerPeriod, tickerPeriod);
                        }
                        else
                        {
                            System.Console.WriteLine("I'm the new primary metadata.");
                            primaryServerLocation = port;
                        }
                    }
                    catch (SocketException)
                    {
                        System.Console.WriteLine("I'm the new primary metadata.");
                        backupReplicas.Clear();
                        primaryServerLocation = port;
                    }
                }
                else
                {
                    System.Console.WriteLine("I'm the new primary metadata.");
                    primaryServerLocation = port;
                }
            }
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
         * Finds all available metadatas, including which one is the primary one.
         * If there aren't metadata servers available, it sets the caller as 
         * the primary metadata server.
         */
        private static bool findAvailableMetadatas(string port)
        {
            System.Console.WriteLine("Finding available metadatas...");
            primaryServerLocation = port;

            foreach (string location in metadataLocations)
            {
                if (!port.Equals(location))
                {
                    try
                    {
                        IMetadataServer replica = getMetadataServer(location);

                        primaryServerLocation = replica.notifyMetadataServers(port); //hack : triggering an exception
                        backupReplicas[location] = replica;
                    }
                    catch (SocketException)
                    {
                        //ignore, means the server is down
                    }
                }
            }

            if (!primaryServerLocation.Equals(port))
            {
                primaryServer = (IMetadataServer)Activator.GetObject(
                    typeof(IMetadataServer),
                    "tcp://localhost:" + primaryServerLocation + "/MetadataServer");

                return true;
            }
            else
            {
                System.Console.WriteLine("I'm the primary!!");
                return false;
            }
        }

        private static IMetadataServer getMetadataServer(string location)
        {
            IMetadataServer replica = (IMetadataServer)Activator.GetObject(
                typeof(IMetadataServer),
                "tcp://localhost:" + location + "/MetadataServer");
            return replica;
        }

        /*
         * Returns the primaryServerLocation, and adds the caller to the
         * list of backup replicas.
         */
        public string notifyMetadataServers(string location)
        {
            isAlive();
            System.Console.WriteLine("Received notification from ms @ " + location);
            backupReplicas[location] = getMetadataServer(location);
            return primaryServerLocation;
        }

        private static void notifyPrimaryMetadata(string notifier)
        {
            System.Console.WriteLine("Notifying the primary metadata that I'm up!");
            primaryServer.requestLog(notifier);
        }

        /*
         * The metadata which is specified by "notifier" sends a notification
         * to the primary metadata, telling that it is online. The primary data
         * sends a log to the notifier, in order to maintain consistency.
         */
        public void requestLog(string notifier)
        {
            isAlive();

            System.Console.WriteLine("Receiving a request from metadata @ " + notifier);

            backupReplicas[notifier].receiveLog(log);
        }

        /*
         * Upon a request, the primary metadata sends a log, that is immediately
         * executed.
         */
        public void receiveLog(List<string> log)
        {
            isAlive();

            executeInstructions(log);
        }

        public void checkpoint()
        {
            isAlive();

            throw new NotImplementedException();
        }

        public void sendInstruction(string instruction)
        {
            isAlive();

            if (port.Equals(primaryServerLocation))
            {
                List<string> toRemoval = new List<string>();
                foreach (KeyValuePair<string, IMetadataServer> entry in backupReplicas)
                {
                    try
                    {
                        entry.Value.receiveInstruction(instruction);
                    }
                    catch (SocketException)
                    {
                        //If replica isn't available, we should remove it from the list!
                        toRemoval.Add(entry.Key);
                    }
                }

                foreach (string remove in toRemoval)
                {
                    backupReplicas.Remove(remove);
                }
            }
        }
        private void executeInstructions(List<string> log)
        {
            foreach (string instruction in log)
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
            isAlive();
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


        public int getMetadataID()
        {
            isAlive();
            return metadataID;
        }


        public string getPrimaryMetadataLocation()
        {
            isAlive();
            return primaryServerLocation;
        }
    }
}
