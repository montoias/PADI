﻿using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace CommonTypes
{
    [Serializable()]
    public class FileAlreadyExistsException : ApplicationException
    {

        public FileAlreadyExistsException() : base() { }
        public FileAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }
}
