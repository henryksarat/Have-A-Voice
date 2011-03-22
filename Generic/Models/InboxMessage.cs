using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social.Generic.Models {
    public class InboxMessage {
        public int MessageId { get; set; }
        public string LastReply { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public string Subject { get; set; }
        public string FromUser { get; set; }
        public int FromUserId { get; set; }
        public bool Viewed { get; set; }
        public string FromUserProfilePictureUrl { get; set; }

        public InboxMessage() {
            LastReply = string.Empty;
            Subject = string.Empty;
            FromUser = string.Empty;
            Viewed = true;
            FromUserProfilePictureUrl = Constants.Constants.NO_PROFILE_PICTURE_URL;
        }
    }
}
