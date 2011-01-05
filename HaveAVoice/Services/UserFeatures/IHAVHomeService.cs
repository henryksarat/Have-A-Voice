using System.Collections.Generic;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVHomeService {
        NotLoggedInModel NotLoggedIn();

        LoggedInListModel<FeedModel> FanFeed(User aUser);
        LoggedInListModel<FeedModel> OfficialsFeed(User aUser);
        FilteredFeedModel FilteredFeed(User aUser);

        bool AddFilter(User aUser, string aCity, string aState, string aZipCode);
    }
}
