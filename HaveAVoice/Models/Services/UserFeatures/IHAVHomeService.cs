using System.Collections.Generic;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Models.Services.UserFeatures {
    public interface IHAVHomeService {
        NotLoggedInModel NotLoggedIn();
        LoggedInModel LoggedIn(User aUser);

        bool AddZipCodeFilter(User aUser, string aZipCode);
        bool AddCityStateFilter(User aUser, string aCity, string aState);
    }
}
