using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVNavigationRepository {
        int GetUnreadMessagesReceivedCount(User aRequestingUser);
        int GetUnreadMessagesSentCount(User aRequestingUser);
        int GetPendingFriendRequestCount(User aRequestingUser);
        int GetUnviewedBoardCount(User aRequestingUser);
    }
}