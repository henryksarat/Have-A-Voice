using System.Collections.Generic;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVHomeService {
        NotLoggedInModel NotLoggedIn();

        IEnumerable<FeedModel> FanFeed(User aUser);
        IEnumerable<FeedModel> OfficialsFeed(User aUser);
        FilteredFeedModel FilteredFeed(User aUser);
        IEnumerable<FeedModel> UserFeedModel(User aViewingUser, int aTargetUser);

        bool AddFilter(User aUser, string aCity, string aState, string aZipCode);
    }
}
