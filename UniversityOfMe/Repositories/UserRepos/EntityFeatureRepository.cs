using System.Linq;
using UniversityOfMe.Models;
using System.Collections.Generic;

namespace UniversityOfMe.Repositories.UserFeatures {
    public class EntityFeatureRepository : IFeatureRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void DisableFeature(User aForUser, string aFeature) {
            FeaturesEnabled myFeatureEnabled = GetFeatureEnabled(aForUser, aFeature);
            if (myFeatureEnabled == null) {
                myFeatureEnabled = FeaturesEnabled.CreateFeaturesEnabled(0, aForUser.Id, aFeature, false);
                theEntities.AddToFeaturesEnableds(myFeatureEnabled);
            }

            myFeatureEnabled.Enabled = false;

            theEntities.SaveChanges();
        }

        public IEnumerable<Feature> GetAllFeatures() {
            return (from f in theEntities.Features
                    select f);
        }

        public IEnumerable<FeaturesEnabled> GetFeaturesEnabledForUser(User aUser) {
            return (from fe in theEntities.FeaturesEnableds
                    where fe.UserId == aUser.Id
                    select fe);
        }

        public void UpdateFeatureSettings(User aUser, IEnumerable<Feature> anEnabledFeatures, IEnumerable<Feature> aDisabledFeatures) {
            EnableDisableFeaturesWithoutSave(aUser, anEnabledFeatures, true);
            EnableDisableFeaturesWithoutSave(aUser, aDisabledFeatures, false);

            theEntities.SaveChanges();
        }

        private void EnableDisableFeaturesWithoutSave(User aUser, IEnumerable<Feature> aFeatures, bool anAllEnabled) {
            foreach (Feature myFeature in aFeatures) {
                FeaturesEnabled myFeatureEnabled = GetFeatureEnabled(aUser, myFeature.Name);

                if (myFeatureEnabled != null) {
                    myFeatureEnabled.Enabled = anAllEnabled;
                } else {
                    myFeatureEnabled = FeaturesEnabled.CreateFeaturesEnabled(0, aUser.Id, myFeature.Name, anAllEnabled);
                    theEntities.AddToFeaturesEnableds(myFeatureEnabled);
                }
            }
        }

        private FeaturesEnabled GetFeatureEnabled(User aUser, string aFeatureName) {
            return (from fe in theEntities.FeaturesEnableds
                    where fe.UserId == aUser.Id
                    && fe.FeatureName == aFeatureName
                    select fe).FirstOrDefault<FeaturesEnabled>();
        }
    }
}