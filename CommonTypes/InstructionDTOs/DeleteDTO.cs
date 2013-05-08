using System;

namespace CommonTypes
{
    [Serializable]
    public class DeleteDTO : InstructionDTO
    {
        public string filename;

        private DeleteDTO() { }

        public DeleteDTO(string filename)
        {
            base.type = "DELETE";
            this.filename = filename;
        }

        public override string ToString()
        {
            return "DELETE " + filename;
        }
    }
}
