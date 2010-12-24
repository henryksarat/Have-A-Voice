using System;
using System.Linq;
using System.Collections.Generic;
using HaveAVoice.Exceptions;
using HaveAVoice.Models.View;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Repositories.AdminFeatures;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVUserRepository : HAVBaseRepository, IHAVUserRepository {

        public User CreateUser(User userToCreate) {
            GetEntities().AddToUsers(userToCreate);
            GetEntities().SaveChanges();

            try {
                AddUserToNotConfirmedUserRole(userToCreate);
            } catch (NullRoleException exception) {
                DeleteUser(userToCreate);
                throw new NullRoleException("Deleted user because there was no \"Not confirmed\" role assigned properly.", exception);
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
            Role role = roleRepository.FindRole(roleId);

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

        public void UpdateUser(User userToEdit) {
            User originalUser = GetUser(userToEdit.Id);
            GetEntities().ApplyCurrentValues(originalUser.EntityKey.EntitySetName, userToEdit);
            GetEntities().SaveChanges();
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

        private User GetUser(int anId) {
            IHAVUserRetrievalRepository myUserRetrieval = new EntityHAVUserRetrievalRepository();
            return myUserRetrieval.GetUser(anId);
        }
    }
}