using Social.Generic.Helpers;
using UniversityOfMe.Models;

namespace UniversityOfMe.Helpers {
    public class PrivacyHelper {
        public static bool PrivacyAllows(User aUser, SocialPrivacySetting aPrivacySetting) {
            foreach (UserPrivacySetting myUserPrivacySetting in aUser.UserPrivacySettings) {
                if (myUserPrivacySetting.PrivacySettingName.Equals(aPrivacySetting.ToString())) {
                    return true;
                }
            }

            return false;
        }
    }
}