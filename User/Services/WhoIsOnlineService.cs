using Social.Users.Services;
using Social.User.Repositories;
using System;
using Social.Generic.Constants;
using Social.User.Models;

namespace HaveAVoice.Services.UserFeatures {
    public class WhoIsOnlineService<T, U> : IWhoIsOnlineService<T, U> {
        private IWhoIsOnlineRepository<T, U> theRepository;

        public WhoIsOnlineService(IWhoIsOnlineRepository<T, U> aRepository) {
            theRepository = aRepository;
        }

        public void AddToWhoIsOnline(T aCurrentUser, string aCurrentIpAddress) {
            theRepository.RemoveFromWhoIsOnline(aCurrentUser, aCurrentIpAddress);
            theRepository.AddToWhoIsOnline(aCurrentUser, aCurrentIpAddress);
            theRepository.MarkForceLogOutOfOtherUsers(aCurrentUser, aCurrentIpAddress);
        }

        public bool IsOnline(T aCurrentUser, string aCurrentIpAddress) {
            AbstractWhoIsOnlineModel<U> myOnlineUser = theRepository.GetAbstractWhoIsOnlineEntry(aCurrentUser, aCurrentIpAddress);
            DateTime expiryTime = DateTime.UtcNow.AddSeconds(-1 * Constants.SECONDS_BEFORE_USER_TIMEOUT);

            bool isOnline = true;
            if (myOnlineUser.DateTimeStamp.CompareTo(expiryTime) < 0 || myOnlineUser.ForceLogOut) {
                theRepository.RemoveFromWhoIsOnline(aCurrentUser, aCurrentIpAddress);
                isOnline = false;
            } else {
                theRepository.UpdateTimeOfWhoIsOnline(aCurrentUser, aCurrentIpAddress);
            }

            return isOnline;
        }

        public void RemoveFromWhoIsOnline(T aCurrentUser, string aCurrentIpAddress) {
            theRepository.RemoveFromWhoIsOnline(aCurrentUser, aCurrentIpAddress);
        }
    }
}