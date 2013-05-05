using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CommonTypes
{
    public interface IMetadataServer
    {
        void checkpoint();
        void isAlive();
        int notifyMetadataServers(int location);
        int getMetadataID();
        void sendInstruction(InstructionDTO instruction);
        void receiveInstruction(InstructionDTO instruction);
        void receiveState(MetadataServerState statelog);
        void requestState(int notifier);
    }
}
