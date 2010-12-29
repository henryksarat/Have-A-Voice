using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVNavigationService {
        int NewMessageCount(User aRequestingUser);
        int PendingFanCount(User aRequestingUser);
        int NotificationCount(User aRequestingUser);
    }
}