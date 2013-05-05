using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
