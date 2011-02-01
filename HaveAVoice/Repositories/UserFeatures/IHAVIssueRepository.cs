using System.Collections.Generic;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVIssueRepository {
        IEnumerable<Issue> GetLatestIssues();
        IEnumerable<IssueWithDispositionModel> GetIssues(User aUser);
        Issue CreateIssue(Issue anIssueToCreate, User aUserCreating);
        Issue GetIssue(int anIssueId);
        IssueReply CreateIssueReply(Issue anIssue, User aUserCreating, string aReply, bool anAnonymous, Disposition aDisposition);
        IssueReply CreateIssueReply(User aUserCreating, int anIssueId, string aReply, bool anAnonymous, int aDisposition);
        IEnumerable<IssueReplyModel> GetReplysToIssue(User aUser, Issue anIssue, IEnumerable<string> aSelectedRoles, PersonFilter aFilter);
        IssueReply GetIssueReply(int anIssueReplyId);
        IEnumerable<IssueReplyComment> GetIssueReplyComments(int anIssueReplyId);
        IssueReplyComment CreateCommentToIssueReply(IssueReply anIssueReply, User aUserCreating, string aComment);
        void CreateCommentToIssueReply(User aUserCreating, int anIssueReplyId, string aComment);
        IssueReplyComment GetIssueReplyComment(int anIssueReplyCommentId);

        bool HasIssueDisposition(User aUser, int anIssueId);
        void CreateIssueDisposition(User aUser, int anIssueId, int aDisposition);
        bool HasIssueReplyDisposition(User aUser, int anIssueReplyId);
        void CreateIssueReplyDisposition(User aUser, int anIssueReplyId, int aDisposition);
        
        void UpdateIssue(User aUser, Issue anOriginal, Issue anNew, bool anOverride);
        void UpdateIssueReply(User aUser, IssueReply anOriginal, IssueReply aNew, bool anOverride);
        void UpdateIssueReplyComment(User aUser, IssueReplyComment anOriginal, IssueReplyComment aNew, bool anOverride);
        
        void DeleteIssue(User aDeletingUser, Issue anIssue, bool anAdminDelete);
        void DeleteIssueReply(User aDeletingUser, IssueReply anIssueReply, bool anAdminDelete);
        void DeleteIssueReplyComment(User aDeletingUser, IssueReplyComment anIssueReplyComment, bool anAdminDelete);
    }
}