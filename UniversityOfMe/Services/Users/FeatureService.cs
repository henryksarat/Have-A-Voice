using UniversityOfMe.Models;
using UniversityOfMe.Repositories.UserFeatures;

namespace UniversityOfMe.Services.UserFeatures {
    public class FeatureService : IFeatureService {
        private IFeatureRepository theFeatureRepo;

        public FeatureService()
            : this(new EntityFeatureRepository()) { }

        public FeatureService(IFeatureRepository aFeatureRepository) {
            theFeatureRepo = aFeatureRepository;
        }

        public void DisableFeature(User aForUser, Helpers.Features aFeature) {
            theFeatureRepo.DisableFeature(aForUser, aFeature.ToString());
        }
    }
}