using CommonTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using System.Windows.Forms;

namespace MetadataServer
{
    public partial class MetadataServer : MarshalByRefObject, IMetadataServerClient, IMetadataServerPuppet, IMetadataServerDataServer, IMetadataServer
    {
        private string projectFolder = Directory.GetParent(Application.StartupPath).Parent.FullName;
        private static int port;
        private string fileFolder;
        private string stateFolder, stateFile;
        private int metadataID;

        private System.Threading.Timer heartbeatTimer, checkpointTimer, updateStatsTimer;

        private long HEARTBEAT_PERIOD;
        private long CHECKPOINT_PERIOD;
        private long UPDATE_STATS_PERIOD;
        private int DISTRIBUTION_FILES;

        private Dictionary<string, MetadataInfo> metadataTable = new Dictionary<string, MetadataInfo>();
        private Dictionary<int, IMetadataServer> backupReplicas = new Dictionary<int, IMetadataServer>();
        private MetadataServerState metadataState = new MetadataServerState();
        private int[] metadataLocations;

        private IMetadataServer primaryServer;
        public int primaryServerLocation;

        //Maintains the load on each server at a given time
        List<KeyValuePair<int, DataServerStats>> dataServerLoad = new List<KeyValuePair<int,DataServerStats>>();

        static void Main(string[] args)
        {
            port = Convert.ToInt32(args[0]);
            TcpChannel channel = (TcpChannel)Helper.GetChannel(port);
            ChannelServices.RegisterChannel(channel, false);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(MetadataServer),
                "MetadataServer",
                WellKnownObjectMode.Singleton);

            System.Console.WriteLine("Registering server object @ port : " + port);

            System.Console.ReadLine();
        }

        public string dump()
        {
            System.Console.WriteLine("Dumping metadata table");
            int numCacheFiles = metadataTable.Count;
            string contents = "METADATA STATE\r\n";
            contents += metadataState + "\r\n";

            contents += "Primary metadata @ " + primaryServerLocation + "\r\n";
            contents += "Known active replicas \r\n";
            foreach (KeyValuePair<int, IMetadataServer> entry in backupReplicas)
            {
                contents += entry.Key + "\r\n";
            }

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

        public void init(int[] metadataList)
        {
            metadataID = port - 8081;

            //Loads constant values from config.ini file
            string[] scriptLines = File.ReadAllLines(Path.Combine(projectFolder, "config.ini"));
            HEARTBEAT_PERIOD = (long)Convert.ToDouble((scriptLines[0].Split('='))[1]);
            CHECKPOINT_PERIOD = (long)Convert.ToDouble((scriptLines[1].Split('='))[1]);
            UPDATE_STATS_PERIOD = (long)Convert.ToDouble((scriptLines[2].Split('='))[1]);
            DISTRIBUTION_FILES = Convert.ToInt32((scriptLines[3].Split('='))[1]);

            //Creates folders if they don't exist
            fileFolder = Path.Combine(Application.StartupPath, "Files_" + port);
            stateFolder = Path.Combine(Application.StartupPath, "State_" + port);
            stateFile = Path.Combine(stateFolder, "metadata_state");
            Utils.createFolderFile(fileFolder);
            Utils.createFolderFile(stateFolder);

            metadataLocations = metadataList;

            //Creates timers
            heartbeatTimer = new System.Threading.Timer(heartbeatTick, null, Timeout.Infinite, Timeout.Infinite);
            checkpointTimer = new System.Threading.Timer(checkpointTick, null, Timeout.Infinite, Timeout.Infinite);
            updateStatsTimer = new System.Threading.Timer(updateStatsTick, null, Timeout.Infinite, Timeout.Infinite);
            int currentInstruction = 0;

            //Obtains previous state
            if (File.Exists(stateFile))
            {
                object key = stateFile;
                lock (key)
                {
                    metadataState = Utils.deserializeObject<MetadataServerState>(stateFile);
                }
                currentInstruction = metadataState.currentInstruction;

                foreach(int location in metadataState.dataServersList)
                    dataServerLoad.Add(new KeyValuePair<int, DataServerStats>(location, new DataServerStats()));
            }

            //Determines which is the primary server
            if (findAvailableMetadatas(port))
            {
                System.Console.WriteLine("I'm not the primary...");
                System.Console.WriteLine("Notifying the primary metadata that I'm up!");
                primaryServer.requestState(port);
                executeInstructions(metadataState.log, currentInstruction);
            }

            System.Console.WriteLine("Metadata " + metadataID + " was launched!...");
        }

        /* 
         * This function applies a checkpoint on each replica.
         * It only runs when all the replicas are up.
         */
        private void checkpointTick(object state)
        {
            if (backupReplicas.Count == 2)
            {
                try
                {
                    System.Console.WriteLine("Marking checkpoint...");
                    foreach (IMetadataServer server in backupReplicas.Values)
                    {
                        System.Console.WriteLine("Checkpointing metadata " + server.getMetadataID());
                    }

                    foreach (IMetadataServer server in backupReplicas.Values)
                    {
                        server.checkpoint();
                    }

                    checkpoint();
                }
                catch (SocketException) { }
            }
        }

        private void heartbeatTick(object state)
        {
            try
            {
                System.Console.WriteLine("Sending an heartbeat to the primary metadata.");
                primaryServer.isAlive();
                return;
            }
            catch (SocketException) { }

            retryTick();
        }
        
        /*
         * Function to detect the new metadata server upon primary server failing.
         */
        private void retryTick()
        {
            System.Console.WriteLine("Primary metadata is down. Determining a new one");
            heartbeatTimer.Change(Timeout.Infinite, Timeout.Infinite);
            backupReplicas.Remove(primaryServerLocation);

            if (backupReplicas.Count > 0)
            {
                List<int> locations = new List<int>(backupReplicas.Keys);
                try
                {
                    if (metadataID > backupReplicas[locations[0]].getMetadataID())
                    {
                        primaryServer = backupReplicas[locations[0]];
                        primaryServerLocation = locations[0];
                        heartbeatTimer.Change(HEARTBEAT_PERIOD, HEARTBEAT_PERIOD);
                        return;
                    }
                }
                catch (SocketException) { }
            }

            System.Console.WriteLine("I'm the new primary metadata.");
            checkpointTimer.Change(CHECKPOINT_PERIOD, CHECKPOINT_PERIOD);
            updateStatsTimer.Change(UPDATE_STATS_PERIOD, UPDATE_STATS_PERIOD);
            backupReplicas.Clear();
            primaryServerLocation = port;
        }

        /*
         * Finds all available metadatas, including which one is the primary one.
         * If there aren't metadata servers available, it sets the caller as 
         * the primary metadata server.
         */
        private bool findAvailableMetadatas(int port)
        {
            System.Console.WriteLine("Finding available metadatas...");
            primaryServerLocation = port;

            foreach (int location in metadataLocations)
            {
                if (!port.Equals(location))
                {
                    try
                    {
                        IMetadataServer replica = getMetadataServer(location);

                        primaryServerLocation = replica.notifyMetadataServers(port); //hack : triggering an exception
                        backupReplicas[location] = replica;
                    }
                    //ignore, means the server is down
                    catch (SocketException) { }
                    catch (IOException) { }
                }
            }

            if (!primaryServerLocation.Equals(port))
            {
                primaryServer = getMetadataServer(primaryServerLocation);
                heartbeatTimer.Change(HEARTBEAT_PERIOD, HEARTBEAT_PERIOD);
                return true;
            }
            else
            {
                System.Console.WriteLine("I'm the primary!!");
                checkpointTimer.Change(CHECKPOINT_PERIOD, CHECKPOINT_PERIOD);
                updateStatsTimer.Change(UPDATE_STATS_PERIOD, UPDATE_STATS_PERIOD);
                return false;
            }
        }

        /*
         * Returns the primaryServerLocation, and adds the caller to the
         * list of backup replicas.
         */
        public int notifyMetadataServers(int location)
        {
            isAlive();
            System.Console.WriteLine("Received notification from ms @ " + location);
            backupReplicas[location] = getMetadataServer(location);
            return primaryServerLocation;
        }

        /*
         * The metadata which is specified by "notifier" sends a notification
         * to the primary metadata, telling that it is online. The primary data
         * sends a log to the notifier, in order to maintain consistency.
         */
        public void requestState(int notifier)
        {
            isAlive();

            System.Console.WriteLine("Receiving a request from metadata @ " + notifier);
            backupReplicas[notifier].receiveState(metadataState);
        }

        /*
         * Upon a request, the primary metadata sends a log, that is immediately
         * executed.
         */
        public void receiveState(MetadataServerState metadataState)
        {
            isAlive();
            this.metadataState = metadataState;
            executeInstructions(metadataState.log, metadataState.currentInstruction);
        }

        public void checkpoint()
        {
            isAlive();
            System.Console.WriteLine("CHECKPOINT!");
            metadataState.log.Clear();
            metadataState.currentInstruction = 0;
        }

        public void sendInstruction(InstructionDTO instruction)
        {
            isAlive();

            if (port.Equals(primaryServerLocation))
            {
                List<int> toRemoval = new List<int>();
                foreach (KeyValuePair<int, IMetadataServer> entry in backupReplicas)
                {
                    try
                    {
                        entry.Value.receiveInstruction(instruction);
                        continue;
                    }
                    catch (SocketException) { }

                    //If replica isn't available, we should remove it from the list!
                    toRemoval.Add(entry.Key);
                }

                foreach (int remove in toRemoval)
                {
                    backupReplicas.Remove(remove);
                }
            }
        }

        private void executeInstructions(List<InstructionDTO> log, int currentInstruction)
        {
            for (int i = currentInstruction; i < log.Count; i++)
            {
                try
                {
                    interpretInstruction(log[i]);
                }
                //ignore
                catch (Exception) { }
            }
        }

        public void receiveInstruction(InstructionDTO instruction)
        {
            isAlive();
            interpretInstruction(instruction);
        }

        private void interpretInstruction(InstructionDTO instruction)
        {
            switch (instruction.type)
            {
                case "OPEN":
                    OpenDTO openDTO = (OpenDTO)instruction;
                    open(openDTO.filename, openDTO.location);
                    break;
                case "CLOSE":
                    CloseDTO closeDTO = (CloseDTO)instruction;
                    close(closeDTO.filename, closeDTO.location);
                    break;
                case "CREATE":
                    CreateDTO createDTO = (CreateDTO)instruction;
                    create(createDTO.metadataInfo.filename, createDTO.metadataInfo.numDataServers,
                        createDTO.metadataInfo.readQuorum, createDTO.metadataInfo.writeQuorum, createDTO.metadataInfo.dataServers);
                    break;
                case "DELETE":
                    DeleteDTO deleteDTO = (DeleteDTO)instruction;
                    delete(deleteDTO.filename);
                    break;
                case "REGISTER":
                    RegisterDTO registerDTO = (RegisterDTO)instruction;
                    register(registerDTO.location);
                    break;
                case "QUEUE":
                    QueueFileDTO queueFileDTO = (QueueFileDTO)instruction;
                    processMetadataQueue(queueFileDTO.filename, queueFileDTO.localFilenameInfo);
                    break;
                case "UPDATE":
                    UpdateMetadataDTO updateMetadataDTO = (UpdateMetadataDTO)instruction;
                    updateMetadata(updateMetadataDTO.metadataInfo);
                    break;
            }
        }

        private void updateMetadata(MetadataInfo metadataInfo)
        {
            System.Console.WriteLine("Updating file:" + metadataInfo.filename);
            Utils.serializeObject<MetadataInfo>(metadataInfo, Path.Combine(fileFolder, metadataInfo.filename));
            metadataTable[metadataInfo.filename] = metadataInfo;

            if (metadataState.queueFiles.ContainsKey(metadataInfo.filename))
            {
                metadataState.queueFiles[metadataInfo.filename] = metadataInfo;
            }

        }

        private void updateStatsTick(object state)
        {
            Dictionary<int, DataServerStats> dataServerStats = new Dictionary<int, DataServerStats>();
            System.Console.WriteLine("Gathering stats information.");
            foreach (int location in metadataState.dataServersList)
            {
                try
                {
                    IDataServerMetadataServer dataServer = getDataServer(location);
                    dataServerStats[location] = dataServer.getStats();
                    System.Console.WriteLine(location + " load: " + dataServerStats[location].serverLoad); 
                    dataServer.restartStats();
                }
                catch (SocketException)
                {
                    dataServerStats[location] = new DataServerStats();
                    dataServerStats[location].serverLoad = int.MaxValue;
                }
                catch (IOException)
                {
                    dataServerStats[location] = new DataServerStats();
                    dataServerStats[location].serverLoad = int.MaxValue;
                }
            }

            System.Console.WriteLine("Applying distribution function.");
            distributionFunction(dataServerStats);
        }

        private void distributionFunction(Dictionary<int, DataServerStats> dataServerStats)
        {
            int counter = 0;
            List<KeyValuePair<int, DataServerStats>> sortedServers = dataServerLoad = dataServerStats.ToList();

            while (counter < DISTRIBUTION_FILES)
            {
                bool updated = false;
                //Sorting servers by load
                sortedServers = (from entry in sortedServers
                                 orderby entry.Value.serverLoad descending
                                 select entry).ToList();

                //Ordering files by load in each server
                foreach (KeyValuePair<int, DataServerStats> serverStats in sortedServers)
                {
                    List<KeyValuePair<string, int>> sortedFiles = (from entry in serverStats.Value.fileLoad
                                                                   orderby entry.Value descending
                                                                   select entry).ToList();

                    foreach (KeyValuePair<string, int> fileStats in sortedFiles)
                    {

                        for (int i = 1; i < sortedServers.Count + 1; i++)
                        {
                            //Checks if pays off to trade files between servers
                            if (!sortedServers[sortedServers.Count - i].Value.filesAccessed.ContainsValue(serverStats.Value.filesAccessed[fileStats.Key]) &&
                                sortedServers[sortedServers.Count - i].Value.serverLoad + fileStats.Value < serverStats.Value.serverLoad - fileStats.Value)
                            {
                                Thread thread = new Thread(() => migrateFile(serverStats.Key, sortedServers[sortedServers.Count - i].Key, fileStats.Key));
                                thread.Start();

                                serverStats.Value.serverLoad -= fileStats.Value;
                                sortedServers[sortedServers.Count - 1].Value.serverLoad += fileStats.Value;
                                updated = true;
                                counter++;
                                break;
                            }
                        }
                        if (updated)
                            break;
                    }
                    if (updated)
                        break;
                }
                if (!updated)
                    break;
            }
            System.Console.WriteLine("Finished distribution");
        }

        private void migrateFile(int oldLocation, int newLocation, string localFilename)
        {
            FileData fileData = getDataServer(oldLocation).read(localFilename);
            getDataServer(newLocation).create(localFilename, fileData.file, fileData.version, fileData.clientID, fileData.filename);
            System.Console.WriteLine("Migrating " + fileData.filename + " from " + oldLocation + " to " + newLocation);
            string path = Path.Combine(fileFolder, fileData.filename);
            MetadataInfo metadata = Utils.deserializeObject<MetadataInfo>(path);

            for (int i = 0; i < metadata.dataServers.Count; i++)
            {
                if (metadata.dataServers[i].location == oldLocation)
                {
                    metadata.dataServers[i].location = newLocation;
                    break;
                }
            }

            notifyClients(metadata);
            UpdateMetadataDTO update = new UpdateMetadataDTO(metadata);
            metadataState.log.Add(update);
            metadataState.currentInstruction++;

            updateMetadata(metadata);
            getDataServer(oldLocation).delete(localFilename);
        }

        public int getMetadataID()
        {
            isAlive();
            return metadataID;
        }

        public int getPrimaryMetadataLocation()
        {
            isAlive();
            return primaryServerLocation;
        }

        private IMetadataServer getMetadataServer(int location)
        {
            IMetadataServer replica = (IMetadataServer)Activator.GetObject(
                typeof(IMetadataServer),
                "tcp://localhost:" + location + "/MetadataServer");
            return replica;
        }

        private IDataServerMetadataServer getDataServer(int location)
        {
            IDataServerMetadataServer replica = (IDataServerMetadataServer)Activator.GetObject(
                typeof(IDataServerMetadataServer),
                "tcp://localhost:" + location + "/DataServer");
            return replica;
        }

        private string generateLocalFileName()
        {
            long i = 1;

            foreach (byte b in Guid.NewGuid().ToByteArray())
                i *= ((int)b + 1);

            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
