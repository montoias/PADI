using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    public interface IPuppetDataServer
    {
        void freeze();
        void unfreeze();
        void fail();
        void recover();
    }
}
