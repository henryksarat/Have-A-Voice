using System;
using System.Linq;
using Social.Generic.Models;
using Social.User.Repositories;
using UniversityOfMe.Models;
using UniversityOfMe.Models.SocialModels;

namespace UniversityOfMe.Repositories.Password {
    public class EntityPasswordRepository : IPasswordRepository<User> {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void UpdateUserForgotPasswordHash(string anEmail, string aHashCode) {
            User myUser = FindUserByEmail(anEmail);
            myUser.ForgotPasswordHash = aHashCode;
            myUser.ForgotPasswordHashDateTimeStamp = DateTime.UtcNow;
            theEntities.ApplyCurrentValues(myUser.EntityKey.EntitySetName, myUser);
            theEntities.SaveChanges();
        }

        public AbstractUserModel<User> GetUserByEmailAndForgotPasswordHash(string anEmail, string aHashCode) {
            User myUser = (from c in theEntities.Users
                           where c.Email == anEmail
                           && c.ForgotPasswordHash == aHashCode
                           select c).FirstOrDefault<User>();
            return SocialUserModel.Create(myUser);
        }

        public void ChangePassword(int aUserId, string aPassword) {
            theEntities = new UniversityOfMeEntities();
            User myUser = FindUserByUserId(aUserId);
            myUser.Password = aPassword;
            myUser.ForgotPasswordHash = null;
            myUser.ForgotPasswordHashDateTimeStamp = null;
            theEntities.ApplyCurrentValues(myUser.EntityKey.EntitySetName, myUser);
            theEntities.SaveChanges();
        }

        private User FindUserByEmail(string anEmail) {
            return (from u in theEntities.Users
                    where u.Email == anEmail
                    select u).FirstOrDefault();
        }

        private User FindUserByUserId(int aUserId) {
            return (from u in theEntities.Users
                    where u.Id == aUserId
                    select u).FirstOrDefault();
        }
    }
}