using Social.Authentication.Helpers;
using Social.Authentication.Services;
using UniversityOfMe.Controllers.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Services.Users;

namespace UniversityOfMe.Helpers {
    public class GetUserStrategy : IGetUserStrategy<User> {
        public User GetUser(int anId) {
            IUofMeUserRetrievalService myService = new UofMeUserRetrievalService();
            return myService.GetUser(anId);
        }

        public Social.Generic.Models.UserInformationModel<User> GetUserInformationModel(int anId) {
            IAuthenticationService<User, Role, Permission, UserRole, PrivacySetting, RolePermission> myAuthService = InstanceHelper.CreateAuthencationService();
            return myAuthService.AuthenticateUser(anId, new ProfilePictureStrategy());
        }
    }
}