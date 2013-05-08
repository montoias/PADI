using System;
using System.Xml.Serialization;

namespace CommonTypes
{
    [Serializable]
    [XmlRoot("DataServerStats")]
    public class DataServerStats
    {
        public int location;

        [XmlElement("fileLoad")]
        public SerializableDictionary<string, int> fileLoad = new SerializableDictionary<string, int>();
        public SerializableDictionary<string, string> filesAccessed = new SerializableDictionary<string, string>();

        public int serverLoad;

        public DataServerStats() { this.serverLoad = 0; }

        public DataServerStats(int location)
        {
            this.location = location;
            this.serverLoad = 0;
        }

    }
}
