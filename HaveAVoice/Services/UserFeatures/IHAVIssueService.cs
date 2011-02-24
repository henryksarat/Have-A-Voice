using System.Collections.Generic;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVIssueService {
        IEnumerable<Issue> GetLatestIssues();
        IEnumerable<IssueWithDispositionModel> GetIssues(User aUser);

        IssueModel CreateIssueModel(UserInformationModel myUserInfo, int anIssueId);
        IssueModel CreateIssueModel(User aViewingUser, string aTitle);
        IssueModel CreateIssueModel(string aTitle);

        bool CreateIssue(UserInformationModel aUserCreating, Issue anIssueToCreate);
        bool CreateIssueReply(UserInformationModel aUserCreating, IssueModel anIssueModel);
        bool CreateIssueReply(UserInformationModel aUserCreating, int anIssueId, string aReply, int aDisposition, bool anAnonymous);
        bool CreateCommentToIssueReply(UserInformationModel aUserCreating, int anIssueReplyId, string aComment);

        Issue GetIssue(int anIssueId, UserInformationModel aViewingUser);
        IEnumerable<IssueReplyModel> GetReplysToIssue(User aUser, Issue anIssue, IEnumerable<string> aRoleName, PersonFilter aFilter);
        IssueReply GetIssueReply(int anIssueReplyId);
        IssueReply GetIssueReply(User aViewingUser, int anIssueReplyId);
        IEnumerable<IssueReplyComment> GetIssueReplyComments(int anIssueReplyId);
        IssueReplyComment GetIssueReplyComment(int anIssueReplyCommentId);
        bool AddIssueDisposition(User aUser, int anIssueId, int aDisposition);
        bool AddIssueReplyDisposition(User aUser, int anIssueReplyId, int aDisposition);

        bool EditIssue(UserInformationModel aUserCreating, Issue anIssue);
        bool EditIssueReply(UserInformationModel aUserCreating, IssueReply anIssueReply);
        bool EditIssueReplyComment(UserInformationModel aUserCreating, IssueReplyComment aComment);

        bool DeleteIssue(UserInformationModel aDeletingUser, int anIssueId);
        bool DeleteIssueReply(UserInformationModel aDeletingUser, int anIssueReplyId);
        bool DeleteIssueReplyComment(UserInformationModel aDeletingUser, int anIssueReplyCommentId);
    }
}