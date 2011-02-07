using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVUserRetrievalRepository : IHAVUserRetrievalRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

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
    }
}