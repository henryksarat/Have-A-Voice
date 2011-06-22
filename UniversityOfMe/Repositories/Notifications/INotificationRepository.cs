using System.Collections;
using System.Collections.Generic;
using UniversityOfMe.Models;
namespace UniversityOfMe.Repositories.Notifications {
    public interface INotificationRepository {
        IEnumerable<BoardViewedState> GetBoardViewedStates(User aUser);
        IEnumerable<ClassEnrollment> GetClassEnrollmentsNotViewedBoard(User aUser);
        IEnumerable<ClubMember> GetClubMembersNotViewedBoared(User aUser);
        IEnumerable<SendItem> GetSendItemsForUser(User aUser);
        IEnumerable<ClubMember> GetPendingClubMembersOfAdminedClubs(User aUser);
    }
}
