using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models;
using Social.Authentication.Helpers;
using Social.Generic.Models;
using Social.User.Services;

namespace HaveAVoice.Helpers {
    public class UserActivationStrategy : IUserActivationStrategy<User, PrivacySetting> {
        public void AddPrivacySettingsBasedOnRole(IUserPrivacySettingsService<User, PrivacySetting> aPrivacySettingService, 
                                                  AbstractUserModel<User> aUser, 
                                                  string aRoleToMoveToName) {
            if (aRoleToMoveToName.Equals(Roles.POLITICIAN) || aRoleToMoveToName.Equals(Roles.POLITICAL_CANDIDATE)) {
                aPrivacySettingService.AddPrivacySettingsForUser(aUser.Model, HAVPrivacyHelper.GetAuthorityPrivacySettings());
            } else {
                aPrivacySettingService.AddPrivacySettingsForUser(aUser.Model, HAVPrivacyHelper.GetDefaultPrivacySettings());
            }
        }
    }
}