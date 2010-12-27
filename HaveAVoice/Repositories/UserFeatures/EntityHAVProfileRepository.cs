using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVProfileRepository : IHAVProfileRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public IEnumerable<IssueReply> IssuesUserRepliedTo(User aUser) {
            return (from ir in theEntities.IssueReplys
                    where ir.User.Id == aUser.Id
                    select ir)
                    .OrderByDescending(ir => ir.DateTimeStamp)
                    .Take(5)
                    .ToList<IssueReply>();
        }
    }
}