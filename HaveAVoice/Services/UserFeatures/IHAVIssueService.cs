using System.Collections.Generic;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVIssueService {
        IEnumerable<Issue> GetLatestIssues();
        IEnumerable<IssueWithDispositionModel> GetIssues(User aUser);

        bool CreateIssue(UserInformationModel aUserCreating, Issue anIssueToCreate);
        bool CreateIssueReply(UserInformationModel aUserCreating, IssueModel anIssueModel);
        bool CreateIssueReply(UserInformationModel aUserCreating, int anIssueId, string aReply, int aDisposition, bool anAnonymous);
        bool CreateCommentToIssueReply(UserInformationModel aUserCreating, IssueReplyDetailsModel anIssueReply);
        bool CreateCommentToIssueReply(UserInformationModel aUserCreating, int anIssueReplyId, string aComment);

        Issue GetIssue(int anIssueId);
        IEnumerable<IssueReplyModel> GetReplysToIssue(User aUser, Issue anIssue, IEnumerable<string> aRoleName, PersonFilter aFilter);
        IssueReply GetIssueReply(int anIssueReplyId);
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