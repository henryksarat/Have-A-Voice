using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Social.Generic.Models {
    //T = Message
    //U = User
    public abstract class AbstractMessageModel<T, U> : AbstractSocialModel<T> {
        public int Id { get; set; }
        public int FromUserId { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FromUserFullName { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FromUserProfilePictureUrl { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int ToUserId { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ToUserFullName { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ToUserProfilePictureUrl { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Subject { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Body { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public bool ToViewed { get; set; }
        public bool FromViewed { get; set; }
        public bool ToDeleted { get; set; }
        public bool FromDeleted { get; set; }
        public bool RepliedTo { get; set; }

        public U FromUser { get; set; }

        public abstract T FromModel();
    }
}
