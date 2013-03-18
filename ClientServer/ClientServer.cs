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

namespace ClientServer
{
    //TODO:Store information from method calls
    class ClientServer : MarshalByRefObject, IPuppetClientServer, IClientMetadata
    {
        //TODO: Replace by bounded list
        MetadataInfo[] metadataInfo = new MetadataInfo[10];
        FileData[] fileInfo = new FileData[10];
        List<string> metadataLocations = new List<string>();

        //FIXME: is this suppose to be static?
        static IClientMetadata primaryMetadata;

        static void Main(string[] args)
        {
            //TODO: throw exception advising that port is already in use
            TcpChannel channel = (TcpChannel)Helper.GetChannel(Convert.ToInt32(args[0]), true);
            ChannelServices.RegisterChannel(channel, true);

            //Registering the client object
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(ClientServer),
                "ClientServer",
                WellKnownObjectMode.Singleton);

            //TODO: Get location of the MetadataServer
            primaryMetadata = (IClientMetadata)Activator.GetObject(
               typeof(IClientMetadata),
               "tcp://localhost:8081/MetadataServer");

            System.Console.WriteLine("press <enter> to exit...");
            System.Console.ReadLine();
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
