using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.UserFeatures {
    public interface IFeatureRepository {
        void DisableFeature(User aForUser, string aFeature);
    }
}