using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Validation;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Repositories;
using HaveAVoice.Helpers;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVWhoIsOnlineService : HAVBaseService, IHAVWhoIsOnlineService {
        private IHAVWhoIsOnlineRepository theRepository;

        public HAVWhoIsOnlineService()
            : this(new EntityHAVWhoIsOnlineRepository(), new HAVBaseRepository()) { }
        
        public HAVWhoIsOnlineService(IHAVWhoIsOnlineRepository aRepository, IHAVBaseRepository baseRepository) : base(baseRepository) {
            theRepository = aRepository;
        }

        public void AddToWhoIsOnline(User aCurrentUser, string aCurrentIpAddress) {
            theRepository.RemoveFromWhoIsOnline(aCurrentUser, aCurrentIpAddress);
            theRepository.AddToWhoIsOnline(aCurrentUser, aCurrentIpAddress);
            theRepository.MarkForceLogOutOfOtherUsers(aCurrentUser, aCurrentIpAddress);
        }

        public bool IsOnline(User aCurrentUser, string aCurrentIpAddress) {
            WhoIsOnline onlineUser = theRepository.GetWhoIsOnlineEntry(aCurrentUser, aCurrentIpAddress);
            DateTime expiryTime = DateTime.UtcNow.AddSeconds(-1 * HAVConstants.SECONDS_BEFORE_USER_TIMEOUT);

            bool isOnline = true;
            if (onlineUser.DateTimeStamp.CompareTo(expiryTime) < 0 || onlineUser.ForceLogOut) {
                theRepository.RemoveFromWhoIsOnline(aCurrentUser, aCurrentIpAddress);
                isOnline = false;
            } else {
                theRepository.UpdateTimeOfWhoIsOnline(aCurrentUser, aCurrentIpAddress);
            }

            return isOnline;
        }

        public void RemoveFromWhoIsOnline(User aCurrentUser, string aCurrentIpAddress) {
            theRepository.RemoveFromWhoIsOnline(aCurrentUser, aCurrentIpAddress);
        }
    }
}