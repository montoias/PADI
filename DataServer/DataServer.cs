﻿using CommonTypes;
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

        private int WRITE_LOAD;
        private int READ_LOAD;

        private string projectFolder = Directory.GetParent(Application.StartupPath).Parent.FullName;

        static void Main(string[] args)
        {
            port = Convert.ToInt32(args[0]);
            TcpChannel channel = (TcpChannel)Helper.GetChannel(Convert.ToInt32(port));
            ChannelServices.RegisterChannel(channel, false);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(DataServer),
                "DataServer",
                WellKnownObjectMode.Singleton);

            System.Console.WriteLine("Registering server object @ port : " + port);
            
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
                //ignore, means the server is down
                catch (SocketException) { }
                catch (IOException) { }
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
            string contents = "Primary metadata @: ";

            try
            {
                contents += primaryMetadata.getPrimaryMetadataLocation() + "\r\n";
            }
            catch (SocketException)
            {
                contents += "Undetermined location \r\n";
            }


            contents += "DATA SERVER FOLDER\r\n";
            foreach (string filename in Directory.GetFiles(fileFolder))
                contents += "File: " + filename + "\r\n" + "Contents: " + Utils.deserializeObject<FileData>(Path.Combine(fileFolder, filename)) + "\r\n";

            System.Console.WriteLine(contents);
            return contents;
        }

        public void init(int[] metadataList)
        {

            dataServerId = Convert.ToInt32(port) - 9000;
            setMetadataLocation(metadataList);

            string[] scriptLines = File.ReadAllLines(Path.Combine(projectFolder, "config.ini"));
            WRITE_LOAD = Convert.ToInt32((scriptLines[0].Split('='))[1]);
            READ_LOAD = Convert.ToInt32((scriptLines[1].Split('='))[1]);

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
