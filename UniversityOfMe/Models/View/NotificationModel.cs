using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityOfMe.Models.View {
    public class NotificationModel {
        public User WhoSent { get; set; }
        public string Url { get; set; }
        public string DisplayText { get; set; }
    }
}