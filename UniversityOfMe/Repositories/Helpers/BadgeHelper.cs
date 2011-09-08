using System;
using System.Collections.Generic;
using System.Linq;
using UniversityOfMe.Models;
using UniversityOfMe.Helpers.Badges;

namespace UniversityOfMe.Repositories.Helpers {
    public static class BadgeHelper {
        public static void AddNecessaryBadgesAndPoints(UniversityOfMeEntities anEntities, int aSourceUserId, BadgeAction aBadgeAction, BadgeSection aBadgeSection, int aBadgeSectionId) {
            //First log the action to audit
            UserAction myUserAction = UserAction.CreateUserAction(0, aSourceUserId, aBadgeAction.ToString(), aBadgeSection.ToString(), aBadgeSectionId, DateTime.UtcNow);
            anEntities.AddToUserActions(myUserAction);

            IEnumerable<Badge> myAllPossibleBadges = GetAllBadgesForSectionAndAction(anEntities, aBadgeAction.ToString(), aBadgeSection.ToString());
            IEnumerable<UserBadge> myCurrentBadges = GetUserBadgesForUserForActionAndSection(anEntities, aSourceUserId, aBadgeAction.ToString(), aBadgeSection.ToString());

            IEnumerable<UserBadge> myIncompleteBadges = myCurrentBadges.Where(ub => !ub.Received).OrderBy(ub => ub.Badge.Level);

            IEnumerable<Badge> myMissingBadgesFromSection = myAllPossibleBadges.Except(myCurrentBadges.Select(b => b.Badge)).OrderBy(b => b.Level);

            //First incomplete badges, then if there are none incomplete we start on a new badge
            if (myIncompleteBadges.Count<UserBadge>() > 0) {
                UserBadge myFirstIncompleteBadge = myIncompleteBadges.FirstOrDefault<UserBadge>();
                myFirstIncompleteBadge.CurrentPoints += myFirstIncompleteBadge.Badge.OneHitPoints;

                if (myFirstIncompleteBadge.CurrentPoints >= myFirstIncompleteBadge.Badge.TotalPoints) {
                    myFirstIncompleteBadge.Received = true;
                }

                anEntities.ApplyCurrentValues(myFirstIncompleteBadge.EntityKey.EntitySetName, myFirstIncompleteBadge);
            } else if (myMissingBadgesFromSection.Count<Badge>() > 0) {
                Badge myBadge = myMissingBadgesFromSection.FirstOrDefault<Badge>();
                UserBadge myUserBadge = UserBadge.CreateUserBadge(0, aSourceUserId, myBadge.Name, DateTime.UtcNow, myBadge.OneHitPoints, false, false);
                if (myUserBadge.CurrentPoints >= myBadge.TotalPoints) {
                    myUserBadge.Received = true;
                }
                anEntities.AddToUserBadges(myUserBadge);
            }
        }

        private static IEnumerable<UserBadge> GetUserBadgesForUserForActionAndSection(
            UniversityOfMeEntities anEntities, int aUserId, string aBadgeAction, string aBadgeSection) {
            return (from ub in anEntities.UserBadges
                    join b in anEntities.Badges on ub.BadgeName equals b.Name
                    where ub.UserId == aUserId
                    && b.Section == aBadgeSection
                    && b.Action == aBadgeAction
                    select ub);
                    
        }

        private static IEnumerable<Badge> GetAllBadgesForSectionAndAction(
            UniversityOfMeEntities anEntities, string aBadgeAction, string aBadgeSection) {
            return (from b in anEntities.Badges
                    where b.Action == aBadgeAction
                    && b.Section == aBadgeSection
                    select b);
        }

        private static UserBadge GetUserBadge(UniversityOfMeEntities anEntities, int aUserBadgeId) {
            return (from ub in anEntities.UserBadges
                    where ub.Id == aUserBadgeId
                    select ub).FirstOrDefault<UserBadge>();
        }
    }
}