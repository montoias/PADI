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
        public List<string> dataServers = new List<string>();

        private MetadataInfo() { }

        public MetadataInfo(string filename, int numDataServers, int readQuorum, int writeQuorum, List<string> dataServers)
        {
            this.filename = filename;
            this.numDataServers = numDataServers;
            this.readQuorum = readQuorum;
            this.writeQuorum = writeQuorum;
            this.dataServers = dataServers;
        }

        public override string ToString()
        {
            string toReturn = this.filename + "\r\n";
            toReturn += numDataServers + "\r\n";
            toReturn += readQuorum + "\r\n";
            toReturn +=  writeQuorum + "\r\n";
            toReturn += dataServersToString();

            return toReturn;
        }

        public string getDataServerLocation(string dataServer)
        {
            return (dataServer.Split(','))[0];
        }

        public string getLocalFilename(string dataServer)
        {
            return (dataServer.Split(','))[1];
        }
        public string dataServersToString()
        {
            string toReturn = "";

            foreach (string s in dataServers)
            {
                toReturn += s + " ";
            }

            return toReturn.Remove(toReturn.Length - 1);
        }
    }
}
