using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVWhoIsOnlineService {
        void AddToWhoIsOnline(User aCurrentUser, string aCurrentIpAddress);
        bool IsOnline(User aCurrentUser, string aCurrentIpAddress);
        void RemoveFromWhoIsOnline(User currentUser, string currentIpAddress);
    }
}