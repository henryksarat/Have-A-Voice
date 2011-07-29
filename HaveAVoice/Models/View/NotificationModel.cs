using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Models.View {
    public class NotificationModel {
        public NotificationType NotificationType { get; set; }
        public string Label { get; set; }
        public string Id { get; set; }
        public string SecondaryId { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public User TriggeredUser { get; set; }
    }
}