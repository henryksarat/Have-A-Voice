using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social.Generic.Models {
    public class InboxMessage<T> {
        public int MessageId { get; set; }
        public string LastReply { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public string Subject { get; set; }
        public T FromUser { get; set; }
        public bool Viewed { get; set; }

        public InboxMessage() {
            LastReply = string.Empty;
            Subject = string.Empty;
            Viewed = true;
        }
    }
}
