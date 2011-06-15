using UniversityOfMe.Models;
using System.Collections;
using System.Collections.Generic;

namespace UniversityOfMe.Repositories.UserFeatures {
    public interface IFeatureRepository {
        void DisableFeature(User aForUser, string aFeature);
        IEnumerable<Feature> GetAllFeatures();
        IEnumerable<FeaturesEnabled> GetFeaturesEnabledForUser(User aUser);
        void UpdateFeatureSettings(User aUser, IEnumerable<Feature> anEnabledFeatures, IEnumerable<Feature> aDisabledFeatures);
    }
}