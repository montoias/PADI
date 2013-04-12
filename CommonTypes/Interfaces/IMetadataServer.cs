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
        void notify(string notifier); //Should send the log to the other metadata
        void sendInstruction(string instruction);
        void receiveInstruction(string instruction);
        void receiveNotify(string log);
    }
}
