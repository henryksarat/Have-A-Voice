using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVFriendRepository {
        void AddFriend(User aUser, int aSourceUserId);
        void ApproveFriend(int aFriendId);
        void DeleteFriend(int aFriendId);
        IEnumerable<Friend> FindFriendsForUser(int aUserId);
        IEnumerable<Friend> FindPendingFriendsForUser(int aUserId);
        bool IsFriend(int aUserId, User aFriend);
        bool IsPending(int aUserId, User aFriend);
    }
}