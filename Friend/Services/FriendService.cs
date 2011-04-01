using System.Collections.Generic;
using Social.Friend.Repositories;
using Social.Generic.Models;

namespace Social.Friend.Services {
    public class FriendService<T, U> : IFriendService<T, U> {
        private IFriendRepository<T, U> theFriendRepo;

        public FriendService(IFriendRepository<T, U> aFriendRepo) {
            theFriendRepo = aFriendRepo;
        }

        public void AddFriend(AbstractUserModel<T> aUser, int aSourceUserId) {
            theFriendRepo.AddFriend(aUser.Model, aSourceUserId);
        }

        public void ApproveFriend(int aFriendId) {
            theFriendRepo.ApproveFriend(aFriendId);
        }

        public void DeclineFriend(int aFriendId) {
            theFriendRepo.DeleteFriend(aFriendId);
        }

        public IEnumerable<U> FindFriendsForUser(int aUserId) {
            return theFriendRepo.FindFriendsForUser(aUserId);
        }

        public IEnumerable<U> FindPendingFriendsForUser(int aUserId) {
            return theFriendRepo.FindPendingFriendsForUser(aUserId);
        }

        public bool IsFriend(int aUserId, AbstractUserModel<T> aFriend) {
            return aUserId == aFriend.Id || theFriendRepo.IsFriend(aUserId, aFriend.Model);
        }

        public bool IsPending(int aUserId, AbstractUserModel<T> aFriend) {
            return theFriendRepo.IsPending(aUserId, aFriend.Model);
        }

        public bool IsPendingForResponse(AbstractUserModel<T> aUser, int aFriendId) {
            return theFriendRepo.IsPendingForResponse(aUser.Model, aFriendId);
        }

        public void RemoveFriend(AbstractUserModel<T> aUser, int aSourceId) {
            theFriendRepo.DeleteFriend(aUser.Model, aSourceId);
        }
    }
}