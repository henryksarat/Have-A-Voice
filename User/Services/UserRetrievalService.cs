using System.Collections.Generic;
using Social.Generic.Models;
using Social.User.Repositories;

namespace Social.User.Services {
    public class UserRetrievalService<T> : IUserRetrievalService<T> {
        private IUserRetrievalRepository<T> theUserRetrievalRepo;

        public UserRetrievalService(IUserRetrievalRepository<T> aUserRetrievalRepo) {
                theUserRetrievalRepo = aUserRetrievalRepo;
        }

        public T GetUser(int aUserId) {
            return theUserRetrievalRepo.GetUser(aUserId);
        }

        public T GetUserByShortUrl(string aShortUrl) {
            return theUserRetrievalRepo.GetUserByShortUrl(aShortUrl);
        }

        public T GetUser(string anEmail, string aPassword) {
            return theUserRetrievalRepo.GetUser(anEmail, aPassword);
        }

        public T GetUser(string anEmail) {
            return theUserRetrievalRepo.GetUser(anEmail);
        }

        public IEnumerable<T> GetUsersByNameSearch(string aNamePortion) {
            return theUserRetrievalRepo.GetUsersByNameContains(aNamePortion);
        }

        public AbstractUserModel<T> GetAbstractUser(string anEmail, string aPassword) {
            return theUserRetrievalRepo.GetAbstractUser(anEmail, aPassword);
        }

        public AbstractUserModel<T> GetAbstractUser(int aUserId) {
            return theUserRetrievalRepo.GetAbstractUser(aUserId);
        }
    }
}