using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVFriendService {
        void AddFriend(User aUser, int aSourceUserId);
        void ApproveFriend(int aFriendId);
        void DeclineFriend(int aFriendId);
        IEnumerable<Friend> FindFriendsForUser(int aUserId);
        IEnumerable<Friend> FindPendingFriendsForUser(int aUserId);
        bool IsFriend(int aSourceUserId, User aFriendUser);
        bool IsPending(int aUserId, User aFriend);
    }
}