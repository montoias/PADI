using CommonTypes;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    public partial class Client : MarshalByRefObject, IClientPuppet, IClientMetadataServer
    {
        private int[] metadataLocations = new int[3];
        private IMetadataServerClient primaryMetadata;
        private static int port;
        private int clientID;

        static void Main(string[] args)
        {
            port = Convert.ToInt32(args[0]);
            TcpChannel channel = (TcpChannel)Helper.GetChannel(port, true);
            ChannelServices.RegisterChannel(channel, true);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(Client),
                "Client",
                WellKnownObjectMode.Singleton);

            System.Console.ReadLine();
        }

        /* 
         * Initialization process. (avoids possible erratic behaviour when initializing multiple instances)
         */
        public void init(int[] metadataList)
        {
            clientID = 8000 - Convert.ToInt32(port);

            setMetadataLocation(metadataList);
            findPrimaryMetadata();

            System.Console.WriteLine("Client " + clientID + " was launched!...");
        }

        /*
         * Dumping mechanism. The following information is shown:
         * The current primary metadata;
         * The contents of file registers;
         * The contents of byte registers.
         */
        public string dump()
        {
            string toReturn = "";

            System.Console.WriteLine("Dumping Client information");
            toReturn += "Primary Metadata: " + primaryMetadata.getPrimaryMetadataLocation() + "\r\n";
            toReturn += "FileRegisters\r\n";

            for (int i = 0; i < fileRegisters.Length; i++)
            {
                if (fileRegisters[i] != null)
                {
                    toReturn += "FileRegister: " + i + "\r\n" + fileRegisters[i] + "\r\n";
                }
            }

            toReturn += "\r\n\r\nByteRegisters\r\n";

            for(int i = 0; i < byteRegisters.Length; i++)
            {
                if (byteRegisters[i] != null)
                {
                    toReturn += "ByteRegister: " + i + "\r\n";
                    toReturn += "Version:" + byteRegisters[i].version + "\r\n";
                    toReturn += Utils.byteArrayToString(byteRegisters[i].file) + "\r\n";
                }
            }

            System.Console.WriteLine(toReturn);
            return toReturn;
        }

        /* Function to find the current primary metadata. */
        private void findPrimaryMetadata()
        {
            System.Console.WriteLine("Finding available metadatas...");

            foreach (int location in metadataLocations)
            {
                try
                {
                    IMetadataServerClient replica = getMetadataServer(location);
                    int primaryServerLocation = replica.getPrimaryMetadataLocation(); //hack : triggering an exception
                    primaryMetadata = primaryServerLocation.Equals(location) ? replica : getMetadataServer(primaryServerLocation);
                    Thread.Sleep(1000);
                    return;
                }
                catch (SocketException)
                {
                    //ignore, means the server is down
                }
            }
        }

        private IMetadataServerClient getMetadataServer(int location)
        {
            return (IMetadataServerClient)Activator.GetObject(
                typeof(IMetadataServerClient),
                "tcp://localhost:" + location + "/MetadataServer");
        }

        private void setMetadataLocation(int[] args)
        {
            metadataLocations = args;
        }

        /*
         * This function is invoked by the metadata server, when a new data server becomes available 
         * and the client requested for a file that didn't have enough servers.
         */
        public void updateMetadata(string filename, MetadataInfo m)
        {
            if (fileIndexer.ContainsKey(filename))
            {
                Object key = fileRegisters[fileIndexer[filename]];
                System.Console.WriteLine("Updating metadata of " + filename + " @ object " + key.GetHashCode());
                lock (key)
                {
                    fileRegisters[fileIndexer[filename]] = m;
                    Monitor.PulseAll(key);
                }
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
