using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonInterfaces;
using CommonTypes;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.IO;
using System.Collections;

namespace Metadata
{

    //TODO: Check for concurrency in all operations!!!
    //http://msdn.microsoft.com/en-us/library/6ka1wd3w.aspx
    //TODO: Check for folder creation
    //http://social.msdn.microsoft.com/forums/en-US/winformsdatacontrols/thread/cc0cfbc5-5824-4b2e-9e9c-11facad95b46
    //http://stackoverflow.com/questions/9065598/if-a-folder-does-not-exist-create-it

    class Metadata : MarshalByRefObject, IClientMetadata, IPuppetMetadata
    {
        //TODO: Relative path is needed
        private static string fileFolder = "C:\\Users\\Manuel\\Documents\\visual studio 2012\\Projects\\PADI\\Metadata\\bin\\Debug\\Files";

        static void Main(string[] args)
        {
            TcpChannel channel = (TcpChannel)Helper.GetChannel(8081, true);
            ChannelServices.RegisterChannel(channel, true);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(Metadata),
                "Metadata",
                WellKnownObjectMode.Singleton);

            createFolderFile();

            System.Console.WriteLine("press <enter> to exit...");
            System.Console.ReadLine();
        }

        private static void createFolderFile()
        {
            bool folderExists = Directory.Exists(fileFolder);
            if (!folderExists)
                Directory.CreateDirectory(fileFolder);
        }

        //Increment counters
        public MetadataInfo open(string filename)
        {
            String path = Path.Combine(fileFolder, filename);

            if (File.Exists(path))
            {
                System.Console.WriteLine("Opening file:" + filename);
                string[] strings = byteArrayToString(File.ReadAllBytes(path)).Split('\n');

                MetadataInfo metadata = new MetadataInfo(strings[0], Convert.ToInt32(strings[1]), Convert.ToInt32(strings[2]), Convert.ToInt32(strings[3]), strings[4]);
                //TODO: Do things with this metadata
                return metadata;
            }
            else
            {
                //TODO: Impement exception
                System.Console.WriteLine("File doesn't exist:" + filename);
                return new MetadataInfo("ERRO", 0, 0, 0, "");
            }
        }

        //Decrement counters
        public void close(string filename)
        {
            throw new NotImplementedException();
        }

        public MetadataInfo create(string filename, int numDataServers, int readQuorum, int writeQuorum)
        {
            String path = Path.Combine(fileFolder, filename);

            MetadataInfo metadata;
            if (!File.Exists(path))
            {
                System.Console.WriteLine("Creating file:" + filename);
                FileStream newFile = File.Create(path);

                //TODO: Obtain the data severs available
                metadata = new MetadataInfo(filename, numDataServers, readQuorum, writeQuorum, "a");
                byte[] metadataBytes = stringToByteArray(metadata.ToString());
                newFile.Write(metadataBytes, 0, metadataBytes.Length);
                newFile.Close();

                return metadata;
            }
            else
            {
                //TODO: Impement exception
                System.Console.WriteLine("File already exists:" + filename);
                return metadata = new MetadataInfo("ERRO", numDataServers, readQuorum, writeQuorum, "");
            }
        }

        public void delete(string filename)
        {
            String path = Path.Combine(fileFolder, filename);
            if (!File.Exists(path))
            {
                System.Console.WriteLine("Deleting file:" + filename);
                File.Delete(path);
            }
            //TODO: throw new exception
        }

        public void fail()
        {
            throw new NotImplementedException();
        }

        public void recover()
        {
            throw new NotImplementedException();
        }

        private byte[] stringToByteArray(string metadata)
        {
            byte[] bytes = new byte[metadata.Length * sizeof(char)];
            System.Buffer.BlockCopy(metadata.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;

        }
        
        private string byteArrayToString(byte[] metadata)
        {
            char[] chars = new char[metadata.Length / sizeof(char)];
            System.Buffer.BlockCopy(metadata, 0, chars, 0, metadata.Length);
            return new string(chars);
        }

    }
}
