using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Social.Admin.Exceptions;
using Social.Admin.Repositories;
using Social.Authentication.Helpers;
using Social.Authentication.Repositories;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.User.Exceptions;
using Social.User.Helpers;
using Social.User.Models;
using Social.User.Repositories;
using Social.User.Services;

namespace Social.Authentication.Services {
    public class AuthenticationService<T, U, V, W, X, Y> : IAuthenticationService<T, U, V, W, X, Y> {
        private IUserPrivacySettingsService<T, X> thePrivacySettingsService;
        private IUserRetrievalService<T> theUserRetrievalService;
        private IAuthenticationRepository<T, V> theAuthRepo;
        private IUserRepository<T, U, W> theUserRepo;
        private IRoleRepository<T, U, Y> theRoleRepo;

        public AuthenticationService(IUserRetrievalService<T> aUserRetrievalService, IUserPrivacySettingsService<T, X> aPrivacyService, 
                                     IAuthenticationRepository<T, V> anAuthRepo, IUserRepository<T, U, W> aUserRepo,
                                     IRoleRepository<T, U, Y> aRoleRepo) {
            theUserRetrievalService = aUserRetrievalService;
            thePrivacySettingsService = aPrivacyService;
            theAuthRepo = anAuthRepo;
            theUserRepo = aUserRepo;
            theRoleRepo = aRoleRepo;
        }

        public UserInformationModel<T> RefreshUserInformationModel(string anEmail, string aPassword, IProfilePictureStrategy<T> aProfilePictureStrategy) {
            return AuthenticateUserWithHashedPassword(anEmail, aPassword, aProfilePictureStrategy);
        }

        public UserInformationModel<T> AuthenticateUser(string anEmail, string aPassword, IProfilePictureStrategy<T> aProfilePictureStrategy) {
            aPassword = HashHelper.DoHash(aPassword);
            return AuthenticateUserWithHashedPassword(anEmail, aPassword, aProfilePictureStrategy);
        }

        private UserInformationModel<T> AuthenticateUserWithHashedPassword(string anEmail, string aHashedPassword, IProfilePictureStrategy<T> aProfilePictureStrategy) {
            AbstractUserModel<T> myUser = theUserRetrievalService.GetAbstractUser(anEmail, aHashedPassword);
            return CreateUserInformationModel(myUser, aProfilePictureStrategy);
        }

        public UserInformationModel<T>CreateUserInformationModel(AbstractUserModel<T> aUser, IProfilePictureStrategy<T> aProfilePictureStrategy) {
            if (aUser == null) {
                return null;
            }

            T myUser = aUser.Model;

            IEnumerable<AbstractPermissionModel<V>> myPermissions = theAuthRepo.FindPermissionsForUser(myUser);
            bool myIsConfirmed = (from p in myPermissions
                                  where p.Name == SocialPermission.Confirmed_User.ToString()
                                  select p).Count() > 0 ? true : false;

            if (!myIsConfirmed) {
                return null;
            }

            IEnumerable<AbstractPrivacySettingModel<X>> myPrivacySettings = thePrivacySettingsService.GetAbstractPrivacySettingsForUser(myUser);

            if (myPrivacySettings == null) {
                throw new Exception("The user has no privacy settings.");
            }

            return new UserInformationModel<T>(myUser, aUser.Id) {
                Permissions = ConvertPermissionToEnums(myPermissions),
                PrivacySettings = ConvertPrivacySettingsToEnums(myPrivacySettings),
                ProfilePictureUrl = aProfilePictureStrategy.GetProfilePicture(myUser),
                FullName = NameHelper<T>.FullName(aUser)
            };
        }

        private IEnumerable<SocialPermission> ConvertPermissionToEnums(IEnumerable<AbstractPermissionModel<V>> aPermissions) {
            IEnumerable<SocialPermission> myPermissions =
                (from p in aPermissions
                 select (SocialPermission)Enum.Parse(typeof(SocialPermission), p.Name))
                .ToList<SocialPermission>();
            return myPermissions;
        }

        private IEnumerable<SocialPrivacySetting> ConvertPrivacySettingsToEnums(IEnumerable<AbstractPrivacySettingModel<X>> aPrivacySettings) {
            IEnumerable<SocialPrivacySetting> myPrivacySettings =
                (from p in aPrivacySettings
                 select (SocialPrivacySetting)Enum.Parse(typeof(SocialPrivacySetting), p.Name))
                .ToList<SocialPrivacySetting>();
            return myPrivacySettings;
        }

        public void CreateRememberMeCredentials(AbstractUserModel<T> aUser) {
            string myCookieHash = CreateCookieHash(aUser);

            CookieHelper.CreateCookie(aUser.Id, myCookieHash);
        }

        public void ActivateNewUser(string activationCode) {
            AbstractRoleModel<U> myDefaultRole = theRoleRepo.GetAbstractDefaultRole();
            if (myDefaultRole == null) {
                throw new NullReferenceException("There is no default role.");
            }

            ActivateUser(activationCode, myDefaultRole.Id, myDefaultRole.Name, new DefaultUserActivationStrategy());
        }

        public T ReadRememberMeCredentials() {

            int myUserId = CookieHelper.GetUserIdFromCookie();
            string myCookieHash = CookieHelper.GetCookieHashFromCookie();
            T myUser = theAuthRepo.FindUserByCookieHash(myUserId, myCookieHash);

            if (myUser != null) {
                theAuthRepo.UpdateCookieHashCreationDate(myUser);
                return myUser;
            }

            return default(T);
        }

        protected T ActivateUser(string anActivationCode, int aRoleToMoveToId, string aRoleToMoveToName, IUserActivationStrategy<T, X> aUseActivationStrategy) {
            AbstractUserModel<T> myUser = theAuthRepo.GetAbstractUserByActivationCode(anActivationCode);

            if (myUser == null) {
                throw new NullUserException("There is no user matching that activation code.");
            }

            AbstractRoleModel<U> myNotConfirmedRole = theRoleRepo.GetAbstractRoleByName(Constants.NOT_CONFIRMED_USER_ROLE);
            if (myNotConfirmedRole == null) {
                throw new NullRoleException("There is no \"Not confirmed\" role");
            }


            List<int> myUsers = new List<int>();
            myUsers.Add(myUser.Id);
            theRoleRepo.MoveUsersToRole(myUsers, myNotConfirmedRole.Id, aRoleToMoveToId);

            aUseActivationStrategy.AddPrivacySettingsBasedOnRole(thePrivacySettingsService, myUser, aRoleToMoveToName);
            
            return myUser.CreateNewModel();
        }

        private string CreateCookieHash(AbstractUserModel<T> aUser) {
            string myTime = DateTime.Now.ToString();
            string myCookieHash =
                System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(aUser.Id + DateTime.Now.ToString(), "SHA1");
            aUser.CookieHash = myCookieHash;
            aUser.CookieHashCreationDate = DateTime.Now;
            theUserRepo.UpdateUser(aUser.CreateNewModel());
            return myCookieHash;
        }

        private class DefaultUserActivationStrategy : IUserActivationStrategy<T, X> {
            public void AddPrivacySettingsBasedOnRole(IUserPrivacySettingsService<T, X> aPrivacySettingService,
                                                      AbstractUserModel<T> aUser,
                                                      string aRoleToMoveToName) {
                //Do nothing
            }
        }

        protected IRoleRepository<T, U, Y> GetRoleRepo() {
            return theRoleRepo;
        }
    }
}