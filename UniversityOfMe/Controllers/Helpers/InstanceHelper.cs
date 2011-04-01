using Social.Authentication.Services;
using Social.User.Services;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.AdminRepos;
using UniversityOfMe.Repositories.AuthenticationRepos;
using UniversityOfMe.Repositories.UserRepos;

namespace UniversityOfMe.Controllers.Helpers {
    public static class InstanceHelper {
        public static IAuthenticationService<User, Role, Permission, UserRole, PrivacySetting, RolePermission> CreateAuthencationService() {
            return new AuthenticationService<User, Role, Permission, UserRole, PrivacySetting, RolePermission>(
                  new UserRetrievalService<User>(new EntityUserRetrievalRepository()), 
                  new UserPrivacySettingsService<User, PrivacySetting>(new EntityUserPrivacySettingsRepository()),
                  new EntityAuthenticationRepository(),
                  new EntityUserRepository(),
                  new EntityRoleRepository());
        }
    }
}