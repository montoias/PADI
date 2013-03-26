using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    public interface IDataServerClientServer
    {
        FileData read(string filename, int semantics);
        void write(string filename, byte[]file);
    }
}
