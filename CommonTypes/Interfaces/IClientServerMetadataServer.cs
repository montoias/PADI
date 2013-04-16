using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CommonTypes
{
    public interface  IClientMetadataServer
    {
        void updateMetadata(string filename, MetadataInfo m);
    }
}
