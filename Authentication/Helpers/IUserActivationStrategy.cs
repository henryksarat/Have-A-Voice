using Social.User.Models;
using Social.Generic.Models;
using Social.User.Services;

namespace Social.Authentication.Helpers {
    public interface IUserActivationStrategy<T, U> {
        void AddPrivacySettingsBasedOnRole(IUserPrivacySettingsService<T, U> aPrivacySettingService, AbstractUserModel<T> aUser, string aRoleToMoveToName);
    }
}
