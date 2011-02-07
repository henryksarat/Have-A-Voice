using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Repositories;
using HaveAVoice.Models.View;
using HaveAVoice.Models;
using HaveAVoice.Services.Helpers;
using HaveAVoice.Exceptions;
using HaveAVoice.Repositories.AdminFeatures;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVAuthenticationService : HAVBaseService, IHAVAuthenticationService {
        private IHAVUserPrivacySettingsService thePrivacySettingsService;
        private IHAVUserRetrievalService theUserRetrievalService;
        private IHAVAuthenticationRepository theAuthRepo;
        private IHAVUserRepository theUserRepo;
        private IHAVRoleRepository theRoleRepo;

        public HAVAuthenticationService()
            : this(new HAVUserRetrievalService(), new HAVBaseRepository(), new HAVUserPrivacySettingsService(), new EntityHAVAuthenticationRepository(), new EntityHAVUserRepository(), new EntityHAVRoleRepository()) { }

        public HAVAuthenticationService(IHAVUserRetrievalService aUserRetrievalService, IHAVBaseRepository baseRepository, 
                                                       IHAVUserPrivacySettingsService aPrivacyService, IHAVAuthenticationRepository anAuthRepo, IHAVUserRepository aUserRepo, IHAVRoleRepository aRoleRepo)
            : base(baseRepository) {
            theUserRetrievalService = aUserRetrievalService;
            thePrivacySettingsService = aPrivacyService;
            theAuthRepo = anAuthRepo;
            theUserRepo = aUserRepo;
            theRoleRepo = aRoleRepo;
        }

        public UserInformationModel RefreshUserInformationModel(UserInformationModel aUserInformationModel) {
            return AuthenticateUserWithHashedPassword(aUserInformationModel.Details.Email, aUserInformationModel.Details.Password);
        }

        public UserInformationModel AuthenticateUser(string anEmail, string aPassword) {
            aPassword = HashHelper.HashPassword(aPassword);
            return AuthenticateUserWithHashedPassword(anEmail, aPassword);
        }

        private UserInformationModel AuthenticateUserWithHashedPassword(string anEmail, string aHashedPassword) {
            User myUser = theUserRetrievalService.GetUser(anEmail, aHashedPassword);
            return CreateUserInformationModel(myUser);
        }

        public UserInformationModel CreateUserInformationModel(User aUser) {
            if (aUser == null) {
                return null;
            }

            IEnumerable<Permission> myPermissions = theAuthRepo.FindPermissionsForUser(aUser);
            bool myIsConfirmed = (from p in myPermissions
                                  where p.Name == HAVPermission.Confirmed_User.ToString()
                                  select p).Count<Permission>() > 0 ? true : false;

            if (!myIsConfirmed) {
                return null;
            }

            Restriction myRestriction = theAuthRepo.FindRestrictionsForUser(aUser);

            if (myRestriction == null) {
                throw new Exception("The user has no restriction.");
            }

            IEnumerable<PrivacySetting> myPrivacySettings = thePrivacySettingsService.FindPrivacySettingsForUser(aUser);

            if (myPrivacySettings == null) {
                throw new Exception("The user has no privacy settings.");
            }

            return new UserInformationModel(aUser, myPermissions, myRestriction, myPrivacySettings);
        }

        public void CreateRememberMeCredentials(User aUser) {
            string myCookieHash = CreateCookieHash(aUser);

            CookieHelper.CreateCookie(aUser.Id, myCookieHash);
        }

        public void ActivateNewUser(string activationCode) {
            Role myDefaultRole = theRoleRepo.GetDefaultRole();
            if (myDefaultRole == null) {
                throw new NullReferenceException("There is no default role.");
            }

            ActivateUser(activationCode, myDefaultRole);
        }

        public void ActivateAuthority(string anActivationCode, string anAuthorityType) {
            Role myAuthorityRole = theRoleRepo.GetRoleByName(anAuthorityType);
            if (myAuthorityRole == null) {
                throw new NullReferenceException("There is no authority role.");
            }

            ActivateUser(anActivationCode, myAuthorityRole);
        }

        public User ReadRememberMeCredentials() {

            int myUserId = CookieHelper.GetUserIdFromCookie();
            string myCookieHash = CookieHelper.GetCookieHashFromCookie();
            User myUser = theAuthRepo.FindUserByCookieHash(myUserId, myCookieHash);

            if (myUser != null) {
                theAuthRepo.UpdateCookieHashCreationDate(myUser);
                return myUser;
            }

            return null;
        }

        private User ActivateUser(string anActivationCode, Role aRoleToMoveTo) {
            User myUser = theAuthRepo.FindUserByActivationCode(anActivationCode);

            if (myUser == null) {
                throw new NullUserException("There is no user matching that activation code.");
            }

            Role myNotConfirmedRole = theRoleRepo.GetNotConfirmedUserRole();
            if (myNotConfirmedRole == null) {
                throw new NullRoleException("There is no \"Not confirmed\" role");
            }


            List<int> myUsers = new List<int>();
            myUsers.Add(myUser.Id);
            theRoleRepo.MoveUsersToRole(myUsers, myNotConfirmedRole.Id, aRoleToMoveTo.Id);

            if (aRoleToMoveTo.Name.Equals(Roles.POLITICIAN) || aRoleToMoveTo.Name.Equals(Roles.POLITICAL_CANDIDATE)) {
                thePrivacySettingsService.AddAuthorityPrivacySettingsForUser(myUser);
            } else {
                thePrivacySettingsService.AddDefaultPrivacySettingsForUser(myUser);
            }

            return myUser;
        }

        private string CreateCookieHash(User aUser) {
            string myTime = DateTime.Now.ToString();
            string myCookieHash =
                System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(aUser.Id + DateTime.Now.ToString(), "SHA1");
            aUser.CookieHash = myCookieHash;
            aUser.CookieCreationDate = DateTime.Now;
            theUserRepo.UpdateUser(aUser);
            return myCookieHash;
        }
    }
}