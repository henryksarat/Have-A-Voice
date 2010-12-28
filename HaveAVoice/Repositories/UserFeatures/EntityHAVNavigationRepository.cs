using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVNavigationRepository : IHAVNavigationRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public int TotalUnreadMessagesReceived(User aRequestingUser) {
            return (from m in theEntities.Messages
                    where m.ToUserId == aRequestingUser.Id
                    && m.ToViewed == false
                    select m).Count<Message>();
        }

        public int TotalUnreadMessagesSent(User aRequestingUser) {
            return (from m in theEntities.Messages
                    where m.FromUserId == aRequestingUser.Id
                    && m.FromViewed == false
                    && m.RepliedTo == true
                    select m).Count<Message>();
        }
    }
}