using System.Collections.Generic;
using System.Linq;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Notifications {
    public class EntityNotificationRepository : INotificationRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public IEnumerable<BoardViewedState> GetBoardViewedStates(User aUser) {
            return (from bvs in theEntities.BoardViewedStates
                    where bvs.UserId == aUser.Id
                    && !bvs.Viewed
                    select bvs);
        }

        public IEnumerable<ClassEnrollment> GetClassEnrollmentsNotViewedBoard(User aUser) {
            return (from ce in theEntities.ClassEnrollments
                    where ce.UserId == aUser.Id
                    && !ce.BoardViewed
                    select ce);
        }

        public IEnumerable<ClubMember> GetClubMembersNotViewedBoared(User aUser) {
            return (from cm in theEntities.ClubMembers
                    where cm.ClubMemberUserId == aUser.Id
                    && !cm.BoardViewed
                    select cm);
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
    }
}