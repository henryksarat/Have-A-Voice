using UniversityOfMe.Models;
using UniversityOfMe.Repositories.UserFeatures;

namespace UniversityOfMe.Services.UserFeatures {
    public class NavigationService : INavigationService {
        private INavigationRepository theNavRepo;

        public NavigationService()
            : this(new EntityNavigationRepository()) { }

        public NavigationService(INavigationRepository aNavigationRepository) {
            theNavRepo = aNavigationRepository;
        }

        public int NewMessageCount(User aRequestingUser) {
            int myTotalUnreadReceived = theNavRepo.GetUnreadMessagesReceivedCount(aRequestingUser);
            int myTotalUnreadSent = theNavRepo.GetUnreadMessagesSentCount(aRequestingUser);

            return myTotalUnreadReceived + myTotalUnreadSent;
        }

        public int PendingFriendCount(User aRequestingUser) {
            return theNavRepo.GetPendingFriendRequestCount(aRequestingUser);
        }
    }
}