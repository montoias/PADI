using CommonTypes;
using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using System.Windows.Forms;

namespace DataServer
{
    public partial class DataServer : MarshalByRefObject, IDataServerClient, IDataServerPuppet, IDataServerMetadataServer
    {
        private string fileFolder;
        private int dataServerId;
        private int[] metadataLocations = new int[3];
        private IMetadataServerDataServer[] metadataServer = new IMetadataServerDataServer[3];
        private IMetadataServerDataServer primaryMetadata;
        private bool ignoringMessages = false;
        private bool isFrozen = false;
        private static int port;

        static void Main(string[] args)
        {
            port = Convert.ToInt32(args[0]);
            TcpChannel channel = (TcpChannel)Helper.GetChannel(Convert.ToInt32(port), true);
            ChannelServices.RegisterChannel(channel, true);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(DataServer),
                "DataServer",
                WellKnownObjectMode.Singleton);

            
            System.Console.ReadLine();
        }

        private void findPrimaryMetadata()
        {
            System.Console.WriteLine("Finding available metadatas...");

            foreach (int location in metadataLocations)
            {
                try
                {
                    IMetadataServerDataServer replica = getMetadataServer(location);
                    int primaryServerLocation = replica.getPrimaryMetadataLocation(); //hack : triggering an exception
                    primaryMetadata = primaryServerLocation.Equals(location) ? replica : getMetadataServer(primaryServerLocation);
                    Thread.Sleep(1000);
                    return;
                }
                catch (SocketException)
                {
                    //ignore, means the server is down
                }
                catch (IOException)
                {
                    //ignore, means the server is down
                }
            }
        }

        private static IMetadataServerDataServer getMetadataServer(int location)
        {
            return (IMetadataServerDataServer)Activator.GetObject(
                typeof(IMetadataServerDataServer),
                "tcp://localhost:" + location + "/MetadataServer");
        }

        private void setMetadataLocation(int[] metadataList)
        {
            metadataLocations = metadataList;
        }

        public string dump()
        {
            string contents = "";
            
            contents += "Primary metadata @: ";

            try
            {
                contents += primaryMetadata.getPrimaryMetadataLocation() + "\r\n";
            }
            catch (SocketException)
            {
                contents += "Undetermined location \r\n";
            }
            catch (IOException)
            {
                contents += "Undetermined location \r\n";
            }


            contents += "DATA SERVER FOLDER\r\n";
            foreach (string filename in Directory.GetFiles(fileFolder))
                contents += filename + "\r\n";

            System.Console.WriteLine(contents);
            return contents;
        }

        public void init(int[] metadataList)
        {

            dataServerId = Convert.ToInt32(port) - 9000;
            setMetadataLocation(metadataList);

            fileFolder = Path.Combine(Application.StartupPath, "Files_" + port);
            Utils.createFolderFile(fileFolder);

            findPrimaryMetadata();
            registerDataServer(port);

            System.Console.WriteLine("Data Server " + dataServerId + " was launched!...");
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
