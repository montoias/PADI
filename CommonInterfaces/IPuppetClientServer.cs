using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommonTypes;
using System.IO;

namespace CommonInterfaces
{
    public interface IPuppetClientServer
    {
        MetadataInfo pcreate(string filename, int numDataServers, int readQuorum, int writeQuorum);
        MetadataInfo popen(string filename);
        void pclose(string filename);
        void pdelete(string filename);
        FileData pread(string filename, int semantics);
        void pwrite(string filename, FileStream file);
    }
}
