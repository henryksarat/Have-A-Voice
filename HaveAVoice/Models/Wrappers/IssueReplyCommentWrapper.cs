using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models {
    public class IssueReplyCommentWrapper : BasicTextModelWrapper{
        public int IssueReplyId { get; set; }

        public IssueReplyComment ToModel() {
            return IssueReplyComment.CreateIssueReplyComment(Id, IssueReplyId, Body, DateTime.UtcNow, 0, false);
        }

        public static IssueReplyCommentWrapper Build(IssueReplyComment aComment) {
            return new IssueReplyCommentWrapper() {
                Id = aComment.Id,
                IssueReplyId = aComment.IssueReplyId,
                Body = aComment.Comment
            };
        }
    }
}