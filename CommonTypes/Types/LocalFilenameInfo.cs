using System;

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

        public override bool Equals(object obj)
        {

            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to LocalFilenameInfo return false.
            LocalFilenameInfo localFilenameInfo = obj as LocalFilenameInfo;
            if ((Object)localFilenameInfo == null)
            {
                return false;
            }

            // Return true if the fields match:
            return location.Equals(localFilenameInfo.location) && localFilename.Equals(localFilenameInfo.localFilename);
        }

        public override int GetHashCode()
        {
            return location.GetHashCode() ^ localFilename.GetHashCode();
        }
    }
}
