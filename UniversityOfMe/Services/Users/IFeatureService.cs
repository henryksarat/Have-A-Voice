using Social.BaseWebsite.Models;
using Social.Generic.Models;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using Social.Generic;
using System.Collections.Generic;
using UniversityOfMe.Models.View;

namespace UniversityOfMe.Services.UserFeatures {
    public interface IFeatureService {
        void DisableFeature(User aForUser, Features aFeature);
        IEnumerable<Pair<Feature, bool>> GetFeatureSettingsForUser(UserInformationModel<User> aUser);
        void UpdateFeatureSettings(UserInformationModel<User> aUser, UpdateFeaturesModel anUpdateFeaturesModel);
    }
}