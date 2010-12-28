using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVNavigationRepository : IHAVNavigationRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public int GetUnreadMessagesReceivedCount(User aRequestingUser) {
            return (from m in theEntities.Messages
                    where m.ToUserId == aRequestingUser.Id
                    && m.ToViewed == false
                    select m).Count<Message>();
        }

        public int GetUnreadMessagesSentCount(User aRequestingUser) {
            return (from m in theEntities.Messages
                    where m.FromUserId == aRequestingUser.Id
                    && m.FromViewed == false
                    && m.RepliedTo == true
                    select m).Count<Message>();
        }

        public int GetPendingFriendRequestCount(User aRequestingUser) {
            return (from f in theEntities.Fans
                    where f.SourceUserId == aRequestingUser.Id
                    && f.Approved == false
                    select f).Count<Fan>();
        }
    }
}