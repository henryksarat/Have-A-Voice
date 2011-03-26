using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

//Henryk:
//Great custom myException tutorial:
//http://blog.gurock.com/articles/creating-custom-exceptions-in-dotnet/
namespace Social.Email.Exceptions {
    [Serializable]
    public class EmailException : Exception {
        public EmailException() {
        }

        public EmailException(string message)
            : base(message) {
        }

        public EmailException(string message,
           Exception innerException)
            : base(message, innerException) {
        }

        protected EmailException(SerializationInfo info,
           StreamingContext context)
            : base(info, context) {
        }
    }
}