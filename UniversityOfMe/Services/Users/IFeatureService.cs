using UniversityOfMe.Helpers;
using UniversityOfMe.Models;

namespace UniversityOfMe.Services.UserFeatures {
    public interface IFeatureService {
        void DisableFeature(User aForUser, Features aFeature);
    }
}