using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.Repositories.UserFeatures {
    public class EntityHAVProfileRepository : HAVBaseRepository, IHAVProfileRepository {



        public IEnumerable<IssueReply> IssuesUserRepliedTo(User aUser) {
            return (from ir in GetEntities().IssueReplys
                    where ir.User.Id == aUser.Id
                    select ir)
                    .OrderByDescending(ir => ir.DateTimeStamp)
                    .Take(5)
                    .ToList<IssueReply>();
        }


        public bool IsFan(int aUserId, User aFan) {
            return (from f in GetEntities().Fans
                    where f.User.Id == aFan.Id && f.SourceUser.Id == aUserId
                    select f)
                    .Count() > 0 ? true : false;
        }

        public IEnumerable<Fan> FindFansForUser(int aUserId) {
            return (from f in GetEntities().Fans
                    where f.SourceUser.Id == aUserId
                    select f).ToList<Fan>();
        }

        public IEnumerable<Fan> FindFansOfUser(int aUserId) {
            return (from f in GetEntities().Fans
                    where f.User.Id == aUserId
                    select f).ToList<Fan>();
        }
    }
}