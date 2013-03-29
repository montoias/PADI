using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.IO;

using CommonTypes;


namespace ClientServer
{
    //TODO:Store information from method calls
    class ClientServer : MarshalByRefObject, IClientServerPuppet
    {
        private Dictionary<string, MetadataInfo> metadataTable = new Dictionary<string, MetadataInfo>();
        private static IMetadataServerClientServer[] metadataServer = new IMetadataServerClientServer[3];
        private static string[] metadataLocation = new string[3];
        private static IMetadataServerClientServer primaryMetadata;

        static void Main(string[] args)
        {
            TcpChannel channel = (TcpChannel)Helper.GetChannel(Convert.ToInt32(args[0]), true);
            ChannelServices.RegisterChannel(channel, true);
            setMetadataLocation(args);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(ClientServer),
                "ClientServer",
                WellKnownObjectMode.Singleton);

            //TODO: Get current location of the MetadataServer
            //Function that check if 0 is up and is the primary server
            //If it isn't up, iterate list
            //If it's not primary, access variable to get primary
            metadataServer[0] = (IMetadataServerClientServer)Activator.GetObject(
               typeof(IMetadataServerClientServer),
               "tcp://localhost:" + metadataLocation[0] + "/MetadataServer");
            primaryMetadata = metadataServer[0];

            System.Console.WriteLine("press <enter> to exit...");
            System.Console.ReadLine();

        }

        private static void setMetadataLocation(string[] args)
        {
            metadataLocation[0] = args[1];
            metadataLocation[1] = args[2];
            metadataLocation[2] = args[3];
        }

        //TODO: Implement algorithm that'll work in case of failure
        private void getMetadataServer()
        {
            metadataServer[0] = (IMetadataServerClientServer)Activator.GetObject(
               typeof(IMetadataServerClientServer),
               "tcp://localhost:" + metadataLocation[0] + "/MetadataServer");

            metadataServer[1] = (IMetadataServerClientServer)Activator.GetObject(
               typeof(IMetadataServerClientServer),
               "tcp://localhost:" + metadataLocation[1] + "/MetadataServer");

            metadataServer[2] = (IMetadataServerClientServer)Activator.GetObject(
               typeof(IMetadataServerClientServer),
               "tcp://localhost:" + metadataLocation[2] + "/MetadataServer");

        }

        public MetadataInfo create(string filename, int numDataServers, int readQuorum, int writeQuorum)
        {
            System.Console.WriteLine("Creating the file:" + filename);
            if (!metadataTable.ContainsKey(filename))
            {
                if (metadataTable.Count == 10) 
                {
                    throw new TableSizeExcedeedException();
                }
                MetadataInfo info = primaryMetadata.create(filename, numDataServers, readQuorum, writeQuorum);
                metadataTable.Add(filename, info);
                return info;
            }
            else 
            {
                throw new FileAlreadyExistsException();
            }
        }

        public void delete(string filename)
        {
            System.Console.WriteLine("Deleting the file: " + filename);
            primaryMetadata.delete(filename);
        }

        public MetadataInfo open(string filename)
        {
            System.Console.WriteLine("Opening the file: " + filename);
            if (!metadataTable.ContainsKey(filename))
            {
                if (metadataTable.Count == 10) 
                {
                    throw new TableSizeExcedeedException();
                }
                MetadataInfo info = primaryMetadata.open(filename);
                metadataTable.Add(filename, info);
                return info;
            }
            else 
            {
                throw new FileAlreadyOpenedException();
            }
        }

        public void close(string filename)
        {
            System.Console.WriteLine("Closing the file: " + filename);
            if (metadataTable.ContainsKey(filename))
            {
                primaryMetadata.close(filename);
                metadataTable.Remove(filename);
            }
            else
            {
                throw new FileNotOpenedException();
            }
        }

        //TODO: Put in file array
        //Make quorum 
        //Async request
        public FileData read(string filename, int semantics)
        {
            if (metadataTable.ContainsKey(filename))
            {
                FileData fileData = null;
                MetadataInfo metadata = metadataTable[filename];
                foreach (string location in metadata.dataServers)
                {
                    System.Console.WriteLine("Reading from dataServer at port " + location);
                    IDataServerClientServer dataServer = (IDataServerClientServer)Activator.GetObject(
                    typeof(IDataServerClientServer),
                    "tcp://localhost:" + location + "/DataServer");
                    fileData = dataServer.read(filename, semantics);
                }

                return fileData;
            }
            else  //TODO: Check if its in disk, load in metadataTable, if not, throw exception
            {
                System.Console.WriteLine("Read - Not in cache.");
                throw new MetadataFileDoesNotExistException();
            } 
        }

        //TODO: keep server objects?
        //TODO: implement quoruns
        public void write(string filename, string textFile)
        {
            if (metadataTable.ContainsKey(filename))
            {
                System.Console.WriteLine("Writing the file: " + filename);

                MetadataInfo metadata = metadataTable[filename];
                foreach (string location in metadata.dataServers)
                {
                    IDataServerClientServer dataServer = (IDataServerClientServer)Activator.GetObject(
                    typeof(IDataServerClientServer),
                    "tcp://localhost:" + location + "/DataServer");
                    dataServer.write(filename, stringToByteArray(textFile));
                }
            }
            else
            {
                throw new MetadataFileDoesNotExistException();
            }
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
