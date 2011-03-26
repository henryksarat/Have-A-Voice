using System.Linq;
using Social.Admin.Exceptions;
using Social.Admin.Repositories;
using Social.User;
using Social.User.Models;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;

namespace UniversityOfMe.Repositories {
    public class EntityUserRepository : IUserRepository<User> {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public AbstractUserModel<User> CreateUser(User userToCreate) {
            theEntities.AddToUsers(userToCreate);
            theEntities.SaveChanges();

            try {
                AddUserToNotConfirmedUserRole(userToCreate);
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

        public bool EmailRegistered(string email) {
            return (from u in theEntities.Users
                    where u.Email == email
                    select u).Count() != 0 ? true : false;
        }

        public bool ShortUrlTaken(string aShortUrl) {
            return (from u in theEntities.Users
                    where u.ShortUrl == aShortUrl
                    select u).Count() != 0 ? true : false;
        }

        private UserRole AddUserToNotConfirmedUserRole(User user) {
            IRoleRepository<User, Role> roleRepository = new EntityRoleRepository();
            Role notConfirmedUserRole = roleRepository.GetNotConfirmedUserRole();

            return AddUserToRole(user, notConfirmedUserRole);
        }


        public UserRole AddUserToRole(User user, Role role) {
            UserRole myRole = UserRole.CreateUserRole(0, user.Id, role.Id);

            theEntities.AddToUserRoles(myRole);
            theEntities.SaveChanges();

            return myRole;
        }

        //TODO: Hack here.. need to use Authentication class... using this because if we use it to DeleteUser after adding it because of an error it errors out.
        private User GetUser(int anId) {
            return (from c in theEntities.Users
                    where c.Id == anId
                    select c).FirstOrDefault();
        }
    }
}