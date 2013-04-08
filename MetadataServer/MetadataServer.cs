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

namespace MetadataServer
{

    //TODO: Check for concurrency in all operations!!!
    //http://msdn.microsoft.com/en-us/library/6ka1wd3w.aspx

    class MetadataServer : MarshalByRefObject, IMetadataServerClientServer, IMetadataServerPuppet, IMetadataServerDataServer
    {
        private static string fileFolder = Path.Combine(Application.StartupPath, "Files");
        private Dictionary<string, int> fileCounter = new Dictionary<string, int>();
        private Dictionary<string, MetadataInfo> metadataTable = new Dictionary<string, MetadataInfo>();
        private Dictionary<string, IDataServerMetadataServer> dataServersList = new Dictionary<string, IDataServerMetadataServer>();

        static void Main(string[] args)
        {
            TcpChannel channel = (TcpChannel)Helper.GetChannel(Convert.ToInt32(args[0]), true);
            ChannelServices.RegisterChannel(channel, true);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(MetadataServer),
                "MetadataServer",
                WellKnownObjectMode.Singleton);

            createFolderFile();

            System.Console.WriteLine("press <enter> to exit...");
            System.Console.ReadLine();
        }

        /**************************
         ********* CLIENT *********
         **************************/

        public MetadataInfo create(string filename, int numDataServers, int readQuorum, int writeQuorum)
        {
            string path = Path.Combine(fileFolder, filename);

            MetadataInfo metadata;
            if (!metadataTable.ContainsKey(filename) && !File.Exists(path))
            {
                if (numDataServers <= dataServersList.Count)
                {
                    byte[] metadataBytes;

                    System.Console.WriteLine("Creating file:" + filename + " Length:" + filename.Length);

                    //TODO: Distribution algorithm
                    ArrayList dataServersArray = new ArrayList(dataServersList.Keys);
                    List<string> locations = new List<string>();
                    for (int i = 0; i < numDataServers; i++)
                    {
                        locations.Add((string)dataServersArray[i] + "," + generateLocalFileName());
                    }

                    metadata = new MetadataInfo(filename, numDataServers, readQuorum, writeQuorum, locations);

                    fileCounter.Add(filename, 1);
                    metadataTable.Add(filename, metadata);

                    metadataBytes = stringToByteArray(metadata.ToString());
                    File.WriteAllBytes(path, metadataBytes);

                    return metadata;
                }
                else 
                {
                    System.Console.WriteLine("There aren't enough servers for the request!");
                    throw new InsufficientDataServersException(); //TODO: pass number of servers available
                }                
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

            if (metadataTable.ContainsKey(filename))
            {
                int numberOfFiles = fileCounter[filename];
                fileCounter[filename]++;
                return metadataTable[filename];
            }
            else
            {
                if (File.Exists(path))
                {
                    System.Console.WriteLine("Opening file:" + filename);
                    string[] strings = byteArrayToString(File.ReadAllBytes(path)).Split('\n');
                    string[] locations = strings[4].Split(' ');
                    List<string> parsedLocations = new List<string>();

                    foreach (string location in locations)
                    {
                        parsedLocations.Add(location);
                    }

                    MetadataInfo metadata = new MetadataInfo(filename, Convert.ToInt32(strings[1]), Convert.ToInt32(strings[2]),
                                                                        Convert.ToInt32(strings[3]), parsedLocations);
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
            fileCounter[filename]--;
        }

        /*******************************
         ********* DATA SERVER *********
         *******************************/

        public void register(string location)
        {
            System.Console.WriteLine("registering new data server at tcp://localhost:" + location + "/DataServer");

            dataServersList.Add(location, (IDataServerMetadataServer)Activator.GetObject(
               typeof(IDataServerMetadataServer),
               "tcp://localhost:" + location + "/DataServer"));
        }

        /*********************************
        ********* PUPPET MASTER *********
        *********************************/

        public void fail()
        {
            throw new NotImplementedException();
        }

        public void recover()
        {
            throw new NotImplementedException();
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

            foreach(string filename in Directory.GetFiles(fileFolder))
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
        

        private byte[] stringToByteArray(string s)
        {
            byte[] bytes = new byte[s.Length * sizeof(char)];
            System.Buffer.BlockCopy(s.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private string byteArrayToString(byte[] b)
        {
            char[] chars = new char[b.Length / sizeof(char)];
            System.Buffer.BlockCopy(b, 0, chars, 0, b.Length);
            return new string(chars);
        }
    }
}
