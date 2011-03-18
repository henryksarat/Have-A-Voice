using System.Collections.Generic;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models;

namespace HaveAVoice.Services.Issues {
    public interface IHAVIssueService {
        bool AddIssueStance(User aUser, int anIssueId, int aDisposition);
        bool CreateIssue(UserInformationModel aUserCreating, Issue anIssueToCreate);
        IssueModel CreateIssueModel(UserInformationModel myUserInfo, int anIssueId);
        IssueModel CreateIssueModel(User aViewingUser, string aTitle);
        IssueModel CreateIssueModel(string aTitle);
        bool DeleteIssue(UserInformationModel aDeletingUser, int anIssueId);
        bool EditIssue(UserInformationModel aUserCreating, Issue anIssue);
        Issue GetIssue(int anIssueId, UserInformationModel aViewingUser);
        IEnumerable<Issue> GetIssueByTitleSearch(string aTitlePortion);
        IEnumerable<IssueWithDispositionModel> GetIssues(User aUser);
        IEnumerable<Issue> GetMostPopularIssuesByHitCount(int aLimit);
        IEnumerable<Issue> GetNewestIssues(int aLimit);
    }
}