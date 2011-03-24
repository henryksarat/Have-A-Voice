using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

//Henryk:
//Great custom exception tutorial:
//http://blog.gurock.com/articles/creating-custom-exceptions-in-dotnet/
namespace Social.Generic.Exceptions {
    [Serializable]
    public class NotFriendException : Exception {
        public NotFriendException() {
        }

        public NotFriendException(string message)
            : base(message) {
        }

        public NotFriendException(string message,
           Exception innerException)
            : base(message, innerException) {
        }

        protected NotFriendException(SerializationInfo info,
           StreamingContext context)
            : base(info, context) {
        }
    }
}