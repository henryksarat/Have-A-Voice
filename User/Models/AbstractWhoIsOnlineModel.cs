using System;
using Social.Generic.Models;

namespace Social.User.Models {
    public abstract class AbstractWhoIsOnlineModel<T> : AbstractSocialModel<T> {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public string IpAddress { get; set; }
        public bool ForceLogOut { get; set; }

        public abstract T FromModel();
    }
}
