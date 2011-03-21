using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.Enums;
using Social.Generic.Models;

namespace HaveAVoice.Services.Issues {
    public interface IHAVIssueReplyService {
        bool AddIssueReplyStance(User aUser, int anIssueReplyId, int aStance);
        bool CreateIssueReply(UserInformationModel<User> aUserCreating, IssueModel anIssueModel);
        bool CreateIssueReply(UserInformationModel<User> aUserCreating, int anIssueId, string aReply, int aDisposition, bool anAnonymous);
        bool EditIssueReply(UserInformationModel<User> aUserEditing, IssueReply anIssueReply);
        bool DeleteIssueReply(UserInformationModel<User> aDeletingUser, int anIssueReplyId);
        IssueReply GetIssueReply(int anIssueReplyId);
        IssueReply GetIssueReply(User aViewingUser, int anIssueReplyId);
        IEnumerable<IssueReplyModel> GetReplysToIssue(User aUser, Issue anIssue, IEnumerable<string> aRoleNames, PersonFilter aFilter);
    }
}