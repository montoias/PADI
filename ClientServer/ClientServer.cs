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
    class ClientServer : MarshalByRefObject, IPuppetClientServer, IClientMetadata
    {
        private Dictionary<string, MetadataInfo> metadataTable = new Dictionary<string, MetadataInfo>();
        private static IClientMetadata[] metadataServer = new IClientMetadata[3];
        private static string[] metadataLocation = new string[3];

        //FIXME: is this suppose to be static?
        static IClientMetadata primaryMetadata;

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
            metadataServer[0] = (IClientMetadata)Activator.GetObject(
               typeof(IClientMetadata),
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
            metadataServer[0] = (IClientMetadata)Activator.GetObject(
               typeof(IClientMetadata),
               "tcp://localhost:" + metadataLocation[0] + "/MetadataServer");

            metadataServer[1] = (IClientMetadata)Activator.GetObject(
               typeof(IClientMetadata),
               "tcp://localhost:" + metadataLocation[1] + "/MetadataServer");

            metadataServer[2] = (IClientMetadata)Activator.GetObject(
               typeof(IClientMetadata),
               "tcp://localhost:" + metadataLocation[2] + "/MetadataServer");

        }

        public MetadataInfo create(string filename, int numDataServers, int readQuorum, int writeQuorum)
        {
            System.Console.WriteLine("Creating the file:" + filename);
            if (!metadataTable.ContainsKey(filename))
            {
                if (metadataTable.Count == 10) //TODO: Exception
                {
                    throw new TableSizeExcedeedException();
                }
                MetadataInfo info = primaryMetadata.create(filename, numDataServers, readQuorum, writeQuorum);
                metadataTable.Add(filename, info);
                return info;
            }
            else //TODO: Exception
            {
                return primaryMetadata.create(filename, numDataServers, readQuorum, writeQuorum);
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
                throw new FileAlreadyOpenException();
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
            else //TODO: Exception
            {
                primaryMetadata.close(filename);
            }
        }

        /*
         * Puppet Master Logic
         */
        public MetadataInfo pcreate(string filename, int numDataServers, int readQuorum, int writeQuorum)
        {
            return create(filename, numDataServers, readQuorum, writeQuorum);
        }

        public void pdelete(string filename)
        {
            delete(filename);
        }

        public MetadataInfo popen(string filename)
        {
            return open(filename);
        }

        public void pclose(string filename)
        {
            close(filename);
        }

        public FileData pread(string filename, int semantics)
        {
            throw new NotImplementedException();
        }

        public void pwrite(string filename, FileStream file)
        {
            throw new NotImplementedException();
        }

    }
}
