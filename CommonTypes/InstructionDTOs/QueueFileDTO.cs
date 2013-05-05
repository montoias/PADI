using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTypes
{
    [Serializable]
    public class QueueFileDTO : InstructionDTO
    {
        public LocalFilenameInfo localFilenameInfo;
        
        private QueueFileDTO() { }

        public QueueFileDTO(LocalFilenameInfo dataServerInfo)
        {
            base.type = "QUEUE";
            this.localFilenameInfo = dataServerInfo;
        }

        public override string ToString()
        {
            return "QUEUE " + localFilenameInfo;
        }
    }
}
