using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVFanRepository : IHAVFanRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public void AddFan(User aUser, int aSourceUserId) {
            Fan myFan = Fan.CreateFan(0, aUser.Id, aSourceUserId, false);
            theEntities.Fans.AddObject(myFan);
            theEntities.SaveChanges();
        }

        public void ApproveFan(int aFanId) {
            Fan myFan = FindFan(aFanId);
            myFan.Approved = true;
            theEntities.ApplyCurrentValues(myFan.EntityKey.EntitySetName, myFan);
            theEntities.SaveChanges();
        }

        public void DeleteFan(int aFanId) {
            Fan myFan = FindFan(aFanId);
            theEntities.DeleteObject(myFan);
            theEntities.SaveChanges();
        }

        public IEnumerable<Fan> FindFansForUser(int aUserId) {
            return (from f in theEntities.Fans
                    where f.SourceUser.Id == aUserId
                    && f.Approved == true
                    select f).ToList<Fan>();
        }

        public IEnumerable<Fan> FindFansOfUser(int aUserId) {
            return (from f in theEntities.Fans
                    where f.FanUserId == aUserId
                    && f.Approved == true
                    select f).ToList<Fan>();
        }

        public IEnumerable<Fan> FindPendingFansForUser(int aUserId) {
            return (from f in theEntities.Fans
                    where f.SourceUserId == aUserId
                    && f.Approved == false
                    select f).ToList<Fan>();
        }

        public bool IsFan(int aUserId, User aFan) {
            return (from f in theEntities.Fans
                    where f.FanUserId == aFan.Id && f.SourceUser.Id == aUserId
                    && f.Approved == true
                    select f).Count() > 0 ? true : false;
        }

        public bool IsPending(int aUserId, User aFan) {
            return (from f in theEntities.Fans
                    where f.FanUserId == aFan.Id && f.SourceUser.Id == aUserId
                    && f.Approved == false
                    select f).Count() > 0 ? true : false;
        }

        private Fan FindFan(int aFanId) {
            return (from f in theEntities.Fans
                    where f.Id == aFanId
                    select f).FirstOrDefault<Fan>();
        }
    }
}