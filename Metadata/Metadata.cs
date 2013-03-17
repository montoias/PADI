﻿using System;
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
using System.Windows.Forms;

namespace Metadata
{

    //TODO: Check for concurrency in all operations!!!
    //http://msdn.microsoft.com/en-us/library/6ka1wd3w.aspx

    class Metadata : MarshalByRefObject, IClientMetadata, IPuppetMetadata
    {
        private static string fileFolder = Path.Combine(Application.StartupPath, "Files");
        private Dictionary<string, int> fileCounter = new Dictionary<string, int>();
        private Dictionary<string, MetadataInfo> metadataTable = new Dictionary<string, MetadataInfo>();

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

        public MetadataInfo create(string filename, int numDataServers, int readQuorum, int writeQuorum)
        {
            String path = Path.Combine(fileFolder, filename);

            MetadataInfo metadata;
            if (!File.Exists(path))
            {
                FileStream newFile = File.Create(path);
                byte[] metadataBytes;

                System.Console.WriteLine("Creating file:" + filename);

                //TODO: Obtain the data severs available
                metadata = new MetadataInfo(filename, numDataServers, readQuorum, writeQuorum, "a");

                fileCounter.Add(filename, 1);
                metadataTable.Add(filename, metadata);

                metadataBytes = stringToByteArray(metadata.ToString());
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
            if (fileCounter[filename] == 0)
            {
                System.Console.WriteLine("Deleting file:" + filename);
                File.Delete(Path.Combine(fileFolder, filename));
                fileCounter.Remove(filename);
                metadataTable.Remove(filename);
            }
            // TODO: throw new exception
        }

        public MetadataInfo open(string filename)
        {
            String path = Path.Combine(fileFolder, filename);

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
                    MetadataInfo metadata = new MetadataInfo(strings[0], Convert.ToInt32(strings[1]), Convert.ToInt32(strings[2]), Convert.ToInt32(strings[3]), strings[4]);
                    metadataTable.Add(filename, metadata);
                    
                    return metadata;
                }
            
                else
                {
                    //TODO: Impement exception
                    System.Console.WriteLine("File doesn't exist:" + filename);
                    return new MetadataInfo("ERRO", 0, 0, 0, "");
                }
            }
        }

        public void close(string filename)
        {
            if (fileCounter[filename] != 0)
            {
                fileCounter[filename]--;
            }
            //TODO: throw exception
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
