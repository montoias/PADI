using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    [Serializable]
    public class FileData
    {
        public byte[] file;
        public int version;
        public int clientID;

        private FileData(){}

        public FileData(byte[] file, int version, int clientID)
        {
            this.file = file;
            this.version = version;
            this.clientID = clientID;
        }
    }
}
