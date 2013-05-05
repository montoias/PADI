using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTypes
{
    [Serializable]
    public class CloseDTO : InstructionDTO
    {
        public string filename;
        public int location;
        
        private CloseDTO() { }

        public CloseDTO(string filename, int location)
        {
            base.type = "CLOSE";
            this.filename = filename;
            this.location = location;
        }

        public override string ToString()
        {
            return "CLOSE " + filename + "," + location;
        }
    }
}
