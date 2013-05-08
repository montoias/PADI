using System;
using System.Xml.Serialization;

namespace CommonTypes
{
    [Serializable]
    [XmlInclude(typeof(CreateDTO))]
    [XmlInclude(typeof(DeleteDTO))]
    [XmlInclude(typeof(OpenDTO))]
    [XmlInclude(typeof(CloseDTO))]
    [XmlInclude(typeof(RegisterDTO))]
    [XmlInclude(typeof(QueueFileDTO))]
    [XmlInclude(typeof(UpdateMetadataDTO))]
    public abstract class InstructionDTO
    {
        public string type = "GENERAL";

        public InstructionDTO() { }
        public InstructionDTO(string type)
        {
            this.type = type;
        }
    }
}
