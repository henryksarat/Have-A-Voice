using System.Collections.Generic;
using Social.Generic.Models;

namespace Social.Friend.Services {
    public interface IFriendService<T, U> {
        void AddFriend(AbstractUserModel<T> aUser, int aSourceUserId);
        void ApproveFriend(int aFriendId, int aSourceUserId);
        void DeclineFriend(int aFriendId, int aSourceUserId);
        void RemoveFriend(AbstractUserModel<T> aUser, int aSourceId);
        IEnumerable<U> FindFriendsForUser(int aUserId);
        IEnumerable<U> FindPendingFriendsForUser(int aUserId);
        bool IsFriend(int aSourceUserId, AbstractUserModel<T> aFriendUser);
        bool IsPending(int aUserId, AbstractUserModel<T> aFriend);
        bool IsPendingForResponse(AbstractUserModel<T> aUser, int aFriendId);
    }
}