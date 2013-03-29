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

        static void Main(string[] args)
        {
            String port = args[0];
            TcpChannel channel = (TcpChannel)Helper.GetChannel(Convert.ToInt32(port), true);
            ChannelServices.RegisterChannel(channel, true);
            setMetadataLocation(args);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(DataServer),
                "DataServer",
                WellKnownObjectMode.Singleton);

            fileFolder = Path.Combine(Application.StartupPath, "Files_" + port);
            createFolderFile();

            //TODO: Get current location of the MetadataServer
            //Function that check if 0 is up and is the primary server
            //If it isn't up, iterate list
            //If it's not primary, access variable to get primary
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
            throw new NotImplementedException();
        }

        public void recover()
        {
            throw new NotImplementedException();
        }

        //TODO: Retrieve version
        public FileData read(string filename, int semantics)
        {
            String path = Path.Combine(fileFolder, filename);

            if (File.Exists(path))
            {
                System.Console.WriteLine("Opening file:" + filename);
                FileData fileData = new FileData(System.IO.File.ReadAllBytes(path), 0);
               
                return fileData;
            }
            else
            {
                System.Console.WriteLine("File doesn't exist:" + filename);
                throw new FileDoesNotExistException();
            }
        }

        //TODO: Attribute version
        public void write(string filename, byte[] file)
        {
            System.Console.WriteLine("Writing the file: " + filename);
            String path = Path.Combine(fileFolder, filename);

            File.WriteAllBytes(path,file);
        }
        
    }
}
