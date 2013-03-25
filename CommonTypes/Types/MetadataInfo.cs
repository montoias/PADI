using System;

namespace CommonTypes
{
    [Serializable]
    public class MetadataInfo
    {
        //FIXME: Should these be public?
        public string filename;
        public int numDataServers;
        public int readQuorum;
        public int writeQuorum;
        public string dataServers; //TODO: change to IDictionary and make it serializable
        
        public MetadataInfo(string filename, int numDataServers, int readQuorum, int writeQuorum, string dataServers)
        {
            // TODO: Complete member initialization
            this.filename = filename;
            this.numDataServers = numDataServers;
            this.readQuorum = readQuorum;
            this.writeQuorum = writeQuorum;
            this.dataServers = dataServers;
        }

        public override string ToString()
        {
            String toReturn = this.filename;
            toReturn += "\r\n" + numDataServers;
            toReturn += "\r\n" + readQuorum;
            toReturn += "\r\n" + writeQuorum;

            //TODO: Iterate through dictionary
            toReturn += "\r\n" + dataServers;

            return toReturn;
        }
    }
}
