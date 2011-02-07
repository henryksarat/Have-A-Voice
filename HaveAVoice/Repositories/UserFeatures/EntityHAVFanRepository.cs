using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVFanRepository : IHAVFanRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public void Add(User aUser, int aSourceUserId) {
            Fan myFan = Fan.CreateFan(0, aSourceUserId, aUser.Id);

            theEntities.AddToFans(myFan);
            theEntities.SaveChanges();
        }

        public void Remove(User aUser, int aSourceUserId) {
            Fan myFan = GetFan(aUser, aSourceUserId);

            theEntities.DeleteObject(myFan);

            theEntities.SaveChanges();
        }

        public IEnumerable<Fan> GetAllFansForUser(User aUser) {
            return (from f in theEntities.Fans
                    where f.SourceUserId == aUser.Id
                    select f).ToList<Fan>();
        }

        public IEnumerable<Fan> GetAllSourceUsersForFan(User aUser) {
            return (from f in theEntities.Fans
                    where f.FanUserId == aUser.Id
                    select f).ToList<Fan>();
        }


        public bool IsFan(User aFanUser, int aSourceUserId) {
            return (from f in theEntities.Fans
                    where f.FanUserId == aFanUser.Id
                    && f.SourceUserId == aSourceUserId
                    select f).Count<Fan>() > 0 ? true : false;
        }

        private Fan GetFan(User aUser, int aSourceUserId) {
            return (from f in theEntities.Fans
                    where f.FanUserId == aUser.Id
                    && f.SourceUserId == aSourceUserId
                    select f).FirstOrDefault<Fan>();
        }
    }
}