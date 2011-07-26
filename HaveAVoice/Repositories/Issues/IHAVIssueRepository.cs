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
        Issue GetIssue(int anIssueId);
        Issue GetIssueByTitle(string aTitle);
        IEnumerable<IssueWithDispositionModel> GetIssues(User aUser);
        IEnumerable<IssueWithDispositionModel> GetIssuesByTitle(User aUser, string aSearchTerm);
        IEnumerable<IssueWithDispositionModel> GetIssuesByDescription(User aUser, string aSearchTerm);
        IEnumerable<Issue> GetIssuesByTitle(string aTitlePortion);
        IEnumerable<Issue> GetLatestIssues();
        IEnumerable<Issue> GetMostPopularIssuesByHitCount(int aLimit);
        IEnumerable<Issue> GetNewestIssues(int aLimit);
        bool HasIssueStance(User aUser, int anIssueId);
        bool HasIssueTitleBeenUsed(string aTitle);
        void MarkIssueAsReadForAuthor(Issue anIssue);
        void MarkIssueAsUnreadForAuthor(int anIssueId);
        void UpdateIssue(User aUser, Issue anOriginal, Issue anNew, bool anOverride);
    }
}