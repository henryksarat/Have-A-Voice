using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

//Henryk:
//Great custom myException tutorial:
//http://blog.gurock.com/articles/creating-custom-exceptions-in-dotnet/
namespace Social.Admin.Exceptions {
    [Serializable]
    public class PermissionDenied : Exception {
        public PermissionDenied() {
        }

        public PermissionDenied(string message)
            : base(message) {
        }

        public PermissionDenied(string message,
           Exception innerException)
            : base(message, innerException) {
        }

        protected PermissionDenied(SerializationInfo info,
           StreamingContext context)
            : base(info, context) {
        }
    }
}