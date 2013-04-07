using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.IO;
using System.Collections.Specialized;

using CommonTypes;


namespace ClientServer
{
    class ClientServer : MarshalByRefObject, IClientServerPuppet
    {
        private OrderedDictionary fileRegisters = new OrderedDictionary();
        private Dictionary<int, FileData> byteRegisters = new Dictionary<int, FileData>();

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
            if (!fileRegisters.Contains(filename)) //This verification may avoid one more message sent through the network.
            {
                if (fileRegisters.Count == 10) 
                {
                    throw new TableSizeExcedeedException();
                }
                MetadataInfo info = primaryMetadata.create(filename, numDataServers, readQuorum, writeQuorum);
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
            if (!fileRegisters.Contains(filename))
            {
                if (fileRegisters.Count == 10) 
                {
                    throw new TableSizeExcedeedException();
                }
                MetadataInfo info = primaryMetadata.open(filename);
                fileRegisters.Add(filename, info);
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
            if (fileRegisters.Contains(filename))
            {
                primaryMetadata.close(filename);
                fileRegisters.Remove(filename);
            }
            else
            {
                throw new FileNotOpenedException();
            }
        }

        public FileData read(int fileRegister, int semantics)
        {

            System.Console.WriteLine("Reading file from metadata info @ register: " + fileRegister);
            FileData fileData = null;
            MetadataInfo metadata = (MetadataInfo)fileRegisters[fileRegister];

            foreach (string location in metadata.dataServers)
            {
                System.Console.WriteLine("Reading from dataServer at port " + location);
                IDataServerClientServer dataServer = (IDataServerClientServer)Activator.GetObject(
                typeof(IDataServerClientServer),
                "tcp://localhost:" + location + "/DataServer");
                fileData = dataServer.read(metadata.filename, semantics);
            }
            
            byteRegisters[fileRegister] = fileData;

            return fileData;
             
        }

        public void write(int fileRegister, string textFile)
        {
            System.Console.WriteLine("Writing file from metadata info @ register: " + fileRegister);

            MetadataInfo metadata = (MetadataInfo)fileRegisters[fileRegister];
            foreach (string location in metadata.dataServers)
            {
                IDataServerClientServer dataServer = (IDataServerClientServer)Activator.GetObject(
                typeof(IDataServerClientServer),
                "tcp://localhost:" + location + "/DataServer");
                dataServer.write(metadata.filename, Encoding.ASCII.GetBytes(textFile));
            }
        }

        public void write(int fileRegister, int byteRegister)
        {
            System.Console.WriteLine("Writing @ register: " + fileRegister);

            MetadataInfo metadata = (MetadataInfo)fileRegisters[fileRegister];
            FileData fileData = byteRegisters[byteRegister];

            foreach (string location in metadata.dataServers)
            {
                IDataServerClientServer dataServer = (IDataServerClientServer)Activator.GetObject(
                typeof(IDataServerClientServer),
                "tcp://localhost:" + location + "/DataServer");
                dataServer.write(metadata.filename, fileData.file);
            }
        }
    }
}
