using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVNotificationRepository : IHAVNotificationRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public IEnumerable<Board> UnreadBoardMessages(User aUser) {
            return (from b in theEntities.Boards
                    join v in theEntities.BoardViewedStates on b.Id equals v.BoardId
                    where v.Viewed == false
                    && b.OwnerUserId == aUser.Id
                    select b).ToList<Board>();
        }
    }
}