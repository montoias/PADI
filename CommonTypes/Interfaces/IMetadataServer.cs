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
        void checkState();
        void sendInstruction(string instruction);
        void receiveInstruction(string instruction);
        void receiveLog(string notifier); 
        void requestLog(string log);
    }
}
