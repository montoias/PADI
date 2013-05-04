using System;
using System.Threading;
using System.Net.Sockets;
using CommonTypes;

namespace DataServer
{
    public partial class DataServer
    {
        public void freeze()
        {
            lock (this)
            {
                if (!isFrozen && !ignoringMessages)
                {
                    System.Console.WriteLine("Now delaying messages.");
                    isFrozen = true;
                }
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
            if (!ignoringMessages)
            {
                System.Console.WriteLine("Now ignoring messages.");
                ignoringMessages = true;
                isFrozen = false;
            }
        }

        public void recover()
        {
            if (ignoringMessages)
            {
                System.Console.WriteLine("Accepting messages again");
                ignoringMessages = false;
            }
        }

        private void checkFailure()
        {
            if (ignoringMessages)
                throw new SocketException();
        }
    }
}
