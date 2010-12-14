using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.View {
    public class LoggedInModel {
        public IEnumerable<IssueReply> FanIssueReplys { get; set; }
        public IEnumerable<IssueReply> OfficialsReplys { get; set; }

        public LoggedInModel() {
            FanIssueReplys = new List<IssueReply>();
            OfficialsReplys = new List<IssueReply>();
        }
    }
}