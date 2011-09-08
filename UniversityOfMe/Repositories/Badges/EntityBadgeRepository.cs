using System;
using System.Linq;
using UniversityOfMe.Models;
using System.Collections.Generic;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Repositories.Badges {
    public class EntityBadgeRepository : IBadgeRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public IEnumerable<Badge> GetBadgesForUser(User aUser) {
            return (from ub in theEntities.UserBadges
                    where ub.UserId == aUser.Id
                    && ub.Received
                    select ub.Badge);
        }

        public UserBadge GetLatestUnseenBadgeForUser(User aUser) {
            return (from ub in theEntities.UserBadges
                    where ub.UserId == aUser.Id
                    && !ub.Seen
                    select ub)
                    .OrderByDescending(ub => ub.DateTimeStamp)
                    .FirstOrDefault<UserBadge>();
        }

        public void MarkUserBadgeAsSeen(User aUser, int aUserBadgeId) {
            UserBadge myUserBadge = (from ub in theEntities.UserBadges
                         where ub.Id == aUserBadgeId &&
                         ub.UserId == aUser.Id
                         select ub).FirstOrDefault<UserBadge>();

            if(myUserBadge != null) {
                myUserBadge.Seen = true;
                theEntities.ApplyCurrentValues(myUserBadge.EntityKey.EntitySetName, myUserBadge);
                theEntities.SaveChanges();
            }
        }
    }
}