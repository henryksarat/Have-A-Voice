using System;
using System.Collections.Generic;
using System.Linq;
using UniversityOfMe.Models;
using UniversityOfMe.Helpers.Badges;

namespace UniversityOfMe.Repositories.Badges {
    public class BadgeRepository : IBadgeRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void AddForAddedFriend(User aUser, int aToUserId) {
            //First log the action to audit
            UserAction myUserAction = UserAction.CreateUserAction(0, aUser.Id, BadgeAction.ADD_FRIEND.ToString(), BadgeSection.FRIEND.ToString(), aToUserId, DateTime.UtcNow);
            theEntities.AddToUserActions(myUserAction);

            IEnumerable<Badge> myAllPossibleBadges = GetAllBadgesForSectionAndAction(BadgeAction.ADD_FRIEND, BadgeSection.FRIEND);
            IEnumerable<UserBadge> myCurrentBadges = GetUserBadgesForUserForActionAndSection(aUser, BadgeAction.ADD_FRIEND, BadgeSection.FRIEND);

            IEnumerable<UserBadge> myIncompleteBadges = myCurrentBadges.Where(ub => !ub.Received).OrderBy(ub => ub.Badge.Level);

            IEnumerable<Badge> myMissingBadgesFromSection = myAllPossibleBadges.Except(myCurrentBadges.Select(b => b.Badge)).OrderBy(b => b.Level);

            //First incomplete badges, then if there are none incomplete we start on a new badge
            if (myIncompleteBadges.Count<UserBadge>() > 0) {
                UserBadge myFirstIncompleteBadge = myIncompleteBadges.FirstOrDefault<UserBadge>();
                myFirstIncompleteBadge.CurrentPoints += myFirstIncompleteBadge.Badge.OneHitPoints;

                if (myFirstIncompleteBadge.CurrentPoints >= myFirstIncompleteBadge.Badge.TotalPoints) {
                    myFirstIncompleteBadge.Received = true;
                }

                theEntities.ApplyCurrentValues(myFirstIncompleteBadge.EntityKey.EntitySetName, myFirstIncompleteBadge);
            } else if(myMissingBadgesFromSection.Count<Badge>() > 0) {
                Badge myBadge = myMissingBadgesFromSection.FirstOrDefault<Badge>();
                UserBadge myUserBadge = UserBadge.CreateUserBadge(0, aUser.Id, myBadge.Name, DateTime.UtcNow, myBadge.OneHitPoints, false);
                if (myUserBadge.CurrentPoints >= myBadge.TotalPoints) {
                    myUserBadge.Received = true;
                }
                theEntities.AddToUserBadges(myUserBadge);
            }

            theEntities.SaveChanges();
        }

        private IEnumerable<UserBadge> GetUserBadgesForUserForActionAndSection(User aUser, BadgeAction aBadgeAction, BadgeSection aBadgeSection) {
            return (from ub in theEntities.UserBadges
                    join b in theEntities.Badges on ub.BadgeName equals b.Name
                    where ub.UserId == aUser.Id
                    && b.Section == aBadgeSection.ToString()
                    && b.Action == aBadgeAction.ToString()
                    select ub);
                    
        }

        private IEnumerable<Badge> GetAllBadgesForSectionAndAction(BadgeAction aBadgeAction, BadgeSection aBadgeSection) {
            return (from b in theEntities.Badges
                    where b.Action == aBadgeAction.ToString()
                    && b.Section == aBadgeSection.ToString()
                    select b);
        }

        private UserBadge GetUserBadge(int aUserBadgeId) {
            return (from ub in theEntities.UserBadges
                    where ub.Id == aUserBadgeId
                    select ub).FirstOrDefault<UserBadge>();
        }
    }
}