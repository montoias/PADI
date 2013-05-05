using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTypes
{
    [Serializable]
    public class OpenDTO : InstructionDTO
    {
        public string filename;
        public int location;
        
        private OpenDTO() { }

        public OpenDTO(string filename, int location)
        {
            base.type = "OPEN";
            this.filename = filename;
            this.location = location;
        }

        public override string ToString()
        {
            return "OPEN " + filename + "," + location;
        }
    }
}
