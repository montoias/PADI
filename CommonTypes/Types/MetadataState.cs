using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTypes
{
    [Serializable]
    public class MetadataState
    {
        Dictionary<string, List<int>> openedFiles;
        Dictionary<string, MetadataInfo> queueFiles;
        List<string> dataServersList;

        private MetadataState() { }

        public MetadataState(Dictionary<string, List<int>> openedFiles, Dictionary<string, MetadataInfo> queueFiles, List<string> dataServersList)
        {
            this.openedFiles = openedFiles;
            this.queueFiles = queueFiles;
            this.dataServersList = dataServersList;
        }
    }
}
