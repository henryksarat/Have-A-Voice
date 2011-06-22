using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Models.View {
    public class NotificationModel {
        public NotificationType NotificationType { get; set; }
        public Class Class { get; set; }
        public Club Club { get; set; }
        public Board Board { get; set; }
        public bool IsMyBoard { get; set; }
        public User ClubMemberUser { get; set; }
        public SendItemOptions SendItem { get; set; }
        public User WhoSent { get; set; }
        public DateTime DateTimeSent { get; set; }
    }
}