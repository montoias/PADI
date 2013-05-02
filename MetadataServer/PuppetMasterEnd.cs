using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTypes;
using System.Threading;
using System.Net.Sockets;

namespace MetadataServer
{
    public partial class MetadataServer : MarshalByRefObject, IMetadataServerClient, IMetadataServerPuppet, IMetadataServerDataServer, IMetadataServer
    {
        private bool ignoringMessages = false;

        public void fail()
        {
            System.Console.WriteLine("Now ignoring messages.");
            ignoringMessages = true;
            backupReplicas.Clear();
            timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public void recover()
        {
            System.Console.WriteLine("Accepting messages again");

            if (ignoringMessages)
            {
                ignoringMessages = false;
                if (findAvailableMetadatas(port))
                {
                    System.Console.WriteLine("I'm not the primary...");
                    notifyPrimaryMetadata(port);
                    timer.Change(tickerPeriod, tickerPeriod);
                }
            }
        }

        public void isAlive()
        {
            System.Console.WriteLine("Checking for failure... It is " + ignoringMessages);

            if (ignoringMessages)
                throw new SocketException();
        }
    }
}
