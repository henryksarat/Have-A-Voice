using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVFanRepository : HAVBaseRepository, IHAVFanRepository {
        public void AddFan(User aUser, int aSourceUserId) {
            Fan myFan = Fan.CreateFan(0, aUser.Id, aSourceUserId);
            GetEntities().Fans.AddObject(myFan);
            GetEntities().SaveChanges();
        }

        public IEnumerable<Fan> FindFansForUser(int aUserId) {
            return (from f in GetEntities().Fans
                    where f.SourceUser.Id == aUserId
                    select f).ToList<Fan>();
        }

        public IEnumerable<Fan> FindFansOfUser(int aUserId) {
            return (from f in GetEntities().Fans
                    where f.FanUserId == aUserId
                    select f).ToList<Fan>();
        }

        public bool IsFan(int aUserId, User aFan) {
            return (from f in GetEntities().Fans
                    where f.FanUserId == aFan.Id && f.SourceUser.Id == aUserId
                    select f)
                    .Count() > 0 ? true : false;
        }
    }
}