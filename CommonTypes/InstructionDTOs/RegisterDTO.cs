﻿using System;

namespace CommonTypes
{
    [Serializable]
    public class RegisterDTO : InstructionDTO
    {
        public int location;

        private RegisterDTO() { }

        public RegisterDTO(int location)
        {
            base.type = "REGISTER";
            this.location = location;
        }

        public override string ToString()
        {
            return "REGISTER " + location;
        }
    }
}
