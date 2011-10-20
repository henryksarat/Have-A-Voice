using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Models.View {
    public class NotificationModel {
        public int Id { get; set; }
        public NotificationType NotificationType { get; set; }
        public Class Class { get; set; }
        public Board Board { get; set; }
        public bool IsMine { get; set; }
        public SendItemOptions SendItem { get; set; }
        public User WhoSent { get; set; }
        public DateTime DateTimeSent { get; set; }
        public Badge Badge { get; set; }
    }
}