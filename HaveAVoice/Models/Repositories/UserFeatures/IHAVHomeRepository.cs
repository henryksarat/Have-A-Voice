﻿using System.Collections.Generic;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;

namespace HaveAVoice.Models.Repositories.UserFeatures {
    public interface IHAVHomeRepository {
        IEnumerable<IssueWithDispositionModel> GetMostPopularIssues(Disposition aDisposition);
        IEnumerable<IssueReply> NewestIssueReplys();
        IEnumerable<IssueReply> GetMostPopularIssueReplys();
        IEnumerable<IssueReply> FanFeed(User aUser);
        IEnumerable<IssueReply> OfficialsFeed(IEnumerable<string> aSelectRoles);
        IEnumerable<IssueReply> FilteredFeed(User aUser);
        void AddZipCodeFilter(User aUser, int aZipCode);
        void AddCityStateFilter(User aUser, string aCity, string aState);
        bool ZipCodeFilterExists(User aUser, int aZipCode);
        bool CityStateFilterExists(User aUser, string aCity, string aState);
    }
}