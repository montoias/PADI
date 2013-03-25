using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    public interface IClientMetadata
    {
        MetadataInfo open(string filename);
        void close(string filename);
        MetadataInfo create(string filename, int numDataServers, int readQuorum, int writeQuorum);
        void delete(string filename);
    }
}
