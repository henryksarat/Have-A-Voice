using System.Linq;
using HaveAVoice.Exceptions;
using HaveAVoice.Models;
using HaveAVoice.Models.SocialWrappers;
using HaveAVoice.Repositories.AdminFeatures;
using Social.Admin.Exceptions;
using Social.Admin.Repositories;
using Social.Generic.Models;
using Social.User.Repositories;
using System.Collections;
using System.Collections.Generic;
using System;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVUserRepository : IHAVUserRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public UserRole AddUserToRole(User aUser, Role aRole) {
            UserRole myRole = UserRole.CreateUserRole(0, aUser.Id, aRole.Id);

            theEntities.AddToUserRoles(myRole);
            theEntities.SaveChanges();

            return myRole;
        }

        public AbstractUserModel<User> CreateUser(User userToCreate, string aNotConfirmedRoleName) {
            theEntities.AddToUsers(userToCreate);
            theEntities.SaveChanges();

            try {
                AddUserToNotConfirmedUserRole(userToCreate, aNotConfirmedRoleName);
            } catch (NullRoleException exception) {
                DeleteUser(userToCreate);
                throw new NullRoleException("Deleted user because there was no \"Not confirmed\" role assigned properly.", exception);
            }

            return SocialUserModel.Create(userToCreate);
        }

        public void DeleteUser(User userToDelete) {
            theEntities = new HaveAVoiceEntities();
            User originalUser = GetUser(userToDelete.Id);
            theEntities.DeleteObject(originalUser);
            theEntities.SaveChanges();
        }

        public void DeleteUserWhoHasNotUsedActivationCode(string email) {
            User myUser = GetUserNotAuthenticated(email);
            theEntities.DeleteObject(myUser);
            theEntities.SaveChanges();
        }

        public User DeleteUserFromRole(int userId, int roleId) {
            IRoleRepository<User, Role, RolePermission> roleRepository = new EntityHAVRoleRepository();

            User user = GetUser(userId);
            Role role = roleRepository.FindRole(roleId);

            if (user == null)
                throw new NullUserException("User is null.");

            if (role == null)
                throw new NullRoleException("Role is null.");

            UserRole userRole = GetUserRole(user, role);
            theEntities.DeleteObject(userRole);
            theEntities.SaveChanges();

            return user;
        }

        public bool EmailRegistered(string email) {
            return (from c in theEntities.Users
                    where c.Email == email
                    && c.ActivationCodeUsed == true
                    select c).Count() != 0 ? true : false;
        }

        public bool EmailRegisteredButNotActiated(string email) {
            return (from c in theEntities.Users
                    where c.Email == email
                    && (c.ActivationCodeUsed == null || c.ActivationCodeUsed == false)
                    select c).Count() != 0 ? true : false;
        }

        public bool IsUserNameTaken(string aUsername) {
            return (from u in theEntities.Users
                    where u.Username == aUsername
                    select u).Count<User>() > 0;
        }

        public void RemoveUserFromRole(User myUser, Role myRole) {
            UserRole userRole = (from ur in theEntities.UserRoles
                            where ur.User.Id == myUser.Id
                            && ur.Role.Id == myRole.Id
                            select ur).FirstOrDefault();

            theEntities.DeleteObject(userRole);
            theEntities.SaveChanges();
        }

        public bool ShortUrlTaken(string aShortUrl) {
            return (from u in theEntities.Users
                    where u.ShortUrl == aShortUrl
                    select u).Count() != 0 ? true : false;
        }

        public void UpdateUser(User userToEdit) {
            User originalUser = GetUser(userToEdit.Id);
            theEntities.ApplyCurrentValues(originalUser.EntityKey.EntitySetName, userToEdit);
            theEntities.SaveChanges();
        }

        public void UpdateUserCookieHash(User userToEdit, string aCookieHash) {
            User originalUser = GetUser(userToEdit.Id);
            originalUser.CookieHash = aCookieHash;
            originalUser.CookieCreationDate = DateTime.UtcNow;
            theEntities.SaveChanges();
        }

        private UserRole GetUserRole(User user, Role role) {
            return (from c in theEntities.UserRoles
                    where c.User.Id == user.Id
                    && c.Role.Id == role.Id
                    select c).FirstOrDefault();
        }

        private UserRole AddUserToNotConfirmedUserRole(User user, string aNotConfirmedRoleName) {
            IRoleRepository<User, Role, RolePermission> roleRepository = new EntityHAVRoleRepository();
            Role notConfirmedUserRole = roleRepository.GetRoleByName(aNotConfirmedRoleName);

            return AddUserToRole(user, notConfirmedUserRole);
        }

        private bool CanFriend(int listenedToUserId, int aFriendUserId) {
            return (ValidUserIdandNotIsNotMyself(listenedToUserId, aFriendUserId)
                    && ((from listener in theEntities.Friends
                         where listener.SourceUserId == listenedToUserId
                         && listener.Id == aFriendUserId
                         select listener).Count() == 0 ? true : false));
        }

        private bool CanMessage(int listenedToUserId, int listeningUserId) {
            return (ValidUserIdandNotIsNotMyself(listenedToUserId, listeningUserId));
        }

        private static bool ValidUserIdandNotIsNotMyself(int listenedToUserId, int listeningUserId) {
            return (listeningUserId != -1)
                                && (listenedToUserId != listeningUserId);
        }

        //TODO: Hack here.. need to use Authentication class... using this because if we use it to DeleteUser after adding it because of an error it errors out.
        private User GetUser(int anId) {
            return (from c in theEntities.Users
                    where c.Id == anId
                    select c).FirstOrDefault();
        }

        private User GetUserNotAuthenticated(int anId) {
            return (from c in theEntities.Users
                    where c.Id == anId
                    && (c.ActivationCodeUsed == null || c.ActivationCodeUsed == false)
                    select c).FirstOrDefault();
        }

        private User GetUserNotAuthenticated(string anEmail) {
            return (from c in theEntities.Users
                    where c.Email == anEmail
                    && (c.ActivationCodeUsed == null || c.ActivationCodeUsed == false)
                    select c).FirstOrDefault();
        }
    }
}