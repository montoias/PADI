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
        void freeze();
        void unfreeze();
        void fail();
        void recover();
    }
}
