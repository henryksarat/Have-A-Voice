using System;
using System.Linq;
using System.Collections.Generic;
using HaveAVoice.Exceptions;
using HaveAVoice.Models.View;
using HaveAVoice.Models.Services.UserFeatures;
using HaveAVoice.Models.Repositories.AdminFeatures;

namespace HaveAVoice.Models.Repositories.UserFeatures {
    public class EntityHAVUserRepository : HAVBaseRepository, IHAVUserRepository {

        public User CreateUser(User userToCreate) {
            GetEntities().AddToUsers(userToCreate);
            GetEntities().SaveChanges();

            try {
                AddUserToNotConfirmedUserRole(userToCreate);
            } catch (NullRoleException exception) {
                DeleteUser(userToCreate);
                throw new NullRoleException("Deleted userToListenTo because there was no \"Not confirmed\" restrictionModel assigned properly.", exception);
            }

            return userToCreate;
        }

        private UserRole AddUserToNotConfirmedUserRole(User user) {
            IHAVRoleRepository roleRepository = new EntityHAVRoleRepository();
            Role notConfirmedUserRole = roleRepository.GetNotConfirmedUserRole();

            return AddUserToRole(user, notConfirmedUserRole);
        }

        public UserRole AddUserToRole(User user, Role role) {
            UserRole userRole = new UserRole();
            userRole.Role = role;
            userRole.User = user;

            GetEntities().AddToUserRoles(userRole);
            GetEntities().SaveChanges();

            return userRole;
        }

        public User DeleteUserFromRole(int userId, int roleId) {
            IHAVRoleRepository roleRepository = new EntityHAVRoleRepository();

            User user = GetUser(userId);
            Role role = roleRepository.GetRole(roleId);

            if (user == null)
                throw new NullUserException("User is null.");

            if (role == null)
                throw new NullRoleException("Role is null.");

            UserRole userRole = GetUserRole(user, role);
            GetEntities().DeleteObject(userRole);
            GetEntities().SaveChanges();

            return user;
        }

        private UserRole GetUserRole(User user, Role role) {
            return (from c in GetEntities().UserRoles
                    where c.User.Id == user.Id
                    && c.Role.Id == role.Id
                    select c).FirstOrDefault();
        }


        public User GetUser(int id) {
            return (from c in GetEntities().Users
                    where c.Id == id
                    select c).FirstOrDefault();
        }


        public User GetUser(string email, string password) {
            return (from c in GetEntities().Users
                    where c.Email == email
                    && c.Password == password
                    select c).FirstOrDefault();
        }

        public User GetUser(string email) {
            return (from c in GetEntities().Users
                    where c.Email == email
                    select c).FirstOrDefault();
        }

        public User GetUserFromCookieHash(int userId, string cookieHash) {
            DateTime dateTimeCookieExpires = DateTime.Now.AddHours(HAVUserService.REMEMBER_ME_COOKIE_HOURS);
            return (from c in GetEntities().Users
                    where c.Id == userId
                    && c.CookieHash == cookieHash
                    && c.CookieCreationDate >= dateTimeCookieExpires
                    select c).FirstOrDefault();
        }

        public User UpdateCookieHashCreationDate(User user) {
            user.CookieCreationDate = DateTime.Now;
            UpdateUser(user);
            return user;
        }

        public void DeleteUser(User userToDelete) {
            User originalUser = GetUser(userToDelete.Id);
            GetEntities().DeleteObject(originalUser);
            GetEntities().SaveChanges();
        }


        public bool EmailRegistered(string email) {
            return (from c in GetEntities().Users
                    where c.Email == email
                    select c).Count() != 0 ? true : false;
        }

        public bool UsernameRegistered(string username) {
            return (from c in GetEntities().Users
                    where c.Username == username
                    select c).Count() != 0 ? true : false;
        }

        public void RemoveUserFromRole(User myUser, Role myRole) {
            UserRole userRole = (from ur in GetEntities().UserRoles
                            where ur.User.Id == myUser.Id
                            && ur.Role.Id == myRole.Id
                            select ur).FirstOrDefault();

            GetEntities().DeleteObject(userRole);
            GetEntities().SaveChanges();
        }

        public User FindUserByActivationCode(string activationCode) {
            return (from c in GetEntities().Users
                    where c.ActivationCode == activationCode
                    select c).FirstOrDefault();
        }

        public IEnumerable<UserDetailsModel> GetUserList(User user) {
            EntityHAVRoleRepository roleRepository = new EntityHAVRoleRepository();
            
            Role notConfirmedRole = roleRepository.GetNotConfirmedUserRole();
            System.Data.Objects.ObjectQuery<Fan> listeners = GetEntities().Fans;
            int userId = (user == null ? -1 : user.Id);

            List<UserDetailsModel> userInformations = (from usr in GetEntities().Users
                                              join ur in GetEntities().UserRoles on usr.Id equals ur.User.Id
                                              where ur.Role.Id != notConfirmedRole.Id
                                              && usr.Id != userId
                                              select new UserDetailsModel {
                                                  UserId = usr.Id,
                                                  Username = usr.Username,
                                                  Email = usr.Email,
                                                  CanListen = false,
                                                  CanMessage = false
                                              }).ToList<UserDetailsModel>();

            foreach (UserDetailsModel userInformation in userInformations) {
                if (CanFan(userInformation.UserId, userId)) {
                    userInformation.CanListen = true;
                }

                if (CanMessage(userInformation.UserId, userId)) {
                    userInformation.CanMessage = true;
                }
            }

            return userInformations;
        }

        private bool CanFan(int listenedToUserId, int aFanUserId) {
            return (ValidUserIdandNotIsNotMyself(listenedToUserId, aFanUserId)
                    && ((from listener in GetEntities().Fans
                         where listener.SourceUser.Id == listenedToUserId
                         && listener.Id == aFanUserId
                         select listener).Count() == 0 ? true : false));
        }

        private bool CanMessage(int listenedToUserId, int listeningUserId) {
            return (ValidUserIdandNotIsNotMyself(listenedToUserId, listeningUserId));
        }

        private static bool ValidUserIdandNotIsNotMyself(int listenedToUserId, int listeningUserId) {
            return (listeningUserId != -1)
                                && (listenedToUserId != listeningUserId);
        }

        public IEnumerable<Timezone> GetTimezones() {
            return GetEntities().Timezones.ToList<Timezone>();
        }

        public void AddProfilePicture(string anImageURL, User aUser) {
            UserPicture userPicture = UserPicture.CreateUserPicture(0, anImageURL, true, DateTime.UtcNow);
            userPicture.User = GetUser(aUser.Id);

            UnSetCurrentUserPicture(aUser);

            GetEntities().AddToUserPictures(userPicture);
            GetEntities().SaveChanges();
        }

        public void SetToProfilePicture(int aUserPictureId, User aUser) {
            UserPicture newProfilePicture = GetUserPicture(aUserPictureId);

            if(newProfilePicture == null) {
                return;
            }

            newProfilePicture.ProfilePicture = true;
            UnSetCurrentUserPicture(aUser);

            GetEntities().ApplyCurrentValues(newProfilePicture.EntityKey.EntitySetName, newProfilePicture);

            GetEntities().SaveChanges();
        }

        private void UnSetCurrentUserPicture(User aUser) {
            UserPicture currentProfilePicture = GetProfilePicture(aUser.Id);
            if(currentProfilePicture != null) {
                currentProfilePicture.ProfilePicture = false;
                GetEntities().ApplyCurrentValues(currentProfilePicture.EntityKey.EntitySetName, currentProfilePicture);
            }
        }

        public UserPicture GetProfilePicture(int aUserId) {
            return (from up in GetEntities().UserPictures
                    where up.User.Id == aUserId
                    && up.ProfilePicture == true
                    select up).FirstOrDefault();
        }

        private UserPicture GetUserPicture(int aUserPictureId) {
            return (from up in GetEntities().UserPictures
                    where up.Id == aUserPictureId
                    select up).FirstOrDefault();
        }

        public IEnumerable<UserPicture> GetUserPictures(int aUserId) {
            return (from up in GetEntities().UserPictures
                    where up.User.Id == aUserId
                    && up.ProfilePicture == false
                    orderby up.DateTimeStamp descending
                    select up).ToList<UserPicture>();
        }

        public void DeleteUserPicture(int aUserPictureId) {
            UserPicture userPicture = GetUserPicture(aUserPictureId);
            GetEntities().DeleteObject(userPicture);
            GetEntities().SaveChanges();
        }

        public IEnumerable<Permission> GetPermissionsForUser(User aUser) {
            return (from u in GetEntities().Users
                    join ur in GetEntities().UserRoles on u.Id equals ur.User.Id
                    join r in GetEntities().Roles on ur.Role.Id equals r.Id
                    join rp in GetEntities().RolePermissions on r.Id equals rp.Role.Id
                    join p in GetEntities().Permissions on rp.Permission.Id equals p.Id
                    where u.Id == aUser.Id
                    select p).ToList<Permission>();
        }

        public Restriction GetRestrictionsForUser(User aUser) {
            return (from u in GetEntities().Users
                    join ur in GetEntities().UserRoles on u.Id equals ur.User.Id
                    join r in GetEntities().Roles on ur.Role.Id equals r.Id
                    join res in GetEntities().Restrictions on r.Restriction.Id equals res.Id 
                    where u.Id == aUser.Id
                    select res).FirstOrDefault<Restriction>();
                    
        }

        public UserPrivacySetting GetUserPrivacySettingsForUser(User aUser) {
            return (from p in GetEntities().UserPrivacySettings
                    where p.User.Id == aUser.Id
                    select p).FirstOrDefault<UserPrivacySetting>();
        }

        UserPicture IHAVUserRepository.GetUserPicture(int userPictureId) {
            return (from up in GetEntities().UserPictures
                    where up.Id == userPictureId
                    select up).FirstOrDefault<UserPicture>();
        }

        #region "Who Is Online"

        public void AddToWhoIsOnline(User currentUser, string currentIpAddress) {
            IHAVUserRepository userRepository = new EntityHAVUserRepository();
            WhoIsOnline model = new WhoIsOnline();
            model.User = userRepository.GetUser(currentUser.Id);
            model.DateTimeStamp = DateTime.UtcNow;
            model.IpAddress = currentIpAddress;
            GetEntities().AddToWhoIsOnline(model);
            GetEntities().SaveChanges();
        }

        public void UpdateTimeOfWhoIsOnline(User currentUser, string currentIpAddress) {
            WhoIsOnline originalWhoIsOnline = GetWhoIsOnlineEntry(currentUser, currentIpAddress);
            originalWhoIsOnline.DateTimeStamp = DateTime.UtcNow;
            GetEntities().ApplyCurrentValues(originalWhoIsOnline.EntityKey.EntitySetName, originalWhoIsOnline);
            GetEntities().SaveChanges();
        }


        public WhoIsOnline GetWhoIsOnlineEntry(User currentUser, string currentIpAddress) {
            return (from w in GetEntities().WhoIsOnline
                    where w.User.Id == currentUser.Id
                    && w.IpAddress == currentIpAddress
                    select w).FirstOrDefault<WhoIsOnline>();
        }


        public void MarkForceLogOutOfOtherUsers(User currentUser, string currentIpAddress) {
            List<WhoIsOnline> otherUsers = (from w in GetEntities().WhoIsOnline
                                            where w.User.Id == currentUser.Id
                                            && w.IpAddress != currentIpAddress
                                            select w).ToList<WhoIsOnline>();

            foreach (WhoIsOnline onlineEntry in otherUsers) {
                onlineEntry.ForceLogOut = true;
                GetEntities().ApplyCurrentValues(onlineEntry.EntityKey.EntitySetName, onlineEntry);
            }
            GetEntities().SaveChanges();
        }


        public void RemoveFromWhoIsOnline(User currentUser, string currentIpAddress) {
            List<WhoIsOnline> onlineEntries = GetWhoIsOnlineEntries(currentUser, currentIpAddress);
            foreach (WhoIsOnline onlineEntry in onlineEntries) {
                GetEntities().DeleteObject(onlineEntry);
            }

            GetEntities().SaveChanges();
        }

        private List<WhoIsOnline> GetWhoIsOnlineEntries(User currentUser, string currentIpAddress) {
            return (from w in GetEntities().WhoIsOnline
                    where w.User.Id == currentUser.Id
                    && w.IpAddress == currentIpAddress
                    select w).ToList<WhoIsOnline>();
        }

        #endregion


        public void UpdatePrivacySettings(User aUser, UserPrivacySetting aUserPrivacySetting) {
            UserPrivacySetting mySettings = GetUserPrivacySettings(aUserPrivacySetting);
            GetEntities().ApplyCurrentValues(mySettings.EntityKey.EntitySetName, aUserPrivacySetting);
            GetEntities().SaveChanges();
        }

        private UserPrivacySetting GetUserPrivacySettings(UserPrivacySetting aUserPrivacySetting) {
            return (from p in GetEntities().UserPrivacySettings
                    where p.Id == aUserPrivacySetting.Id
                    select p).FirstOrDefault<UserPrivacySetting>();
        }

        public void UpdateUserForgotPasswordHash(string anEmail, string aHashCode) {
            User myUser = GetUser(anEmail);
            myUser.ForgotPasswordHash = aHashCode;
            myUser.ForgotPasswordHashDateTimeStamp = DateTime.UtcNow;
            GetEntities().ApplyCurrentValues(myUser.EntityKey.EntitySetName, myUser);
            GetEntities().SaveChanges();
        }

        public User GetUserByEmailAndForgotPasswordHash(string anEmail, string aHashCode) {
            return (from c in GetEntities().Users
                    where c.Email == anEmail
                    && c.ForgotPasswordHash == aHashCode
                    select c).FirstOrDefault();
        }

        public void ChangePassword(int aUserId, string aPassword) {
            User myUser = GetUser(aUserId);
            myUser.Password = aPassword;
            myUser.ForgotPasswordHash = null;
            myUser.ForgotPasswordHashDateTimeStamp = null;
            GetEntities().ApplyCurrentValues(myUser.EntityKey.EntitySetName, myUser);
            GetEntities().SaveChanges();
        }

        public void AddFan(User aUser, int aSourceUserId) {
            IHAVUserRepository myUserRepository = new EntityHAVUserRepository();
            Fan myFan = new Fan();
            myFan.User = myUserRepository.GetUser(aUser.Id);
            myFan.SourceUser = myUserRepository.GetUser(aSourceUserId);
            GetEntities().Fans.AddObject(myFan);
            GetEntities().SaveChanges();
        }


        public void AddDefaultUserPrivacySettings(User aUser) {
            UserPrivacySetting myPrivacySettings = UserPrivacySetting.CreateUserPrivacySetting(0, false, false, false);
            myPrivacySettings.User = GetUser(aUser.Id);
            GetEntities().AddToUserPrivacySettings(myPrivacySettings);
            GetEntities().SaveChanges();
        }


        public void UpdateUser(User userToEdit) {
            User originalUser = GetUser(userToEdit.Id);
            GetEntities().ApplyCurrentValues(originalUser.EntityKey.EntitySetName, userToEdit);
            GetEntities().SaveChanges();
        }
    }
}