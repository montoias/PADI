using CommonTypes;
using System;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;

namespace DataServer
{
    public class DataServer : MarshalByRefObject, IDataServerClient, IDataServerPuppet, IDataServerMetadataServer
    {
        private static string fileFolder;
        private static int dataServerId;
        private static string[] metadataLocations = new string[3];
        private static IMetadataServerDataServer[] metadataServer = new IMetadataServerDataServer[3];
        private static IMetadataServerDataServer primaryMetadata;
        private bool ignoringMessages = false;
        private bool isFrozen = false;

        static void Main(string[] args)
        {
            string port = args[0];
            dataServerId = Convert.ToInt32(port) - 9000;
            TcpChannel channel = (TcpChannel)Helper.GetChannel(Convert.ToInt32(port), true);
            ChannelServices.RegisterChannel(channel, true);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(DataServer),
                "DataServer",
                WellKnownObjectMode.Singleton);

            setMetadataLocation(args);

            fileFolder = Path.Combine(Application.StartupPath, "Files_" + port);
            Utils.createFolderFile(fileFolder);

            findPrimaryMetadata();
            registerDataServer(port);

            System.Console.WriteLine("Data Server " + dataServerId + " was launched!...");

            while (true)
            {
                System.Console.ReadLine();
                System.Console.WriteLine("Data Server " + dataServerId);
            }
        }

        private static void registerDataServer(string port)
        {
            try
            {
                primaryMetadata.register(port);
            }
            catch (SocketException)
            {
                findPrimaryMetadata();
                registerDataServer(port);
            }
        }

        /**************************
         ********* CLIENT *********
         **************************/

        public FileData read(string filename)
        {
            lock (this)
            {
                if (isFrozen)
                    Monitor.Wait(this);
                else
                    checkFailure();

                string path = Path.Combine(fileFolder, filename);

                if (File.Exists(path))
                {
                    System.Console.WriteLine("Opening file:" + filename);
                    return Utils.deserializeObject<FileData>(path);
                }
                else
                {
                    FileData f = new FileData(Utils.stringToByteArray(""), 0);
                    Utils.serializeObject<FileData>(f, path);
                    return f;
                }
            }
        }

        public void write(string filename, byte[] file)
        {
            lock (this)
            {
                if (isFrozen)
                    Monitor.Wait(this);
                else
                    checkFailure();

                System.Console.WriteLine("Writing the file:" + filename);
                string path = Path.Combine(fileFolder, filename);
                FileData prev = read(filename);
                FileData f = new FileData(file, prev == null ? 0 : prev.version + 1);
                Utils.serializeObject<FileData>(f, path);

            }
        }

        /********************************
        ********* PUPPET MASTER *********
        *********************************/

        public void freeze()
        {
            lock (this)
            {
                System.Console.WriteLine("Now delaying messages.");
                isFrozen = true;
            }
        }

        public void unfreeze()
        {
            lock (this)
            {
                if (isFrozen)
                {
                    System.Console.WriteLine("Receiving messages normally again.");
                    Monitor.PulseAll(this);
                    isFrozen = false;
                }
            }
        }

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
            if (ignoringMessages)
                throw new SocketException();
        }

        /******************************
        ********* DATA SERVER *********
        *******************************/

        private static void findPrimaryMetadata()
        {
            System.Console.WriteLine("Finding available metadatas...");

            foreach (string location in metadataLocations)
            {
                try
                {
                    IMetadataServerDataServer replica = getMetadataServer(location);
                    string primaryServerLocation = replica.getPrimaryMetadataLocation(); //hack : triggering an exception
                    primaryMetadata = primaryServerLocation.Equals(location) ? replica : getMetadataServer(primaryServerLocation);
                    return;
                }
                catch (SocketException)
                {
                    //ignore, means the server is down
                }
            }

            Thread.Sleep(1000);
        }

        private static IMetadataServerDataServer getMetadataServer(string location)
        {
            IMetadataServerDataServer replica = (IMetadataServerDataServer)Activator.GetObject(
                typeof(IMetadataServerDataServer),
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
            string contents = "DATA SERVER FOLDER\r\n";
            foreach (string filename in Directory.GetFiles(fileFolder))
                contents += filename + "\r\n";

            System.Console.WriteLine(contents);
            return contents;
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
