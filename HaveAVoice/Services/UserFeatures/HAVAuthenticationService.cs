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

namespace HaveAVoice.Services.UserFeatures {
    public class HAVAuthenticationService : HAVBaseService, IHAVAuthenticationService {
        public static string REMEMBER_ME_COOKIE = "HaveAVoiceRememberMeCookie";
        public static int REMEMBER_ME_COOKIE_HOURS = 4;

        private IHAVAuthenticationRepository theAuthRepo;
        private IHAVUserRepository theUserRepo;
        private IHAVRoleRepository theRoleRepo;

        public HAVAuthenticationService()
            : this(new HAVBaseRepository(), new EntityHAVAuthenticationRepository(), new EntityHAVUserRepository(), new EntityHAVRoleRepository()) { }
        
        public HAVAuthenticationService(IHAVBaseRepository baseRepository, IHAVAuthenticationRepository anAuthRepo, IHAVUserRepository aUserRepo, IHAVRoleRepository aRoleRepo) : base(baseRepository) {
            theAuthRepo = anAuthRepo;
            theUserRepo = aUserRepo;
            theRoleRepo = aRoleRepo;
        }

        public UserInformationModel AuthenticateUser(string anEmail, string aPassword) {
            aPassword = PasswordHelper.HashPassword(aPassword);
            User myUser = theUserRepo.GetUser(anEmail, aPassword);

            if (myUser == null) {
                return null;
            }

            IEnumerable<Permission> myPermissions = theAuthRepo.GetPermissionsForUser(myUser);
            Restriction myRestriction = theUserRepo.GetRestrictionsForUser(myUser);

            if (myRestriction == null) {
                throw new Exception("The user has no restriction.");
            }

            UserPrivacySetting myPrivacySettings = theUserRepo.GetUserPrivacySettingsForUser(myUser);

            if (myPrivacySettings == null) {
                throw new Exception("The user has no privacy settings.");
            }

            return new UserInformationModel(myUser, myPermissions, myRestriction, myPrivacySettings);
        }

        public void CreateRememberMeCredentials(User aUser) {
            string myCookieHash = CreateCookieHash(aUser);
            HttpCookie aCookie = new HttpCookie(REMEMBER_ME_COOKIE);
            aCookie["UserId"] = aUser.Id.ToString();
            aCookie["CookieHash"] = myCookieHash;
            aCookie.Expires = DateTime.Today.AddHours(REMEMBER_ME_COOKIE_HOURS);
            HttpContext.Current.Response.Cookies.Add(aCookie);
        }

        public UserInformationModel ActivateNewUser(string activationCode) {
            User myUser = ActivateUser(activationCode);
            IEnumerable<Permission> permissions = theAuthRepo.GetPermissionsForUser(myUser);
            Restriction myRestriction = theUserRepo.GetRestrictionsForUser(myUser);
            UserPrivacySetting myPrivacySettings = theUserRepo.GetUserPrivacySettingsForUser(myUser);
            return new UserInformationModel(myUser, permissions, myRestriction, myPrivacySettings);
        }

        public User ReadRememberMeCredentials() {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies.Get(REMEMBER_ME_COOKIE);
            if (myCookie != null) {
                int userId = Int32.Parse(myCookie["UserId"]);
                string cookieHash = myCookie["CookieHash"];
                User myUser = theUserRepo.GetUserFromCookieHash(userId, cookieHash);
                myCookie.Expires = DateTime.Now.AddDays(REMEMBER_ME_COOKIE_HOURS);
                theUserRepo.UpdateCookieHashCreationDate(myUser);
                return myUser;
            } else {
                return null;
            }
        }

        private User ActivateUser(string anActivationCode) {
            User myUser = theUserRepo.FindUserByActivationCode(anActivationCode);

            if (myUser == null) {
                throw new NullUserException("There is no user matching that activation code.");
            }

            Role myNotConfirmedRole = theRoleRepo.GetNotConfirmedUserRole();
            if (myNotConfirmedRole == null) {
                throw new NullRoleException("There is no \"Not confirmed\" role");
            }
            Role myDefaultRole = theRoleRepo.GetDefaultRole();
            if (myDefaultRole == null) {
                throw new NullReferenceException("There is no default role.");
            }

            List<int> myUsers = new List<int>();
            myUsers.Add(myUser.Id);
            theRoleRepo.MoveUsersToRole(myUsers, myNotConfirmedRole.Id, myDefaultRole.Id);
            theUserRepo.AddDefaultUserPrivacySettings(myUser);
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