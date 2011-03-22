using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social.Generic.Models {
    public abstract class AbstractMessageModel<T> : AbstractSocialModel<T> {
        public int Id { get; set; }
        public string FromUserFullName { get; set; }
        public string FromUserProfilePictureUrl { get; set; }
        public int ToUserId { get; set; }
        public int FromUserId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public bool ToViewed { get; set; }
        public bool FromViewed { get; set; }
        public bool ToDeleted { get; set; }
        public bool FromDeleted { get; set; }
        public bool RepliedTo { get; set; }

        public abstract T FromModel();
    }
}
