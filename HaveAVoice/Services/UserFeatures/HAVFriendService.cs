using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Repositories;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVFriendService : HAVBaseService, IHAVFriendService {
        private IHAVFriendRepository theFriendRepo;

        public HAVFriendService()
            : this(new HAVBaseRepository(), new EntityHAVFriendRepository()) { }

        public HAVFriendService(IHAVBaseRepository aBaseRepository, IHAVFriendRepository aFriendRepo)
            : base(aBaseRepository) {
                theFriendRepo = aFriendRepo;
        }

        public void AddFriend(User aUser, int aSourceUserId) {
            theFriendRepo.AddFriend(aUser, aSourceUserId);
        }

        public void ApproveFriend(int aFriendId) {
            theFriendRepo.ApproveFriend(aFriendId);
        }

        public void DeclineFriend(int aFriendId) {
            theFriendRepo.DeleteFriend(aFriendId);
        }

        public IEnumerable<Friend> FindFriendsForUser(int aUserId) {
            return theFriendRepo.FindFriendsForUser(aUserId);
        }

        public IEnumerable<Friend> FindPendingFriendsForUser(int aUserId) {
            return theFriendRepo.FindPendingFriendsForUser(aUserId);
        }

        public bool IsFriend(int aUserId, User aFriend) {
            return aUserId == aFriend.Id || theFriendRepo.IsFriend(aUserId, aFriend);
        }

        public bool IsPending(int aUserId, User aFriend) {
            return theFriendRepo.IsPending(aUserId, aFriend);
        }

       public bool IsPendingForResponse(User aUser, int aFriendId) {
           return theFriendRepo.IsPendingForResponse(aUser, aFriendId);
        }

       public void RemoveFriend(User aUser, int aSourceId) {
           theFriendRepo.DeleteFriend(aUser, aSourceId);
       }
    }
}