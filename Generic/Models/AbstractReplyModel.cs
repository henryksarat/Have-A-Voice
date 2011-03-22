using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social.Generic.Models {
    public abstract class AbstractReplyModel<T> : AbstractSocialModel<T> {
        public int Id { get; set; }
        public int ReplyUserId { get; set; }
        public int MessageId { get; set; }
        public string Body { get; set; }
        public DateTime DateTimeStamp { get; set; }

        public abstract T FromModel();
    }
}
