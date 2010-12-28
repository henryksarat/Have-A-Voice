using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVNavigationRepository {
        int TotalUnreadMessagesReceived(User aRequestingUser);
        int TotalUnreadMessagesSent(User aRequestingUser);
    }
}