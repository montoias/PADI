using CommonTypes;
using System.Collections.Generic;

namespace PuppetMaster
{
    public partial class PuppetMaster
    {
        private List<IDataServerPuppet> dataServersList = new List<IDataServerPuppet>();

        public void freezeDataServer(int selectedDataServer)
        {
            dataServersList[selectedDataServer].freeze();
        }

        public void unfreezeDataServer(int selectedDataServer)
        {
            dataServersList[selectedDataServer].unfreeze();
        }

        public void failDataServer(int selectedDataServer)
        {
            dataServersList[selectedDataServer].fail();
        }

        public void recoverDataServer(int selectedDataServer)
        {
            dataServersList[selectedDataServer].recover();
        }

        public void dumpDataServer(int selectedDataServer)
        {
            //form.showDumpMessage("DataServer " + selectedDataServer + "\r\n" + dataServersList[selectedDataServer].dump());
            dataServersList[selectedDataServer].dump();
        }
    }
}
