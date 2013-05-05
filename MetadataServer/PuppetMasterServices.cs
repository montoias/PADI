using CommonTypes;
using System.IO;
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
                heartbeatTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        public void recover()
        {
            if (ignoringMessages)
            {
                System.Console.WriteLine("Accepting messages again");
                ignoringMessages = false;

                int currentInstruction = 0;

                if (File.Exists(stateFile))
                {
                    metadataState = Utils.deserializeObject<MetadataServerState>(stateFile);
                    currentInstruction = metadataState.currentInstruction;
                }

                if (findAvailableMetadatas(port))
                {
                    System.Console.WriteLine("I'm not the primary...");
                    System.Console.WriteLine("Notifying the primary metadata that I'm up!");
                    primaryServer.requestState(port);
                    heartbeatTimer.Change(tickerPeriod, tickerPeriod);

                    executeInstructions(metadataState.log, currentInstruction);
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
