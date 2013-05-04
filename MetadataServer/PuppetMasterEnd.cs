using System.Net.Sockets;
using System.Threading;

namespace MetadataServer
{
    public partial class MetadataServer
    {
        private bool ignoringMessages = false;

        public void fail()
        {
            if (!ignoringMessages)
            {
                System.Console.WriteLine("Now ignoring messages.");
                ignoringMessages = true;
                backupReplicas.Clear();
                timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        public void recover()
        {
            if (ignoringMessages)
            {
                System.Console.WriteLine("Accepting messages again");
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
