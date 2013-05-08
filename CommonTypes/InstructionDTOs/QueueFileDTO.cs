using System;

namespace CommonTypes
{
    [Serializable]
    public class QueueFileDTO : InstructionDTO
    {
        public LocalFilenameInfo localFilenameInfo;
        public string filename;

        private QueueFileDTO() { }

        public QueueFileDTO(string filename, LocalFilenameInfo dataServerInfo)
        {
            base.type = "QUEUE";
            this.filename = filename;
            this.localFilenameInfo = dataServerInfo;
        }

        public override string ToString()
        {
            return type + " " + localFilenameInfo;
        }
    }
}
