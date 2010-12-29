using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Repositories;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVFanService : HAVBaseService, IHAVFanService {
        private IHAVFanRepository theFanRepo;

        public HAVFanService()
            : this(new HAVBaseRepository(), new EntityHAVFanRepository()) { }

        public HAVFanService(IHAVBaseRepository aBaseRepository, IHAVFanRepository aFanRepo)
            : base(aBaseRepository) {
                theFanRepo = aFanRepo;
        }

        public void AddFan(User aUser, int aSourceUserId) {
            theFanRepo.AddFan(aUser, aSourceUserId);
        }

        public void ApproveFan(int aFanId) {
            theFanRepo.ApproveFan(aFanId);
        }

        public void DeclineFan(int aFanId) {
            theFanRepo.DeleteFan(aFanId);
        }

        public IEnumerable<Fan> FindFansForUser(int aUserId) {
            return theFanRepo.FindFansForUser(aUserId);
        }

        public IEnumerable<Fan> FindPendingFansForUser(int aUserId) {
            return theFanRepo.FindPendingFansForUser(aUserId);
        }

        public bool IsFan(int aUserId, User aFan) {
            return theFanRepo.IsFan(aUserId, aFan);
        }

        public bool IsPending(int aUserId, User aFan) {
            return theFanRepo.IsPending(aUserId, aFan);
        }
    }
}