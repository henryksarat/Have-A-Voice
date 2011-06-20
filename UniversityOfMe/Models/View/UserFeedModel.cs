using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Models.View {
    public class UserFeedModel {
        public string FeedString { get; set; }
        public string CssClass { get; set; }
        public FeedType FeedType { get; set; }
        public DateTime DateTimeStamp { get; set; }
    }
}