using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTypes;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace DataServer
{
    public partial class DataServer
    {

        private void registerDataServer(int port)
        {
            try
            {
                primaryMetadata.register(port);
            }
            catch (SocketException)
            {
                retryRegister(port);
            }
            catch (IOException)
            {
                retryRegister(port);
            }
        }

        private void retryRegister(int port)
        {
            findPrimaryMetadata();
            registerDataServer(port);
        }
    }
}
