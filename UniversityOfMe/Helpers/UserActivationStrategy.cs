using Social.Authentication.Helpers;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.User.Services;
using UniversityOfMe.Models;

namespace UniversityOfMe.Helpers {
    public class UserActivationStrategy : IUserActivationStrategy<User, PrivacySetting> {
        public void AddPrivacySettingsBasedOnRole(IUserPrivacySettingsService<User, PrivacySetting> aPrivacySettingService,
                                                  AbstractUserModel<User> aUser,
                                                  string aRoleToMoveToName) {
            aPrivacySettingService.AddPrivacySettingsForUser(aUser.Model, new SocialPrivacySetting[] {
                SocialPrivacySetting.Display_Profile_To_Friend,
                SocialPrivacySetting.Display_Class_Enrollment
            });
        }
    }
}