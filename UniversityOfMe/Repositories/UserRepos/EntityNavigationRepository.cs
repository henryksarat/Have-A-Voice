using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.UserFeatures {
    public class EntityNavigationRepository : INavigationRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public int GetUnreadMessagesReceivedCount(User aRequestingUser) {
            return (from m in theEntities.Messages
                    where m.ToUserId == aRequestingUser.Id
                    && m.ToViewed == false
                    && !m.ToDeleted 
                    select m).Count<Message>();
        }

        public int GetUnreadMessagesSentCount(User aRequestingUser) {
            return (from m in theEntities.Messages
                    where m.FromUserId == aRequestingUser.Id
                    && m.FromViewed == false
                    && m.FromDeleted == false
                    && m.RepliedTo == true
                    select m).Count<Message>();
        }

        public int GetPendingFriendRequestCount(User aRequestingUser) {
            return (from f in theEntities.Friends
                    where f.SourceUserId == aRequestingUser.Id
                    && f.Approved == false
                    select f).Count<Friend>();
        }

        public int GetUnviewedBoardCount(User aRequestingUser) {
            return (from v in theEntities.BoardViewedStates
                    where v.UserId == aRequestingUser.Id
                    && v.Viewed == false
                    select v).Count<BoardViewedState>();
        }
    }
}