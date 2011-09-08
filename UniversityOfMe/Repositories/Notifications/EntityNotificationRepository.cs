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

        public IEnumerable<ClassBoardViewState> GetClassBoardsWithNewReplies(User aUser) {
            return (from v in theEntities.ClassBoardViewStates
                    where !v.Viewed
                    && v.UserId == aUser.Id
                    select v);
        }

        public IEnumerable<ClassEnrollment> GetClassEnrollmentsNotViewedBoard(User aUser) {
            return (from ce in theEntities.ClassEnrollments
                    where ce.UserId == aUser.Id
                    && !ce.BoardViewed
                    && ce.LastClassBoardId != null
                    select ce);
        }

        public IEnumerable<ClubMember> GetClubMembersNotViewedBoared(User aUser) {
            return (from cm in theEntities.ClubMembers
                    where cm.ClubMemberUserId == aUser.Id
                    && !cm.BoardViewed
                    select cm);
        }

        public IEnumerable<EventAttendence> GetEventsWithNewBoardPosts(User aUser) {
            return (from ea in theEntities.EventAttendences
                    where !ea.BoardViewed
                    && ea.UserId == aUser.Id
                    select ea);
        }

        public IEnumerable<GeneralPostingViewState> GetGeneralPostingsNotViewed(User aUser) {
            return (from v in theEntities.GeneralPostingViewStates
                    where v.UserId == aUser.Id
                    && !v.Viewed
                    && !v.Unsubscribe
                    select v);
        }

        public IEnumerable<SendItem> GetSendItemsForUser(User aUser) {
            return (from s in theEntities.SendItems
                    where s.ToUserId == aUser.Id
                    && !s.Seen
                    select s).OrderByDescending(s => s.DateTimeStamp).ToList<SendItem>();
        }

        public IEnumerable<ClubMember> GetPendingClubMembersOfAdminedClubs(User aUser) {
            IEnumerable<int> myAdminedClubs = (from cm in theEntities.ClubMembers
                                               where cm.Administrator
                                               && cm.ClubMemberUserId == aUser.Id
                                               select cm.ClubId).ToList<int>();

            return (from cm in theEntities.ClubMembers
                    where myAdminedClubs.Contains(cm.ClubId)
                    && cm.Approved == UOMConstants.PENDING
                    select cm).ToList<ClubMember>();
        }


        public UserBadge GetLatestBadgeEarnedAndNotSeen(User aUser) {
            return (from ub in theEntities.UserBadges
                    where ub.UserId == aUser.Id
                    && !ub.Seen
                    select ub).FirstOrDefault<UserBadge>();
        }
    }
}