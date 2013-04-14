using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    public interface IMetadataServerClientServer
    {
        MetadataInfo open(string filename, int location);
        void close(string filename, int location);
        MetadataInfo create(string filename, int numDataServers, int readQuorum, int writeQuorum);
        void delete(string filename);
    }
}
