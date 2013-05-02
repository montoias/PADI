using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTypes;
using System.Net.Sockets;
using System.Threading;

namespace DataServer
{
    public partial class DataServer : MarshalByRefObject, IDataServerClient, IDataServerPuppet, IDataServerMetadataServer
    {

        private static void registerDataServer(int port)
        {
            try
            {
                System.Console.WriteLine("Object:" + primaryMetadata.GetHashCode());
                primaryMetadata.register(port);

            }
            catch (SocketException)
            {
                findPrimaryMetadata();
                System.Console.WriteLine("Object:" + primaryMetadata.GetHashCode());
                Thread.Sleep(1000);
                registerDataServer(port);
            }
        }
    }
}
