using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using CommonTypes;

namespace DataServer
{
    public partial class DataServer : MarshalByRefObject, IDataServerClient, IDataServerPuppet, IDataServerMetadataServer
    {
        public void freeze()
        {
            lock (this)
            {
                System.Console.WriteLine("Now delaying messages.");
                isFrozen = true;
            }
        }

        public void unfreeze()
        {
            lock (this)
            {
                if (isFrozen)
                {
                    System.Console.WriteLine("Receiving messages normally again.");
                    Monitor.PulseAll(this);
                    isFrozen = false;
                }
            }
        }

        public void fail()
        {
            System.Console.WriteLine("Now ignoring messages.");
            ignoringMessages = true;
        }

        public void recover()
        {
            System.Console.WriteLine("Accepting messages again");
            ignoringMessages = false;
        }

        private void checkFailure()
        {
            if (ignoringMessages)
                throw new SocketException();
        }
    }
}
