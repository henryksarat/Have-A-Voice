using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVPasswordRepository : IHAVPasswordRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public void UpdateUserForgotPasswordHash(string anEmail, string aHashCode) {
            User myUser = FindUserByEmail(anEmail);
            myUser.ForgotPasswordHash = aHashCode;
            myUser.ForgotPasswordHashDateTimeStamp = DateTime.UtcNow;
            theEntities.ApplyCurrentValues(myUser.EntityKey.EntitySetName, myUser);
            theEntities.SaveChanges();
        }

        public User GetUserByEmailAndForgotPasswordHash(string anEmail, string aHashCode) {
            return (from c in theEntities.Users
                    where c.Email == anEmail
                    && c.ForgotPasswordHash == aHashCode
                    select c).FirstOrDefault();
        }

        public void ChangePassword(int aUserId, string aPassword) {
            User myUser = FindUserByUserId(aUserId);
            myUser.Password = aPassword;
            myUser.ForgotPasswordHash = null;
            myUser.ForgotPasswordHashDateTimeStamp = null;
            theEntities.ApplyCurrentValues(myUser.EntityKey.EntitySetName, myUser);
            theEntities.SaveChanges();
        }

        private User FindUserByEmail(string anEmail) {
            IHAVUserRetrievalRepository myUserRepo = new EntityHAVUserRetrievalRepository();
            return myUserRepo.GetUser(anEmail);
        }

        private User FindUserByUserId(int aUserId) {
            IHAVUserRetrievalRepository myUserRepo = new EntityHAVUserRetrievalRepository();
            return myUserRepo.GetUser(aUserId);
        }
    }
}