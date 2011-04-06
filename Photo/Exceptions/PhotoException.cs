using System;
using System.Runtime.Serialization;

//Henryk:
//Great custom myException tutorial:
//http://blog.gurock.com/articles/creating-custom-exceptions-in-dotnet/
namespace Social.Photo.Exceptions {
    [Serializable]
    public class PhotoException : Exception {
        public PhotoException() {
        }

        public PhotoException(string message)
            : base(message) {
        }

        public PhotoException(string message,
           Exception innerException)
            : base(message, innerException) {
        }

        protected PhotoException(SerializationInfo info,
           StreamingContext context)
            : base(info, context) {
        }
    }
}