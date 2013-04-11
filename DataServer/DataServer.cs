using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.IO;
using System.Windows.Forms;
using CommonTypes;

namespace DataServer
{
    public class DataServer : MarshalByRefObject, IDataServerClientServer, IDataServerPuppet, IDataServerMetadataServer
    {
        private static string fileFolder;
        private static string[] metadataLocation = new string[3];
        private static IMetadataServerDataServer[] metadataServer = new IMetadataServerDataServer[3];
        private static IMetadataServerDataServer primaryMetadata;
        private bool ignoringMessages = false;

        static void Main(string[] args)
        {
            string port = args[0];
            TcpChannel channel = (TcpChannel)Helper.GetChannel(Convert.ToInt32(port), true);
            ChannelServices.RegisterChannel(channel, true);
            setMetadataLocation(args);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(DataServer),
                "DataServer",
                WellKnownObjectMode.Singleton);

            fileFolder = Path.Combine(Application.StartupPath, "Files_" + port);
            createFolderFile();

            metadataServer[0] = (IMetadataServerDataServer)Activator.GetObject(
               typeof(IMetadataServerDataServer),
               "tcp://localhost:" + metadataLocation[0] + "/MetadataServer");
            primaryMetadata = metadataServer[0];

            primaryMetadata.register(port);

            System.Console.WriteLine("press <enter> to exit...");
            System.Console.ReadLine();

        }

        private static void setMetadataLocation(string[] args)
        {
            metadataLocation[0] = args[1];
            metadataLocation[1] = args[2];
            metadataLocation[2] = args[3];
        }

        private static void createFolderFile()
        {
            bool folderExists = Directory.Exists(fileFolder);

            if (!folderExists)
                Directory.CreateDirectory(fileFolder);
        }

        public void freeze()
        {
            throw new NotImplementedException();
        }

        public void unfreeze()
        {
            throw new NotImplementedException();
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

        //TODO: Retrieve version
        public FileData read(string filename, string semantics)
        {
            checkFailure();
            string path = Path.Combine(fileFolder, filename);

            if (File.Exists(path))
            {
                System.Console.WriteLine("Opening file:" + filename);
                return Utils.deserializeObject<FileData>(path);
            }
            else
            {
                System.Console.WriteLine("File doesn't exist:" + filename);
                return null;
            }
        }

        //TODO: Check semantics
        public void write(string filename, byte[] file)
        {
            checkFailure();
            System.Console.WriteLine("Writing the file:" + filename);
            string path = Path.Combine(fileFolder, filename);
            FileData prev = read(filename, "");
            FileData f = new FileData(file, prev == null ? 0 : prev.version+1);
            Utils.serializeObject<FileData>(f, path);
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
            string contents = "DATA SERVER FOLDER\r\n";
            foreach (string filename in Directory.GetFiles(fileFolder))
                contents += filename + "\r\n";

            return contents;
        }
    }
}
