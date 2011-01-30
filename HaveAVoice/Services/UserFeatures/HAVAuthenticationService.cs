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

        public UserInformationModel AuthenticateUser(string anEmail, string aPassword) {
            aPassword = HashHelper.HashPassword(aPassword);
            User myUser = theUserRetrievalService.GetUser(anEmail, aPassword);

            if (myUser == null) {
                return null;
            }

            IEnumerable<Permission> myPermissions = theAuthRepo.FindPermissionsForUser(myUser);
            bool myIsConfirmed = (from p in myPermissions
                                  where p.Name == HAVPermission.Confirmed_User.ToString()
                                  select p).Count<Permission>() > 0 ? true : false;

            if (!myIsConfirmed) {
                return null;
            }

            Restriction myRestriction = theAuthRepo.FindRestrictionsForUser(myUser);

            if (myRestriction == null) {
                throw new Exception("The user has no restriction.");
            }

            IEnumerable<PrivacySetting> myPrivacySettings = thePrivacySettingsService.FindPrivacySettingsForUser(myUser);

            if (myPrivacySettings == null) {
                throw new Exception("The user has no privacy settings.");
            }

            return new UserInformationModel(myUser, myPermissions, myRestriction, myPrivacySettings);
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

        public void ActivateAuthority(string activationCode) {
            Role myAuthorityRole = theRoleRepo.GetAuthorityRole();
            if (myAuthorityRole == null) {
                throw new NullReferenceException("There is no authority role.");
            }

            ActivateUser(activationCode, myAuthorityRole);
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
            thePrivacySettingsService.AddDefaultPrivacySettingsForUser(myUser);
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