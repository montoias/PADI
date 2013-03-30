﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    [Serializable]
    public class FileData
    {
        public byte[] file;
        public int version;

        public FileData(byte[] file, int version)
        {
            this.file = file;
            this.version = version;
        }
    }
}