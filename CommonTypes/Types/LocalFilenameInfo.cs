using System;
using System.Collections.Generic;

namespace CommonTypes
{
    [Serializable]
    public class LocalFilenameInfo
    {
        public int location;
        public string localFilename;

        private LocalFilenameInfo() { }

        public LocalFilenameInfo(int location, string localFilename)
        {
            this.location = location;
            this.localFilename = localFilename;
        }

        public override string ToString()
        {
            return location + "," + localFilename;
        }
    }
}
