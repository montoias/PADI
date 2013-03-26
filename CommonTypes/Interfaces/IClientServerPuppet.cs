using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CommonTypes
{
    public interface IClientServerPuppet
    {
        MetadataInfo create(string filename, int numDataServers, int readQuorum, int writeQuorum);
        MetadataInfo open(string filename);
        void close(string filename);
        void delete(string filename);
        FileData read(string filename, int semantics);
        void write(string filename, byte[] file);
    }
}
