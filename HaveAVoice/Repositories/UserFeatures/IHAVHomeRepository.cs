using System.Collections.Generic;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVHomeRepository {
        IEnumerable<IssueWithDispositionModel> GetMostPopularIssues(Disposition aDisposition);
        IEnumerable<IssueReply> NewestIssueReplys();
        IEnumerable<IssueReply> GetMostPopularIssueReplys();

        IEnumerable<Issue> FilteredIssuesFeed(User aUser);
        IEnumerable<IssueReply> FilteredIssueReplysFeed(User aUser);
        
        IEnumerable<Issue> OfficialsIssueFeed(User aViewingUser, IEnumerable<string> aSelectRoles);
        IEnumerable<IssueReply> OfficialsIssueReplyFeed(User aViewingUser, IEnumerable<string> aSelectRoles);

        IEnumerable<Issue> FanIssueFeed(User aUser);
        IEnumerable<IssueReply> FanIssueReplyFeed(User aUser);

        IEnumerable<Issue> UserIssueFeed(int aTargetUserId);
        IEnumerable<IssueReply> UserIssueReplyFeed(int aTargetUserId);

        void AddZipCodeFilter(User aUser, int aZipCode);
        void AddCityStateFilter(User aUser, string aCity, string aState);
        bool ZipCodeFilterExists(User aUser, int aZipCode);
        bool CityStateFilterExists(User aUser, string aCity, string aState);
    }
}