﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    public interface IMetadataServerDataServer
    {
        int getPrimaryMetadataLocation();
        void register(int location);
    }
}
