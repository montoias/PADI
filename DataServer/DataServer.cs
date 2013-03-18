using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTypes;

namespace DataServer
{
    class DataServer : MarshalByRefObject, IClientServerDataServer, IMetadataServerDataServer, IPuppetDataServer
    {
        static void Main(string[] args)
        {
        }

        public void freeze()
        {
            throw new NotImplementedException();
        }

        public void unfreeze()
        {
            throw new NotImplementedException();
        }

        public void fail()
        {
            throw new NotImplementedException();
        }

        public void recover()
        {
            throw new NotImplementedException();
        }
    }
}
