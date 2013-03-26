using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    public interface IPuppetMetadataServer
    {
        void fail();
        void recover();
        string dump();
    }
}
