using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace CommonTypes.Exceptions
{
    class TableSizeExcedeedException : ApplicationException
    {
        protected TableSizeExcedeedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
 
 
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter =true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
          base.GetObjectData(info, context);
        }

    }
}
