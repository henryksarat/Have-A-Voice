using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

//Henryk:
//Great custom myException tutorial:
//http://blog.gurock.com/articles/creating-custom-exceptions-in-dotnet/
namespace HaveAVoice.Exceptions {
    [Serializable]
    public class NullRoleException : Exception {
        public NullRoleException() {
        }

        public NullRoleException(string message)
            : base(message) {
        }

        public NullRoleException(string message,
           Exception innerException)
            : base(message, innerException) {
        }

        protected NullRoleException(SerializationInfo info,
           StreamingContext context)
            : base(info, context) {
        }
    }
}