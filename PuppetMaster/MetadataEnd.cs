using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonTypes;

namespace PuppetMaster
{
    public partial class PuppetMaster
    {
        private IMetadataServerPuppet[] metadataList = new IMetadataServerPuppet[3];

        public void failMetadata(int selectedMetadata)
        {
            metadataList[selectedMetadata].fail();
        }

        public void recoverMetadata(int selectedMetadata)
        {
            metadataList[selectedMetadata].recover();
        }

        public void dumpMetadataServer(int selectedMetadata)
        {
            metadataList[selectedMetadata].dump();
        }
    }
}
