using UniversityOfMe.Models;
using UniversityOfMe.Repositories.UserFeatures;
using Social.BaseWebsite.Models;
using System.Collections.Generic;
using Social.Generic;
using Social.Generic.Models;
using System.Linq;
using UniversityOfMe.Models.View;

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

        public IEnumerable<Pair<Feature, bool>> GetFeatureSettingsForUser(UserInformationModel<User> aUser) {
            IEnumerable<Feature> myAllFeatures = theFeatureRepo.GetAllFeatures().OrderBy(f => f.ListOrder);
            IEnumerable<FeaturesEnabled> myAllFeaturesEnabled = theFeatureRepo.GetFeaturesEnabledForUser(aUser.Details);
            List<Pair<Feature, bool>> myFeatureAndSelection = new List<Pair<Feature, bool>>();

            foreach (Feature myFeature in myAllFeatures) {
                bool myEnabled = (from fe in myAllFeaturesEnabled
                                  where fe.FeatureName.Equals(myFeature.Name)
                                  select fe.Enabled)
                                  .DefaultIfEmpty(true)
                                  .FirstOrDefault();

                myFeatureAndSelection.Add(new Pair<Feature, bool>() {
                    First = myFeature,
                    Second = myEnabled
                });
            }

            return myFeatureAndSelection;
        }

        public void UpdateFeatureSettings(UserInformationModel<User> aUser, UpdateFeaturesModel anUpdateFeaturesModel) {
            IEnumerable<Feature> myEnabledFeatures = (from f in anUpdateFeaturesModel.Features
                                                      where f.Second
                                                      select f.First);
            IEnumerable<Feature> myDisabledFeatures = (from f in anUpdateFeaturesModel.Features
                                                       where !f.Second
                                                       select f.First);
            theFeatureRepo.UpdateFeatureSettings(aUser.Details, myEnabledFeatures, myDisabledFeatures);
        }
    }
}