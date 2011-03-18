using System.Collections.Generic;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.Issues {
    public interface IHAVIssueRepository {
        void AddHitToIssue(int anIssueId);
        Issue CreateIssue(Issue anIssueToCreate, User aUserCreating);
        void CreateIssueStance(User aUser, int anIssueId, int aStance);
        void DeleteIssue(User aDeletingUser, Issue anIssue, bool anAdminDelete);
        IEnumerable<IssueWithDispositionModel> GetIssues(User aUser);
        Issue GetIssue(int anIssueId);
        Issue GetIssueByTitle(string aTitle);
        bool HasIssueStance(User aUser, int anIssueId);
        bool HasIssueTitleBeenUsed(string aTitle);
        IEnumerable<Issue> GetIssuesByTitleContains(string aTitlePortion);
        IEnumerable<Issue> GetLatestIssues();
        IEnumerable<Issue> GetMostPopularIssuesByHitCount(int aLimit);
        IEnumerable<Issue> GetNewestIssues(int aLimit);
        void MarkIssueAsReadForAuthor(Issue anIssue);
        void MarkIssueAsUnreadForAuthor(int anIssueId);
        void UpdateIssue(User aUser, Issue anOriginal, Issue anNew, bool anOverride);
    }
}