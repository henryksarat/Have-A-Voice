using UniversityOfMe.Models;

namespace UniversityOfMe.Helpers {
    public class FeatureHelper {
        public static bool IsFeatureEnabled(User aUser, Features aFeature) {
            bool myIsEnabled = true;

            foreach (FeaturesEnabled myFeaturesEnabled in aUser.FeaturesEnableds) {
                if(myFeaturesEnabled.FeatureName.Equals(aFeature.ToString())) {
                    if(!myFeaturesEnabled.Enabled) {
                        myIsEnabled = false;
                        break;
                    }
                }
            }

            return myIsEnabled;
        }
    }
}