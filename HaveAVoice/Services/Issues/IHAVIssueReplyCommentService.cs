using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models.View;
using HaveAVoice.Models;
using Social.Generic.Models;

namespace HaveAVoice.Services.Issues {
    public interface IHAVIssueReplyCommentService {
        bool CreateCommentToIssueReply(UserInformationModel<User> aUserCreating, int anIssueReplyId, string aComment);
        bool DeleteIssueReplyComment(UserInformationModel<User> aDeletingUser, int anIssueReplyCommentId);
        bool EditIssueReplyComment(UserInformationModel<User> aUserEditing, IssueReplyComment aComment);
        IssueReplyComment GetIssueReplyComment(int anIssueReplyCommentId);
        IEnumerable<IssueReplyComment> GetIssueReplyComments(int anIssueReplyId);
    }
}