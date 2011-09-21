using System;
using System.Runtime.Serialization;

//Henryk:
//Great custom myException tutorial:
//http://blog.gurock.com/articles/creating-custom-exceptions-in-dotnet/
namespace Social.Generic.Exceptions {
    [Serializable]
    public class NotActivatedException : Exception {
        public NotActivatedException() {
        }

        public NotActivatedException(string message)
            : base(message) {
        }

        public NotActivatedException(string message,
           Exception innerException)
            : base(message, innerException) {
        }

        protected NotActivatedException(SerializationInfo info,
           StreamingContext context)
            : base(info, context) {
        }
    }
}