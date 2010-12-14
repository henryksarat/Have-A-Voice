using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;

namespace HaveAVoice.Models.View {
    public class IssueReplyDetailsModel {
        public IssueReply IssueReply { get; set; }
        public IEnumerable<IssueReplyComment> Comments { get; set; }
        public string Comment { get; set; }

        public IssueReplyDetailsModel(IssueReply anIssueReply, IEnumerable<IssueReplyComment> aComments) {
            this.IssueReply = anIssueReply;
            this.Comments = aComments;
            this.Comment = string.Empty;
        }
    }
}
