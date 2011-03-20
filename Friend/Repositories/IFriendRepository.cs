using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Social.Friend.Repositories {
    public interface IFriendRepository<T, U> {
        void AddFriend(T aUser, int aSourceUserId);
        void ApproveFriend(int aFriendId);
        void DeleteFriend(int aFriendId);
        void DeleteFriend(T aUser, int aSourceUserId);
        IEnumerable<U> FindFriendsForUser(int aUserId);
        IEnumerable<U> FindPendingFriendsForUser(int aUserId);
        bool IsFriend(int aUserId, T aFriend);
        bool IsPending(int aUserId, T aFriend);
        bool IsPendingForResponse(T aUser, int aFriendId);
    }
}