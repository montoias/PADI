using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonInterfaces;

namespace DataServer
{
    class DataServer : MarshalByRefObject, IClientServerDataServer, IMetadataServerDataServer, IPuppetDataServer
    {
        static void Main(string[] args)
        {
        }
    }
}
