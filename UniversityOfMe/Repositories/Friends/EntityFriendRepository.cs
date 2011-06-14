using System.Collections.Generic;
using System.Linq;
using Social.Friend.Repositories;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Friends {
    public class EntityFriendRepository : IFriendRepository<User, Friend> {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void AddFriend(User aUser, int aSourceUserId) {
            Friend myFriend = Friend.CreateFriend(0, aUser.Id, aSourceUserId, false);
            theEntities.Friends.AddObject(myFriend);
            theEntities.SaveChanges();
        }

        public void ApproveFriend(int aFriendId, int aSourceUserId) {
            Friend myFriend = FindFriend(aFriendId, aSourceUserId);
            myFriend.Approved = true;

            Friend myInverseFriendEntry = Friend.CreateFriend(0, myFriend.SourceUserId, myFriend.FriendUserId, true);

            theEntities.AddToFriends(myInverseFriendEntry);
            theEntities.ApplyCurrentValues(myFriend.EntityKey.EntitySetName, myFriend);
            theEntities.SaveChanges();
        }

        public void DenyFriend(int aFriendId, int aSourceUserId) {
            Friend myFriend = FindFriend(aFriendId, aSourceUserId);
            theEntities.DeleteObject(myFriend);
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

        public void DeleteFriend(User aUser, int aSourceUserId) {
            IEnumerable<Friend> myFriends = GetFriendRecords(aUser, aSourceUserId);

            foreach (Friend myFriend in myFriends) {
                theEntities.DeleteObject(myFriend);
            }

            theEntities.SaveChanges();
        }

        private IEnumerable<Friend> GetFriendRecords(User aUser, int aSourceUserId) {
            return (from f in theEntities.Friends
                    where (f.FriendUserId == aUser.Id && f.SourceUserId == aSourceUserId)
                    || (f.FriendUserId == aSourceUserId && f.SourceUserId == aUser.Id)
                    select f).ToList<Friend>();
        }

        private Friend FindFriend(int aFriendId) {
            return (from f in theEntities.Friends
                    where f.Id == aFriendId
                    select f).FirstOrDefault<Friend>();
        }

        private Friend FindFriend(int aFriendId, int aSourceUserId) {
            return (from f in theEntities.Friends
                    where f.Id == aFriendId
                    && f.SourceUserId == aSourceUserId
                    select f).FirstOrDefault<Friend>();
        }
    }
}