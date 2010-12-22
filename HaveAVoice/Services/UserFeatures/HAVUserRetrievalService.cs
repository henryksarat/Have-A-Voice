using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Repositories;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVUserRetrievalService : HAVBaseService, IHAVUserRetrievalService {
        private IHAVUserRetrievalRepository theUserRetrievalRepo;

        public HAVUserRetrievalService()
            : this(new EntityHAVUserRetrievalRepository(), new HAVBaseRepository()) { }

        public HAVUserRetrievalService(IHAVUserRetrievalRepository aUserRetrievalRepo, IHAVBaseRepository myBaseRepo)
            : base(myBaseRepo) {
                theUserRetrievalRepo = aUserRetrievalRepo;
        }

        public Models.User GetUser(int aUserId) {
            return theUserRetrievalRepo.GetUser(aUserId);
        }

        public Models.User GetUser(string anEmail, string aPassword) {
            return theUserRetrievalRepo.GetUser(anEmail, aPassword);
        }

        public Models.User GetUser(string anEmail) {
            return theUserRetrievalRepo.GetUser(anEmail);
        }
    }
}