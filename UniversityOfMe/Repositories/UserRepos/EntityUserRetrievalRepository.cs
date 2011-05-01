﻿using System.Collections.Generic;
using System.Linq;
using Social.Generic.Models;
using Social.User.Repositories;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;

namespace UniversityOfMe.Repositories.UserRepos {
    public class EntityUserRetrievalRepository : IUofMeUserRetrievalRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public AbstractUserModel<User> GetAbstractUser(string anEmail, string aPassword) {
            return SocialUserModel.Create(GetUser(anEmail, aPassword));
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
            return (from u in theEntities.Users
                    where u.Gender == aGender
                    select u).ToList<User>();
        }


    }
}