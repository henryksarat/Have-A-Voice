using System;
using System.Runtime.Serialization;

//Henryk:
//Great custom myException tutorial:
//http://blog.gurock.com/articles/creating-custom-exceptions-in-dotnet/
namespace Social.Generic.Exceptions {
    [Serializable]
    public class CustomException : Exception {
        public CustomException() {
        }

        public CustomException(string message)
            : base(message) {
        }

        public CustomException(string message,
           Exception innerException)
            : base(message, innerException) {
        }

        protected CustomException(SerializationInfo info,
           StreamingContext context)
            : base(info, context) {
        }
    }
}