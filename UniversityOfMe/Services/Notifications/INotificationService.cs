using System.Collections.Generic;
using UniversityOfMe.Models;
using Social.Generic.Models;

namespace UniversityOfMe.Services.Notifications {
    public interface INotificationService {
        IEnumerable<SendItem> GetSendItemsForUser(User aUser);
        IEnumerable<ClubMember> GetPendingClubMembersOfAdminedClubs(User aUser);
    }
}