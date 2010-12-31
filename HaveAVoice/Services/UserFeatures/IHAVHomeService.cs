using System.Collections.Generic;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVHomeService {
        NotLoggedInModel NotLoggedIn();

        LoggedInModel<FeedModel> FanFeed(User aUser);
        LoggedInModel<FeedModel> OfficialsFeed(User aUser);
        LoggedInModel<FeedModel> FilteredFeed(User aUser);

        bool AddZipCodeFilter(User aUser, string aZipCode);
        bool AddCityStateFilter(User aUser, string aCity, string aState);
    }
}
