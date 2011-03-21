using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HaveAVoice.Exceptions;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.AdminFeatures;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.Helpers;
using Social.Admin.Repositories;
using Social.Generic.Helpers;
using Social.Generic.Models;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVAuthenticationService : HAVBaseService, IHAVAuthenticationService {
        private IHAVUserPrivacySettingsService thePrivacySettingsService;
        private IHAVUserRetrievalService theUserRetrievalService;
        private IHAVAuthenticationRepository theAuthRepo;
        private IHAVUserRepository theUserRepo;
        private IRoleRepository<User, Role> theRoleRepo;

        public HAVAuthenticationService()
            : this(new HAVUserRetrievalService(), new HAVBaseRepository(), new HAVUserPrivacySettingsService(), 
            new EntityHAVAuthenticationRepository(), new EntityHAVUserRepository(), new EntityHAVRoleRepository()) { }

        public HAVAuthenticationService(IHAVUserRetrievalService aUserRetrievalService, IHAVBaseRepository baseRepository,
                                        IHAVUserPrivacySettingsService aPrivacyService, IHAVAuthenticationRepository anAuthRepo, 
                                        IHAVUserRepository aUserRepo, IRoleRepository<User, Role> aRoleRepo)
            : base(baseRepository) {
            theUserRetrievalService = aUserRetrievalService;
            thePrivacySettingsService = aPrivacyService;
            theAuthRepo = anAuthRepo;
            theUserRepo = aUserRepo;
            theRoleRepo = aRoleRepo;
        }

        public UserInformationModel<User> RefreshUserInformationModel(UserInformationModel<User> aUserInformationModel) {
            return AuthenticateUserWithHashedPassword(aUserInformationModel.Details.Email, aUserInformationModel.Details.Password);
        }

        public UserInformationModel<User> AuthenticateUser(string anEmail, string aPassword) {
            aPassword = HashHelper.HashPassword(aPassword);
            return AuthenticateUserWithHashedPassword(anEmail, aPassword);
        }

        private UserInformationModel<User>AuthenticateUserWithHashedPassword(string anEmail, string aHashedPassword) {
            User myUser = theUserRetrievalService.GetUser(anEmail, aHashedPassword);
            return CreateUserInformationModel(myUser);
        }

        public UserInformationModel<User>CreateUserInformationModel(User aUser) {
            if (aUser == null) {
                return null;
            }

            IEnumerable<Permission> myPermissions = theAuthRepo.FindPermissionsForUser(aUser);
            bool myIsConfirmed = (from p in myPermissions
                                  where p.Name == SocialPermission.Confirmed_User.ToString()
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

            return new UserInformationModel<User>(aUser) {
                Permissions = ConvertPermissionToEnums(myPermissions),
                PrivacySettings = ConvertPrivacySettingsToEnums(myPrivacySettings),
                PermissionToRestriction = ConvertRestrictionToHashTable(myRestriction),
                ProfilePictureUrl = PhotoHelper.ProfilePicture(aUser),
                FullName = NameHelper.FullName(aUser)
            };
        }

        private IEnumerable<SocialPermission> ConvertPermissionToEnums(IEnumerable<Permission> aPermissions) {
            IEnumerable<SocialPermission> myPermissions =
                (from p in aPermissions
                 select (SocialPermission)Enum.Parse(typeof(SocialPermission), p.Name))
                .ToList<SocialPermission>();
            return myPermissions;
        }

        private IEnumerable<SocialPrivacySetting> ConvertPrivacySettingsToEnums(IEnumerable<PrivacySetting> aPrivacySettings) {
            IEnumerable<SocialPrivacySetting> myPrivacySettings =
                (from p in aPrivacySettings
                 select (SocialPrivacySetting)Enum.Parse(typeof(SocialPrivacySetting), p.Name))
                .ToList<SocialPrivacySetting>();
            return myPrivacySettings;
        }

        private Hashtable ConvertRestrictionToHashTable(Restriction aRestriction) {
            Hashtable myPermissionToRestriction = new Hashtable();

            List<Pair<SocialRestriction, long>> myRestrictions = new List<Pair<SocialRestriction, long>>();
            myRestrictions.Add(CreateRestrictionList(SocialRestriction.SecondsToWait, aRestriction.IssuePostsWaitTimeBeforePostSeconds));
            myRestrictions.Add(CreateRestrictionList(SocialRestriction.TimeLimit, aRestriction.IssuePostsTimeLimit));
            myRestrictions.Add(CreateRestrictionList(SocialRestriction.OccurencesWithinTimeLimit, aRestriction.IssuePostsWithinTimeLimit));

            myPermissionToRestriction.Add(SocialPermission.Post_Issue, myRestrictions);

            myRestrictions = new List<Pair<SocialRestriction, long>>();
            myRestrictions.Add(CreateRestrictionList(SocialRestriction.SecondsToWait, aRestriction.IssueReplyPostsWaitTimeBeforePostSeconds));
            myRestrictions.Add(CreateRestrictionList(SocialRestriction.TimeLimit, aRestriction.IssueReplyPostsTimeLimit));
            myRestrictions.Add(CreateRestrictionList(SocialRestriction.OccurencesWithinTimeLimit, aRestriction.IssueReplyPostsWithinTimeLimit));

            myPermissionToRestriction.Add(SocialPermission.Post_Issue_Reply, myRestrictions);
            return myPermissionToRestriction;
        }

        private Pair<SocialRestriction, long> CreateRestrictionList(SocialRestriction anEnumeratedRestriction, long restrictionValue) {
            Pair<SocialRestriction, long> pair = new Pair<SocialRestriction, long>();
            pair.First = anEnumeratedRestriction;
            pair.Second = restrictionValue;
            return pair;
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