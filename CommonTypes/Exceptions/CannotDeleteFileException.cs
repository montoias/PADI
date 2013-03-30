﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace CommonTypes
{
    [Serializable()]
    public class CannotDeleteFileException : ApplicationException
    {
        public CannotDeleteFileException() : base() { }
        public CannotDeleteFileException(SerializationInfo info, StreamingContext context) : base(info, context) { }
  
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter =true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
          base.GetObjectData(info, context);
        }

    }
}