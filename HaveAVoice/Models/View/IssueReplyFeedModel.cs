using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.View {
    public class IssueReplyFeedModel : FeedModel {
        public DateTime DateTimeStamp { get; set; }
        public IEnumerable<IssueReplyComment> IssueReplyComments { get; set; }
        public Issue Issue { get; set; }
        public string Reply { get; set; }
        public int TotalLikes { get; set; }
        public int TotalDislikes { get; set; }
        public bool HasDisposition { get; set; }
        public int TotalComments { get; set; }

        public IssueReplyFeedModel(User aUser) : base(aUser) {
            IssueReplyComments = new List<IssueReplyComment>();
        }
    }
}