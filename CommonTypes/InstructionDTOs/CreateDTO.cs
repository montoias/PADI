using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTypes
{
    [Serializable]
    public class CreateDTO : InstructionDTO
    {
        public MetadataInfo metadataInfo;
        
        private CreateDTO() { }

        public CreateDTO(MetadataInfo metadataInfo)
        {
            base.type = "CREATE";
            this.metadataInfo = metadataInfo;
        }

        public override string ToString()
        {
            return "CREATE " + metadataInfo;
        }
    }
}
