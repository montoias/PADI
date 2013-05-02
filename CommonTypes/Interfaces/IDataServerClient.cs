using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    public interface IDataServerClient
    {
        FileData read(string filename);
        void write(string filename, FileData file);
    }
}
