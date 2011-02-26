using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Repositories;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVUserRetrievalService : HAVBaseService, IHAVUserRetrievalService {
        private IHAVUserRetrievalRepository theUserRetrievalRepo;

        public HAVUserRetrievalService()
            : this(new EntityHAVUserRetrievalRepository(), new HAVBaseRepository()) { }

        public HAVUserRetrievalService(IHAVUserRetrievalRepository aUserRetrievalRepo, IHAVBaseRepository myBaseRepo)
            : base(myBaseRepo) {
                theUserRetrievalRepo = aUserRetrievalRepo;
        }

        public User GetUser(int aUserId) {
            return theUserRetrievalRepo.GetUser(aUserId);
        }

        public User GetUserByShortUrl(string aShortUrl) {
            return theUserRetrievalRepo.GetUserByShortUrl(aShortUrl);
        }

        public User GetUser(string anEmail, string aPassword) {
            return theUserRetrievalRepo.GetUser(anEmail, aPassword);
        }

        public User GetUser(string anEmail) {
            return theUserRetrievalRepo.GetUser(anEmail);
        }

        public IEnumerable<User> GetUsersByNameSearch(string aNamePortion) {
            return theUserRetrievalRepo.GetUsersByNameContains(aNamePortion);
        }
    }
}