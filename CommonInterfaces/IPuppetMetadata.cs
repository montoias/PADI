using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTypes;

namespace CommonInterfaces
{
    public interface IPuppetMetadata
    {
        void fail();
        void recover();
    }
}
