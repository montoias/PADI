using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.IO;
using System.Collections;
using System.Windows.Forms;

using CommonTypes;
using System.Text.RegularExpressions;

namespace MetadataServer
{

    //TODO: Check for concurrency in all operations!!!
    //http://msdn.microsoft.com/en-us/library/6ka1wd3w.aspx

    class MetadataServer : MarshalByRefObject, IMetadataServerClientServer, IMetadataServerPuppet, IMetadataServerDataServer, IMetadataServer
    {
        private static string fileFolder;
        private static string port;
        private Dictionary<string, int> fileCounter = new Dictionary<string, int>();
        private Dictionary<string, MetadataInfo> metadataTable = new Dictionary<string, MetadataInfo>();
        private Dictionary<string, IDataServerMetadataServer> dataServersList = new Dictionary<string, IDataServerMetadataServer>();
        private static List<IMetadataServer> backupReplicas = new List<IMetadataServer>();
        private static string[] metadataLocation = { "8081", "8082", "8083" };

        static string log = "";

        static void Main(string[] args)
        {
            port = args[0];
            TcpChannel channel = (TcpChannel)Helper.GetChannel(Convert.ToInt32(args[0]), true);
            fileFolder = Path.Combine(Application.StartupPath, "Files_" + port);
            ChannelServices.RegisterChannel(channel, true);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(MetadataServer),
                "MetadataServer",
                WellKnownObjectMode.Singleton);

            createFolderFile();

            if (!port.Equals("8081"))
            {
                IMetadataServer replica = (IMetadataServer)Activator.GetObject(
                typeof(IMetadataServer),
                "tcp://localhost:8081/MetadataServer");

                backupReplicas.Add(replica);

                sendNotify(port);
            }


            System.Console.WriteLine("press <enter> to exit...");
            System.Console.ReadLine();
        }

        private static void sendNotify(string p)
        {
            //Should find primary
            backupReplicas[0].notify(p);
        }

        public void receiveNotify(string log)
        {
            System.Console.WriteLine(log);
            executeInstructions(log);
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
                int tempNumServers = numDataServers;

                if (numDataServers > dataServersList.Count)
                {
                    tempNumServers = dataServersList.Count;
                    //TODO: add to processing queue
                }


                System.Console.WriteLine("Creating file: " + filename);

                //TODO: Distribution algorithm
                ArrayList dataServersArray = new ArrayList(dataServersList.Keys);
                List<string> locations = new List<string>();
                for (int i = 0; i < tempNumServers; i++)
                {
                    locations.Add((string)dataServersArray[i] + "," + generateLocalFileName());
                }

                metadata = new MetadataInfo(filename, numDataServers, readQuorum, writeQuorum, locations);

                fileCounter.Add(filename, 1);
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
                if (fileCounter[filename] == 0)
                {
                    System.Console.WriteLine("Deleting file:" + filename);
                    File.Delete(path);
                    fileCounter.Remove(filename);
                    metadataTable.Remove(filename);
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

        public MetadataInfo open(string filename)
        {
            string path = Path.Combine(fileFolder, filename);
            string inst = "OPEN" + "," + filename;
            sendInstruction(inst);
            log += inst + "-";

            if (metadataTable.ContainsKey(filename))
            {
                System.Console.WriteLine("File in cache:" + filename);
                int numberOfFiles = fileCounter[filename];
                fileCounter[filename]++;
                return metadataTable[filename];
            }
            else
            {
                if (File.Exists(path))
                {
                    System.Console.WriteLine("Opening file from disk:" + filename);
                    MetadataInfo metadata = Utils.deserializeObject<MetadataInfo>(path);

                    metadataTable.Add(filename, metadata);
                    fileCounter.Add(filename, 1);

                    return metadata;
                }

                else
                {
                    System.Console.WriteLine("File doesn't exist:" + filename);
                    throw new FileDoesNotExistException();
                }
            }
        }

        public void close(string filename)
        {
            string inst = "CLOSE" + "," + filename;
            sendInstruction(inst);
            log += inst + "-";
            fileCounter[filename]--;
        }

        /*******************************
         ********* DATA SERVER *********
         *******************************/

        public void register(string location)
        {
            System.Console.WriteLine("registering new data server at tcp://localhost:" + location + "/DataServer");
            string inst = "REGISTER" + "," + location;
            sendInstruction(inst);
            log += inst + "-";

            dataServersList.Add(location, (IDataServerMetadataServer)Activator.GetObject(
               typeof(IDataServerMetadataServer),
               "tcp://localhost:" + location + "/DataServer"));
        }

        /*********************************
        ********* PUPPET MASTER *********
        *********************************/

        public void fail()
        {
        }

        public void recover()
        {
        }

        public string dump()
        {
            System.Console.WriteLine("Dumping metadata table");
            int numCacheFiles = metadataTable.Count;
            string contents = "";
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


        public void checkState()
        {
            throw new NotImplementedException();
        }

        public void notify(string notifier)
        {
            IMetadataServer replica = (IMetadataServer)Activator.GetObject(
                typeof(IMetadataServer),
                "tcp://localhost:" + notifier + "/MetadataServer");

            backupReplicas.Add(replica);
            replica.receiveNotify(log);
        }

        private void executeInstructions(string log)
        {
            string [] instructions = log.Split('-');
            foreach (string instruction in instructions)
            {
                interpretInstruction(instruction);
            }
        }

        public void sendInstruction(string instruction)
        {
            if (port.Equals("8080"))
            {
                foreach (IMetadataServer replica in backupReplicas)
                    replica.receiveInstruction(instruction);
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
                case "REGISTER":
                    register(parameters[1]);
                    break;
            }
        }
    }
}
