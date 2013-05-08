using System;

namespace CommonTypes
{
    [Serializable]
    public class UpdateMetadataDTO : InstructionDTO
    {
        public MetadataInfo metadataInfo;

        private UpdateMetadataDTO() { }

        public UpdateMetadataDTO(MetadataInfo metadataInfo)
        {
            base.type = "UPDATE";
            this.metadataInfo = metadataInfo;
        }

        public override string ToString()
        {
            return type + " " + metadataInfo;
        }
    }
}
