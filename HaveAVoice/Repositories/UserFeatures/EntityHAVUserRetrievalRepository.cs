using System.Collections.Generic;
using System.Linq;
using HaveAVoice.Models;
using HaveAVoice.Models.SocialWrappers;
using Social.Generic.Models;
using Social.User.Repositories;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVUserRetrievalRepository : IUserRetrievalRepository<User> {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

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

        public IEnumerable<User> GetUsersByNameContains(string aNamePortion) {
            return (from u in theEntities.Users
                    where u.FirstName + " " + u.LastName == aNamePortion
                    || u.FirstName.Contains(aNamePortion) 
                    || u.LastName.Contains(aNamePortion)
                    select u);
        }
    }
}