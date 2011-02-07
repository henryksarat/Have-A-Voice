using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Services.Helpers {
    public class FriendHelper {
        public static bool IsFriend(User aSourceUser, User aTargetUser) {
            bool myIsFriend = true;

            if (aSourceUser.Id != aTargetUser.Id) {
                IEnumerable<int> myFriendIds =
                    (from f in aSourceUser.Friends.ToList<Friend>()
                     select f.Id).ToList<int>(); ;

                myIsFriend = myFriendIds.Contains(aTargetUser.Id);
            }

            return myIsFriend;
        }
    }
}