using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.UserFeatures {
    public interface INavigationRepository {
        int GetUnreadMessagesReceivedCount(User aRequestingUser);
        int GetUnreadMessagesSentCount(User aRequestingUser);
        int GetPendingFriendRequestCount(User aRequestingUser);
        int GetUnviewedBoardCount(User aRequestingUser);
    }
}