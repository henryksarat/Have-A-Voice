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
    public class NullUserException : Exception {
        public NullUserException() {
        }

        public NullUserException(string message)
            : base(message) {
        }

        public NullUserException(string message,
           Exception innerException)
            : base(message, innerException) {
        }

        protected NullUserException(SerializationInfo info,
           StreamingContext context)
            : base(info, context) {
        }
    }
}