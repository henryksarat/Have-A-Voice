using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVWhoIsOnlineRepository {
        WhoIsOnline GetWhoIsOnlineEntry(User currentUser, string currentIpAddress);
        void AddToWhoIsOnline(User currentUser, string currentIpAddress);
        void UpdateTimeOfWhoIsOnline(User currentUser, string currentIpAddress);
        void MarkForceLogOutOfOtherUsers(User currentUser, string currentIpAddress);
        void RemoveFromWhoIsOnline(User currentUser, string currentIpAddress);
    }
}