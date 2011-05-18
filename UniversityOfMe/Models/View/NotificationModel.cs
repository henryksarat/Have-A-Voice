using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Models.View {
    public class NotificationModel {
        public SendItemOptions SendItem { get; set; }
        public User WhoSent { get; set; }
        public string FormattedDateTimeSent { get; set; }
    }
}