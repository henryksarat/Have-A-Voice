using System.Collections.Generic;
using System.Linq;
using Social.Generic.Models;
using Social.User.Repositories;
using UniversityOfMe.Models;
using UniversityOfMe.Models.SocialModels;

namespace UniversityOfMe.Repositories.UserRepos {
    public class EntityUserRetrievalRepository : IUofMeUserRetrievalRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public AbstractUserModel<User> GetAbstractUser(string anEmail, string aPassword) {
            return SocialUserModel.Create(GetUser(anEmail, aPassword));
        }

        public AbstractUserModel<User> GetAbstractUser(int aUserId) {
            return SocialUserModel.Create(GetUser(aUserId));
        }

        public User GetUser(int id) {
            return (from c in theEntities.Users
                    where c.Id == id
                    select c).FirstOrDefault();
        }

        public User GetUserByShortUrl(string aShortUrl) {
            return (from u in theEntities.Users
                    where u.ShortUrl == aShortUrl
                    select u).FirstOrDefault<User>();
        }

        public User GetUser(string email, string password) {
            return (from c in theEntities.Users
                    where c.Email == email
                    && c.Password == password
                    select c).FirstOrDefault();
        }

        public User GetUser(string email) {
            return (from c in theEntities.Users
                    where c.Email == email
                    select c).FirstOrDefault();
        }

        public User GetUserByChangeEmailInformation(string anOldEmail, string aNewEmailHash) {
            return (from u in theEntities.Users
                    where u.Email == anOldEmail
                    && u.NewEmailHash == aNewEmailHash
                    select u).FirstOrDefault<User>();
        }

        public IEnumerable<User> GetUsersByNameContains(string aNamePortion) {
            return (from u in theEntities.Users
                    where u.FirstName + " " + u.LastName == aNamePortion
                    || u.FirstName.Contains(aNamePortion)
                    || u.LastName.Contains(aNamePortion)
                    select u);
        }


        public IEnumerable<User> GetUsersWithGender(string aGender) {
            Role myRole = GetDefaultRole();

            return (from u in theEntities.Users
                    join ur in theEntities.UserRoles on u.Id equals ur.UserId
                    where u.Gender == aGender
                    && ur.RoleId == myRole.Id
                    select u).ToList<User>();
        }

        private Role GetDefaultRole() {
            return (from c in theEntities.Roles
                    where c.DefaultRole == true
                    select c).FirstOrDefault();
        }
    }
}