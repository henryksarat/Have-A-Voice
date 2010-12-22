using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVProfileRepository : HAVBaseRepository, IHAVProfileRepository {
        public IEnumerable<IssueReply> IssuesUserRepliedTo(User aUser) {
            return (from ir in GetEntities().IssueReplys
                    where ir.User.Id == aUser.Id
                    select ir)
                    .OrderByDescending(ir => ir.DateTimeStamp)
                    .Take(5)
                    .ToList<IssueReply>();
        }
    }
}