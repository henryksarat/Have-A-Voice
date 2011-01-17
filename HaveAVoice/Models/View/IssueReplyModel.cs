using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Models.View {
    public class IssueReplyModel {
        public Issue Issue { get; set; }
        public int Id { get; set; }
        public User User { get; set; }
        public string Reply { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public int CommentCount { get; set; }
        public bool Anonymous { get; set; }
        public bool HasDisposition { get; set; }
        public int IssueStance { get; set; }
    }
}
