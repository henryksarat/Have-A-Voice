using System.Collections.Generic;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVHomeRepository {
        IEnumerable<IssueWithDispositionModel> GetMostPopularIssues(Disposition aDisposition);
        IEnumerable<IssueReply> NewestIssueReplys();
        IEnumerable<IssueReply> GetMostPopularIssueReplys();

        void AddZipCodeFilter(User aUser, int aZipCode);
        void AddCityStateFilter(User aUser, string aCity, string aState);
        bool ZipCodeFilterExists(User aUser, int aZipCode);
        bool CityStateFilterExists(User aUser, string aCity, string aState);
    }
}