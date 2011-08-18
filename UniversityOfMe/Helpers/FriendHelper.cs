using System.Collections.Generic;
using System.Linq;
using UniversityOfMe.Models;

namespace UniversityOfMe.Helpers {
    public class FriendHelper {
        public static bool IsFriend(User aSourceUser, User aTargetUserCheckingToSeeIfFriend) {
            bool myIsFriend = true;

            if (aSourceUser.Id != aTargetUserCheckingToSeeIfFriend.Id) {
                IEnumerable<int> myFriendIds =
                    (from f in aSourceUser.Friends.ToList<Friend>()
                     select f.SourceUserId).ToList<int>(); ;

                myIsFriend = myFriendIds.Contains(aTargetUserCheckingToSeeIfFriend.Id);
            }

            return myIsFriend;
        }

        public static bool IsPendingFriendRequest(User aSourceUser, User aTargetUserCheckingToSeeIfPendingFriendRequest) {
            bool myIsPending = false;

            if (aSourceUser.Id != aTargetUserCheckingToSeeIfPendingFriendRequest.Id) {
                Friend myPossibleFriendRequest = (from f in aSourceUser.Friends
                                                  where f.SourceUserId == aTargetUserCheckingToSeeIfPendingFriendRequest.Id
                                                  select f).FirstOrDefault<Friend>();
                if (myPossibleFriendRequest != null) {
                    myIsPending = !myPossibleFriendRequest.Approved;
                } else {
                    myPossibleFriendRequest = (from f in aTargetUserCheckingToSeeIfPendingFriendRequest.Friends
                                               where f.SourceUserId == aSourceUser.Id
                                               select f).FirstOrDefault<Friend>();
                    if (myPossibleFriendRequest != null) {
                        myIsPending = !myPossibleFriendRequest.Approved;
                    }
                }
            }

            return myIsPending;
        }
    }
}