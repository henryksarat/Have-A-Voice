using UniversityOfMe.Models;

namespace UniversityOfMe.Services.UserFeatures {
    public interface INavigationService {
        int NewMessageCount(User aRequestingUser);
        int PendingFriendCount(User aRequestingUser);
    }
}