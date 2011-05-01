using System.Collections.Generic;
using System.Linq;
using Social.Admin.Exceptions;
using Social.Admin.Repositories;
using Social.Generic.Models;
using Social.User.Exceptions;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Repositories.AdminRepos;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Repositories.UserRepos {
    public class EntityUserRepository : IUofMeUserRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

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
            theEntities = new UniversityOfMeEntities();
            User originalUser = GetUser(userToDelete.Id);
            theEntities.DeleteObject(originalUser);
            theEntities.SaveChanges();
        }

        public User DeleteUserFromRole(int userId, int roleId) {
            IRoleRepository<User, Role, RolePermission> roleRepository = new EntityRoleRepository();

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
                    select c).Count() != 0 ? true : false;
        }

        public IEnumerable<User> GetNewestUsersFromUniversity(User aRequestingUser, string aUniversity, int aLimit) {
            return (from u in theEntities.Users
                    join uu in theEntities.UserUniversities on u.Id equals uu.UserId
                    join ue in theEntities.UniversityEmails on uu.UniversityEmailId equals ue.Email
                    where ue.UniversityId == aUniversity
                    && u.Id != aRequestingUser.Id
                    select u).Take<User>(aLimit).ToList<User>();

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

        public void UpdateUserEmailAndUniversities(User aUser) {
            User originalUser = GetUser(aUser.Id);

            DeleteCurrentUserUniversities(aUser);
            string myEmail = EmailHelper.ExtractEmailExtension(aUser.Email);
            UserUniversity myUserUniversity = UserUniversity.CreateUserUniversity(0, aUser.Id, myEmail);

            theEntities.AddToUserUniversities(myUserUniversity);
            theEntities.ApplyCurrentValues(originalUser.EntityKey.EntitySetName, aUser);

            theEntities.SaveChanges();
        }

        private void DeleteCurrentUserUniversities(User aUser) {
            IEnumerable<UserUniversity> myUserUniversities = GetUserUniversities(aUser);
            foreach (UserUniversity myUserUniversity in myUserUniversities) {
                theEntities.DeleteObject(myUserUniversity);
            }
        }

        private IEnumerable<UserUniversity> GetUserUniversities(User aUser) {
            return (from uu in theEntities.UserUniversities
                    where uu.UserId == aUser.Id
                    select uu).ToList<UserUniversity>();
        }

        private UserRole GetUserRole(User user, Role role) {
            return (from c in theEntities.UserRoles
                    where c.User.Id == user.Id
                    && c.Role.Id == role.Id
                    select c).FirstOrDefault();
        }

        private UserRole AddUserToNotConfirmedUserRole(User aUser, string aNotConfirmedRoleName) {
            IRoleRepository<User, Role, RolePermission> roleRepository = new EntityRoleRepository();
            Role notConfirmedUserRole = roleRepository.GetRoleByName(aNotConfirmedRoleName);

            return AddUserToRole(aUser, notConfirmedUserRole);
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
    }
}