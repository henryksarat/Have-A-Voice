using System;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Repositories.AdminFeatures;
using HaveAVoice.Repositories.UserFeatures;
using Social.Authentication.Services;
using Social.User.Services;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVAuthenticationService : AuthenticationService<User, Role, Permission, UserRole, PrivacySetting, RolePermission>, IHAVAuthenticationService {
        public HAVAuthenticationService()
            : base(new UserRetrievalService<User>(new EntityHAVUserRetrievalRepository()), 
                   new UserPrivacySettingsService<User, PrivacySetting>(new EntityHAVUserPrivacySettingsRepository()), 
                   new EntityHAVAuthenticationRepository(), new EntityHAVUserRepository(), new EntityHAVRoleRepository()) { }


        public void ActivateAuthority(string anActivationCode, string anAuthorityType) {
            Role myAuthorityRole = GetRoleRepo().GetRoleByName(anAuthorityType);
            if (myAuthorityRole == null) {
                throw new NullReferenceException("There is no authority role.");
            }

            ActivateUser(anActivationCode, myAuthorityRole.Id, myAuthorityRole.Name, new UserActivationStrategy());
        }
    }
}