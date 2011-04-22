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
    }
}