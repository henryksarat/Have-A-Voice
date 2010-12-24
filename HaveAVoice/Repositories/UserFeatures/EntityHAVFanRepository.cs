using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVFanRepository : HAVBaseRepository, IHAVFanRepository {
        public void AddFan(User aUser, int aSourceUserId) {
            Fan myFan = Fan.CreateFan(0, aUser.Id, aSourceUserId, false);
            GetEntities().Fans.AddObject(myFan);
            GetEntities().SaveChanges();
        }

        public void ApproveFan(int aFanId) {
            Fan myFan = FindFan(aFanId);
            myFan.Approved = true;
            GetEntities().ApplyCurrentValues(myFan.EntityKey.EntitySetName, myFan);
            GetEntities().SaveChanges();
        }

        public void DeleteFan(int aFanId) {
            Fan myFan = FindFan(aFanId);
            GetEntities().DeleteObject(myFan);
            GetEntities().SaveChanges();
        }

        public IEnumerable<Fan> FindFansForUser(int aUserId) {
            return (from f in GetEntities().Fans
                    where f.SourceUser.Id == aUserId
                    && f.Approved == true
                    select f).ToList<Fan>();
        }

        public IEnumerable<Fan> FindFansOfUser(int aUserId) {
            return (from f in GetEntities().Fans
                    where f.FanUserId == aUserId
                    && f.Approved == true
                    select f).ToList<Fan>();
        }

        public IEnumerable<Fan> FindPendingFansForUser(int aUserId) {
            return (from f in GetEntities().Fans
                    where f.SourceUserId == aUserId
                    && f.Approved == false
                    select f).ToList<Fan>();
        }

        public bool IsFan(int aUserId, User aFan) {
            return (from f in GetEntities().Fans
                    where f.FanUserId == aFan.Id && f.SourceUser.Id == aUserId
                    && f.Approved == true
                    select f).Count() > 0 ? true : false;
        }

        public bool IsPending(int aUserId, User aFan) {
            return (from f in GetEntities().Fans
                    where f.FanUserId == aFan.Id && f.SourceUser.Id == aUserId
                    && f.Approved == false
                    select f).Count() > 0 ? true : false;
        }

        private Fan FindFan(int aFanId) {
            return (from f in GetEntities().Fans
                    where f.Id == aFanId
                    select f).FirstOrDefault<Fan>();
        }
    }
}