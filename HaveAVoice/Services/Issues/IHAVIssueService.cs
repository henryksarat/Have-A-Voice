using System.Collections.Generic;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models;
using Social.Generic.Models;
using HaveAVoice.Helpers.Search;

namespace HaveAVoice.Services.Issues {
    public interface IHAVIssueService {
        bool AddIssueStance(User aUser, int anIssueId, int aDisposition);
        bool CreateIssue(UserInformationModel<User> aUserCreating, Issue anIssueToCreate);
        IssueModel CreateIssueModel(UserInformationModel<User> myUserInfo, int anIssueId);
        IssueModel CreateIssueModel(User aViewingUser, string aTitle);
        IssueModel CreateIssueModel(string aTitle);
        bool DeleteIssue(UserInformationModel<User> aDeletingUser, int anIssueId);
        bool EditIssue(UserInformationModel<User> aUserCreating, Issue anIssue);
        Issue GetIssue(int anIssueId, UserInformationModel<User> aViewingUser);
        IEnumerable<Issue> GetIssueByTitleSearch(string aTitlePortion);
        IEnumerable<IssueWithDispositionModel> GetIssues(User aUser, SearchBy aSearchBy, OrderBy anOrderBy, string aSearchTerm);
        IEnumerable<Issue> GetMostPopularIssuesByHitCount(int aLimit);
        IEnumerable<Issue> GetNewestIssues(int aLimit);
        IDictionary<string, string> OrderByOptions();
        IDictionary<string, string> SearchByOptions();
    }
}