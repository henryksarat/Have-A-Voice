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