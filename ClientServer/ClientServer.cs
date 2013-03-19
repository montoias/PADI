using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTypes;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.IO;

namespace ClientServer
{
    //TODO:Store information from method calls
    class ClientServer : MarshalByRefObject, IPuppetClientServer, IClientMetadata
    {
        //TODO: Replace by bounded list
        private MetadataInfo[] metadataInfo = new MetadataInfo[10];
        private FileData[] fileInfo = new FileData[10];
        private static IClientMetadata[] metadataServer = new IClientMetadata[3];
        private static string[] metadataLocation = new string[3];

        //FIXME: is this suppose to be static?
        static IClientMetadata primaryMetadata;

        static void Main(string[] args)
        {
            //TODO: throw exception advising that port is already in use
            TcpChannel channel = (TcpChannel)Helper.GetChannel(Convert.ToInt32(args[0]), true);
            ChannelServices.RegisterChannel(channel, true);
            setMetadataLocation(args);

            //Registering the client object
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

        public MetadataInfo pcreate(string filename, int numDataServers, int readQuorum, int writeQuorum)
        {
            System.Console.WriteLine("Creating the file:" + filename);
            return create(filename, numDataServers, readQuorum, writeQuorum);
        }

        public MetadataInfo popen(string filename)
        {
            System.Console.WriteLine("Opening the file: " + filename);
            return open(filename);
        }

        public void pclose(string filename)
        {
            System.Console.WriteLine("Closing the file: " + filename);
            close(filename);
        }

        public void pdelete(string filename)
        {
            System.Console.WriteLine("Deleting the file: " + filename);
            delete(filename);
        }

        public FileData pread(string filename, int semantics)
        {
            throw new NotImplementedException();
        }

        public void pwrite(string filename, FileStream file)
        {
            throw new NotImplementedException();
        }

        public MetadataInfo open(string filename)
        {
            return primaryMetadata.open(filename);
        }

        public void close(string filename)
        {
            primaryMetadata.close(filename);
        }

        public MetadataInfo create(string filename, int numDataServers, int readQuorum, int writeQuorum)
        {
            return primaryMetadata.create(filename, numDataServers, readQuorum, writeQuorum);
        }

        public void delete(string filename)
        {
            primaryMetadata.delete(filename);
        }
    }
}
