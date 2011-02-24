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

        public IEnumerable<BoardViewedState> UnreadParticipatingBoardMessages(User aUser) {
            return (from v in theEntities.BoardViewedStates
                    join b in theEntities.Boards on v.BoardId equals b.Id
                    where v.Viewed == false
                    && v.UserId == aUser.Id
                    && b.OwnerUserId != aUser.Id
                    select v).ToList<BoardViewedState>();
        }

        public IEnumerable<IssueViewedState> UnreadIssues(User aUser) {
            return (from v in theEntities.IssueViewedStates
                    where v.UserId == aUser.Id
                    && v.Viewed == false
                    select v).ToList<IssueViewedState>();
        }
    }
}