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
    public partial class MetadataServer : MarshalByRefObject, IMetadataServerClient, IMetadataServerPuppet, IMetadataServerDataServer, IMetadataServer
    {
        private string fileFolder;
        private string stateFolder, stateFile;
        private static int port;
        private int metadataID;

        private System.Threading.Timer heartbeatTimer, checkpointTimer;
        private long tickerPeriod = 10 * 1000;

        private Dictionary<string, MetadataInfo> metadataTable = new Dictionary<string, MetadataInfo>();
        private Dictionary<int, IMetadataServer> backupReplicas = new Dictionary<int, IMetadataServer>();
        private MetadataServerState metadataState = new MetadataServerState();
        private int[] metadataLocations;

        public int primaryServerLocation;
        private IMetadataServer primaryServer;

        static void Main(string[] args)
        {
            port = Convert.ToInt32(args[0]);
            TcpChannel channel = (TcpChannel)Helper.GetChannel(port, true);
            ChannelServices.RegisterChannel(channel, true);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(MetadataServer),
                "MetadataServer",
                WellKnownObjectMode.Singleton);

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
            fileFolder = Path.Combine(Application.StartupPath, "Files_" + port);
            stateFolder = Path.Combine(Application.StartupPath, "State_" + port);
            Utils.createFolderFile(fileFolder);
            Utils.createFolderFile(stateFolder);
            stateFile = Path.Combine(stateFolder, "metadata_state");
            metadataLocations = metadataList;
            heartbeatTimer = new System.Threading.Timer(heartbeatTick, null, Timeout.Infinite, Timeout.Infinite);
            checkpointTimer = new System.Threading.Timer(checkpointTick, null, Timeout.Infinite, Timeout.Infinite);
            int currentInstruction = 0;

            if (File.Exists(stateFile))
            {
                metadataState = Utils.deserializeObject<MetadataServerState>(stateFile);
                currentInstruction = metadataState.currentInstruction;
            }

            if (findAvailableMetadatas(port))
            {
                System.Console.WriteLine("I'm not the primary...");
                System.Console.WriteLine("Notifying the primary metadata that I'm up!");
                primaryServer.requestState(port);
                heartbeatTimer.Change(tickerPeriod, tickerPeriod);
                executeInstructions(metadataState.log, currentInstruction);
            }

            System.Console.WriteLine("Metadata " + metadataID + " was launched!...");
        }

        private void checkpointTick(object state)
        {
            if (backupReplicas.Count == 2)
            {
                try
                {
                    System.Console.WriteLine("Marking checkpoint...");
                    foreach (IMetadataServer server in backupReplicas.Values)
                    {
                        System.Console.WriteLine("Check @ " + server.getMetadataID());
                    }

                    foreach (IMetadataServer server in backupReplicas.Values)
                    {
                        server.checkpoint();
                    }

                    checkpoint();
                }
                catch (SocketException) { }
                catch (IOException) { }
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
            catch (IOException) { }

            retryTick();
        }

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
                        heartbeatTimer.Change(tickerPeriod, tickerPeriod);
                        return;
                    }
                }
                catch (SocketException) { }
                catch (IOException) { }
            }

            System.Console.WriteLine("I'm the new primary metadata.");
            checkpointTimer.Change(tickerPeriod * 2, tickerPeriod * 2);
            backupReplicas.Clear();
            primaryServerLocation = port;
        }

        private string generateLocalFileName()
        {
            long i = 1;

            foreach (byte b in Guid.NewGuid().ToByteArray())
                i *= ((int)b + 1);

            return string.Format("{0:x}", i - DateTime.Now.Ticks);
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
                return true;
            }
            else
            {
                System.Console.WriteLine("I'm the primary!!");
                checkpointTimer.Change(tickerPeriod * 2, tickerPeriod * 2);
                return false;
            }
        }

        private static IMetadataServer getMetadataServer(int location)
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
                    catch (IOException) { }

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
                catch (SocketException) { }
                catch (IOException) { }
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
                    processMetadataQueue(queueFileDTO.localFilenameInfo);
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

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
