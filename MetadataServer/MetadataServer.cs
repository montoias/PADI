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

        private System.Threading.Timer timer;
        private long tickerPeriod = 10 * 1000;

        private Dictionary<string, List<int>> openedFiles = new Dictionary<string, List<int>>();
        private Dictionary<string, MetadataInfo> queueMetadata = new Dictionary<string, MetadataInfo>();
        private Dictionary<string, MetadataInfo> metadataTable = new Dictionary<string, MetadataInfo>();

        private Dictionary<int, IDataServerMetadataServer> dataServersList = new Dictionary<int, IDataServerMetadataServer>();
        private Dictionary<int, IMetadataServer> backupReplicas = new Dictionary<int, IMetadataServer>();
        
        private int[] metadataLocations;

        private List<string> log = new List<string>();

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
            int numCacheFiles = metadataTable.Count;
            string contents = "";

            contents += "Primary metadata @ " + primaryServerLocation + "\r\n";
            contents += "Known active replicas \r\n";
            foreach (KeyValuePair<int, IMetadataServer> entry in backupReplicas)
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

        public void init(int[] metadataList)
        {
            metadataID = port - 8081;
            fileFolder = Path.Combine(Application.StartupPath, "Files_" + port);
            stateFolder = Path.Combine(Application.StartupPath, "State_" + port);
            Utils.createFolderFile(fileFolder);
            Utils.createFolderFile(stateFolder);
            stateFile = Path.Combine(stateFolder, "metadata_state");
            metadataLocations = metadataList; 
            timer = new System.Threading.Timer(Tick, null, Timeout.Infinite, Timeout.Infinite);

            if (findAvailableMetadatas(port))
            {
                System.Console.WriteLine("I'm not the primary...");
                notifyPrimaryMetadata(port);
                timer.Change(tickerPeriod, tickerPeriod);
            }

            System.Console.WriteLine("Metadata " + metadataID + " was launched!...");
        }

        private void Tick(object state)
        {
            try
            {
                System.Console.WriteLine("Sending an heartbeat to the primary metadata.");
                primaryServer.isAlive();
            }
            catch (SocketException)
            {
                retryTick();
            }
            catch (IOException)
            {
                retryTick();
            }
        }

        private void retryTick()
        {
            System.Console.WriteLine("Primary metadata is down. Determining a new one");
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            backupReplicas.Remove(primaryServerLocation);

            System.Console.WriteLine("BR Available: " + backupReplicas.Count);

            if (backupReplicas.Count > 0)
            {
                List<int> list = new List<int>(backupReplicas.Keys);
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
                catch (IOException)
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
                    catch (SocketException)
                    {
                        //ignore, means the server is down
                    }
                    catch (IOException)
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

        private void notifyPrimaryMetadata(int notifier)
        {
            System.Console.WriteLine("Notifying the primary metadata that I'm up!");
            primaryServer.requestLog(notifier);
        }

        /*
         * The metadata which is specified by "notifier" sends a notification
         * to the primary metadata, telling that it is online. The primary data
         * sends a log to the notifier, in order to maintain consistency.
         */
        public void requestLog(int notifier)
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
                List<int> toRemoval = new List<int>();
                foreach (KeyValuePair<int, IMetadataServer> entry in backupReplicas)
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
                    catch (IOException)
                    {
                        //If replica isn't available, we should remove it from the list!
                        toRemoval.Add(entry.Key);
                    }
                }

                foreach (int remove in toRemoval)
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
                    if (parameters.Length > 5)
                    {
                        List<string> locations = new List<string>();
                        string[] allLocations = parameters[5].Split('\n');

                        foreach (string location in allLocations)
                            locations.Add(location);

                        create(parameters[1], Convert.ToInt32(parameters[2]), Convert.ToInt32(parameters[3]), Convert.ToInt32(parameters[4]), locations);
                    }
                    else
                        create(parameters[1], Convert.ToInt32(parameters[2]), Convert.ToInt32(parameters[3]), Convert.ToInt32(parameters[4]));
                    break;
                case "DELETE":
                    delete(parameters[1]);
                    break;
                case "REGISTER":
                    register(Convert.ToInt32(parameters[1]));
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


        public int getPrimaryMetadataLocation()
        {
            isAlive();
            return primaryServerLocation;
        }
    }
}
