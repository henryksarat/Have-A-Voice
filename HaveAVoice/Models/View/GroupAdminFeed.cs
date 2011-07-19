using System;
using HaveAVoice.Helpers;
using Social.Generic.Helpers;

namespace HaveAVoice.Models.View {
    public class GroupAdminFeed {
        public int GroupMemberId { get; set; }
        public int GroupId { get; set; }
        public User AdminUser { get; set; }
        public User MemberUser { get; set; }
        public DateTime? DateTimeStamp { get; set; }
        public bool HasDetails { get; set; }
        public Status Status { get; set; }
        public FeedType FeedType { get; set; }
    }
}