using HaveAVoice.Models;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication.Helpers;
using Social.Authentication.Services;

namespace HaveAVoice.Helpers.UserInformation {
    public class GetUserStrategy : IGetUserStrategy<User> {
        public Social.Generic.Models.UserInformationModel<User> GetUserInformationModel(int anId) {
            IAuthenticationService<User, Role, Permission, UserRole, PrivacySetting, RolePermission> myAuthService = new HAVAuthenticationService();
            return myAuthService.AuthenticateUser(anId, new ProfilePictureStrategy());
        }
    }
}