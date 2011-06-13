using System.Collections;
using System.Collections.Generic;
using UniversityOfMe.Models;
namespace UniversityOfMe.Repositories.Notifications {
    public interface INotificationRepository {
        IEnumerable<SendItem> GetSendItemsForUser(User aUser);
        IEnumerable<ClubMember> GetPendingClubMembersOfAdminedClubs(User aUser);
    }
}
