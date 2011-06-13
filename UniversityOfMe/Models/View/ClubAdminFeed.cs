using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Models.View {
    public class ClubAdminFeed {
        public int ClubMemberId { get; set; }
        public int ClubId { get; set; }
        public User AdminUser { get; set; }
        public User MemberUser { get; set; }
        public DateTime? DateTimeStamp { get; set; }
        public bool HasDetails { get; set; }
        public Status Status { get; set; }
    }
}