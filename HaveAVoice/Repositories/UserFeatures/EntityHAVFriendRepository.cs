using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVFriendRepository : IHAVFriendRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public void AddFriend(User aUser, int aSourceUserId) {
            Friend myFriend = Friend.CreateFriend(0, aUser.Id, aSourceUserId, false);
            theEntities.Friends.AddObject(myFriend);
            theEntities.SaveChanges();
        }

        public void ApproveFriend(int aFriendId) {
            Friend myFriend = FindFriend(aFriendId);
            myFriend.Approved = true;

            Friend myInverseFriendEntry = Friend.CreateFriend(0, myFriend.SourceUserId, myFriend.FriendUserId, true);

            theEntities.AddToFriends(myInverseFriendEntry);
            theEntities.ApplyCurrentValues(myFriend.EntityKey.EntitySetName, myFriend);
            theEntities.SaveChanges();
        }

        public void DeleteFriend(int aFriendId) {
            Friend myFriend = FindFriend(aFriendId);
            theEntities.DeleteObject(myFriend);
            theEntities.SaveChanges();
        }

        public IEnumerable<Friend> FindFriendsForUser(int aUserId) {
            return (from f in theEntities.Friends
                    where (f.SourceUserId == aUserId)
                    && f.Approved == true
                    select f).ToList<Friend>();
        }

        public IEnumerable<Friend> FindPendingFriendsForUser(int aUserId) {
            return (from f in theEntities.Friends
                    where f.SourceUserId == aUserId
                    && f.Approved == false
                    select f).ToList<Friend>();
        }

        public bool IsFriend(int aUserId, User aFriend) {
            return (from f in theEntities.Friends
                    where (f.FriendUserId == aFriend.Id && f.SourceUserId == aUserId)
                    && f.Approved == true
                    select f).Count() > 0 ? true : false;
        }

        public bool IsPending(int aUserId, User aFriend) {
            return (from f in theEntities.Friends
                    where f.FriendUserId == aFriend.Id && f.SourceUserId == aUserId
                    && f.Approved == false
                    select f).Count() > 0 ? true : false;
        }

        public bool IsPendingForResponse(User aUser, int aFriendId) {
            return (from f in theEntities.Friends
                    where f.FriendUserId == aFriendId && f.SourceUserId == aUser.Id
                    && f.Approved == false
                    select f).Count() > 0 ? true : false;
        }

        private Friend FindFriend(int aFriendId) {
            return (from f in theEntities.Friends
                    where f.Id == aFriendId
                    select f).FirstOrDefault<Friend>();
        }
    }
}