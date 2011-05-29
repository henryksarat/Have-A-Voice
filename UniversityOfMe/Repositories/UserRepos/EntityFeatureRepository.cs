using System.Linq;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.UserFeatures {
    public class EntityFeatureRepository : IFeatureRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void DisableFeature(User aForUser, string aFeature) {
            FeaturesEnabled myFeatureEnabled = FeaturesEnabled.CreateFeaturesEnabled(0, aForUser.Id, aFeature, false);
            theEntities.AddToFeaturesEnableds(myFeatureEnabled);
            theEntities.SaveChanges();
        }
    }
}