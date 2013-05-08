using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CommonTypes
{
    [Serializable]
    [XmlRoot("MetadataServerState")]
    public class MetadataServerState
    {
        [XmlElement("openedFiles")]
        public SerializableDictionary<string, List<int>> openedFiles = new SerializableDictionary<string, List<int>>();

        [XmlElement("queueFiles")]
        public SerializableDictionary<string, MetadataInfo> queueFiles = new SerializableDictionary<string, MetadataInfo>();

        public List<int> dataServersList = new List<int>();
        public List<InstructionDTO> log = new List<InstructionDTO>();
        public int currentInstruction = 0;

        public MetadataServerState() { }

        public MetadataServerState(SerializableDictionary<string, List<int>> openedFiles, SerializableDictionary<string,
            MetadataInfo> queueFiles, List<int> dataServersList, int currentInstruction, List<InstructionDTO> log)
        {
            this.openedFiles = openedFiles;
            this.queueFiles = queueFiles;
            this.dataServersList = dataServersList;
            this.currentInstruction = currentInstruction;
            this.log = log;
        }

        public override string ToString()
        {
            string toReturn = "QUEUE FILES\r\n";
            foreach (KeyValuePair<string, MetadataInfo> entry in queueFiles)
            {
                toReturn += "Filename : " + entry.Key + "\r\n";
                toReturn += "Contents: \r\n" + entry.Value + "\r\n";
            }

            toReturn += "DATA SERVERS AVAILABLE\r\n";
            foreach (int dataServer in dataServersList)
            {
                toReturn += dataServer + "\r\n";
            }

            toReturn += "LOG\r\n";
            for (int i = 0; i < log.Count; i++)
            {
                toReturn += "[" + i + "] " + log[i] + "\r\n";
            }

            toReturn += "Current Instruction: " + currentInstruction + "\r\n";

            return toReturn;
        }
    }
}
