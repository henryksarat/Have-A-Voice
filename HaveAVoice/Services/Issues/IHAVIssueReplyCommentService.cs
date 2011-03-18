using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models.View;
using HaveAVoice.Models;

namespace HaveAVoice.Services.Issues {
    public interface IHAVIssueReplyCommentService {
        bool CreateCommentToIssueReply(UserInformationModel aUserCreating, int anIssueReplyId, string aComment);
        bool DeleteIssueReplyComment(UserInformationModel aDeletingUser, int anIssueReplyCommentId);
        bool EditIssueReplyComment(UserInformationModel aUserEditing, IssueReplyComment aComment);
        IssueReplyComment GetIssueReplyComment(int anIssueReplyCommentId);
        IEnumerable<IssueReplyComment> GetIssueReplyComments(int anIssueReplyId);
    }
}