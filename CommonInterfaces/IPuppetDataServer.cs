using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonInterfaces
{
    interface IPuppetDataServer
    {
        void freeze();
        void unfreeze();
        void fail();
        void recover();
    }
}
