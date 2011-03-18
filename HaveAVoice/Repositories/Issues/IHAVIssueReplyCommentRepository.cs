using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.Issues {
    public interface IHAVIssueReplyCommentRepository {
        void CreateCommentToIssueReply(User aUserCreating, int anIssueReplyId, string aComment);
        void DeleteIssueReplyComment(User aDeletingUser, IssueReplyComment anIssueReplyComment, bool anAdminDelete);
        IssueReplyComment GetIssueReplyComment(int anIssueReplyCommentId);
        IEnumerable<IssueReplyComment> GetIssueReplyComments(int anIssueReplyId);
        void UpdateIssueReplyComment(User aUser, IssueReplyComment anOriginal, IssueReplyComment aNew, bool anOverride);
    }
}