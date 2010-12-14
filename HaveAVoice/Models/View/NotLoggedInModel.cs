using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.View {
    public class NotLoggedInModel {
        public IEnumerable<IssueWithDispositionModel> LikedIssues { get; set; }
        public IEnumerable<IssueWithDispositionModel> DislikedIssues { get; set; }
        public IEnumerable<IssueReply> NewestIssueReplys { get; set; }
        public IEnumerable<IssueReply> MostPopularIssueReplys { get; set; }
        public NotLoggedInModel() {
            LikedIssues = new List<IssueWithDispositionModel>();
            DislikedIssues = new List<IssueWithDispositionModel>();
            NewestIssueReplys = new List<IssueReply>();
            MostPopularIssueReplys = new List<IssueReply>();
        }
    }
}