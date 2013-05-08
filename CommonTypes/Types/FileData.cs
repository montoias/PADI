using System;

namespace CommonTypes
{
    [Serializable]
    public class FileData
    {
        public byte[] file;
        public int version;
        public int clientID;
        public string filename; //used to apply the workload redistribution

        private FileData() { }

        public FileData(byte[] file, int version, int clientID, string filename)
        {
            this.file = file;
            this.version = version;
            this.clientID = clientID;
            this.filename = filename;
        }

        public override string ToString()
        {
            return "Filename: " + filename + "r\n File: " + Utils.byteArrayToString(file) +
                " \r\n Version: " + version + "  ClientID: " + clientID;
        }
    }
}
