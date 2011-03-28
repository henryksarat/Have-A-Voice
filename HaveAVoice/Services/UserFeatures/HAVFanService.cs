using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Repositories;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVFanService : IHAVFanService {
        private IHAVFanRepository theFanRepo;

        public HAVFanService()
            : this(new EntityHAVFanRepository()) { }

        public HAVFanService(IHAVFanRepository aFanRepo) {
                theFanRepo = aFanRepo;
        }

        public void Add(User aUser, int aSourceUserId) {
            theFanRepo.Add(aUser, aSourceUserId);
        }

        public void Remove(User aUser, int aSourceUserId) {
            theFanRepo.Remove(aUser, aSourceUserId);
        }

        public IEnumerable<Fan> GetAllFansForUser(User aUser) {
            return theFanRepo.GetAllFansForUser(aUser);
        }

        public IEnumerable<Fan> GetAllSourceUsersForFan(User aUser) {
            return theFanRepo.GetAllSourceUsersForFan(aUser);
        }

        public bool IsFan(User aFanUser, int aSourceUserId) {
            return theFanRepo.IsFan(aFanUser, aSourceUserId);
        }
    }
}