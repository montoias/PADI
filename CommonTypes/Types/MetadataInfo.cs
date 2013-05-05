using System;
using System.Collections.Generic;

namespace CommonTypes
{
    [Serializable]
    public class MetadataInfo
    {
        public string filename;
        public int numDataServers;
        public int readQuorum;
        public int writeQuorum;
        public List<LocalFilenameInfo> dataServers = new List<LocalFilenameInfo>();

        private MetadataInfo() { }

        public MetadataInfo(string filename, int numDataServers, int readQuorum, int writeQuorum, List<LocalFilenameInfo> dataServers)
        {
            this.filename = filename;
            this.numDataServers = numDataServers;
            this.readQuorum = readQuorum;
            this.writeQuorum = writeQuorum;
            this.dataServers = dataServers;
        }

        public override string ToString()
        {
            return filename + ", " + numDataServers + "," + readQuorum + "," + writeQuorum + "\r\n" + dataServersToString();
        }

        public string dataServersToString()
        {
            string toReturn = "";

            foreach (LocalFilenameInfo localFilenameInfo in dataServers)
            {
                toReturn += localFilenameInfo + "\r\n";
            }

            return toReturn.Remove(toReturn.Length - 1);
        }
    }
}
