﻿using System;
using System.Linq;
using System.Collections.Generic;
using HaveAVoice.Exceptions;
using HaveAVoice.Models.View;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Repositories.AdminFeatures;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVUserRepository : IHAVUserRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public User CreateUser(User userToCreate) {
            theEntities.AddToUsers(userToCreate);
            theEntities.SaveChanges();

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

            theEntities.AddToUserRoles(userRole);
            theEntities.SaveChanges();

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
            theEntities.DeleteObject(userRole);
            theEntities.SaveChanges();

            return user;
        }

        private UserRole GetUserRole(User user, Role role) {
            return (from c in theEntities.UserRoles
                    where c.User.Id == user.Id
                    && c.Role.Id == role.Id
                    select c).FirstOrDefault();
        }

        public void DeleteUser(User userToDelete) {
            User originalUser = GetUser(userToDelete.Id);
            theEntities.DeleteObject(originalUser);
            theEntities.SaveChanges();
        }


        public bool EmailRegistered(string email) {
            return (from c in theEntities.Users
                    where c.Email == email
                    select c).Count() != 0 ? true : false;
        }

        public bool UsernameRegistered(string username) {
            return (from c in theEntities.Users
                    where c.Username == username
                    select c).Count() != 0 ? true : false;
        }

        public void RemoveUserFromRole(User myUser, Role myRole) {
            UserRole userRole = (from ur in theEntities.UserRoles
                            where ur.User.Id == myUser.Id
                            && ur.Role.Id == myRole.Id
                            select ur).FirstOrDefault();

            theEntities.DeleteObject(userRole);
            theEntities.SaveChanges();
        }

        public IEnumerable<UserDetailsModel> GetUserList(User user) {
            EntityHAVRoleRepository roleRepository = new EntityHAVRoleRepository();
            
            Role notConfirmedRole = roleRepository.GetNotConfirmedUserRole();
            System.Data.Objects.ObjectQuery<Fan> listeners = theEntities.Fans;
            int userId = (user == null ? -1 : user.Id);

            List<UserDetailsModel> userInformations = (from usr in theEntities.Users
                                              join ur in theEntities.UserRoles on usr.Id equals ur.User.Id
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
            theEntities.ApplyCurrentValues(originalUser.EntityKey.EntitySetName, userToEdit);
            theEntities.SaveChanges();
        }

        private bool CanFan(int listenedToUserId, int aFanUserId) {
            return (ValidUserIdandNotIsNotMyself(listenedToUserId, aFanUserId)
                    && ((from listener in theEntities.Fans
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