﻿using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace CommonTypes
{
    [Serializable()]
    public class FileAlreadyOpenedException : ApplicationException
    {
        public FileAlreadyOpenedException() : base() { }
        public FileAlreadyOpenedException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }
}
