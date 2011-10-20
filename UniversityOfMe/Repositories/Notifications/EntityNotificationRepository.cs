using System.Collections.Generic;
using System.Linq;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Notifications {
    public class EntityNotificationRepository : INotificationRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public IEnumerable<BoardViewedState> GetBoardViewedStates(User aUser) {
            return (from bvs in theEntities.BoardViewedStates
                    join b in theEntities.Boards on bvs.BoardId equals b.Id
                    where bvs.UserId == aUser.Id
                    && !bvs.Viewed
                    && !b.Deleted
                    select bvs);
        }

        public IEnumerable<SendItem> GetSendItemsForUser(User aUser) {
            return (from s in theEntities.SendItems
                    where s.ToUserId == aUser.Id
                    && !s.Seen
                    select s).OrderByDescending(s => s.DateTimeStamp).ToList<SendItem>();
        }

        public UserBadge GetLatestBadgeEarnedAndNotSeen(User aUser) {
            return (from ub in theEntities.UserBadges
                    where ub.UserId == aUser.Id
                    && !ub.Seen
                    select ub).FirstOrDefault<UserBadge>();
        }
    }
}