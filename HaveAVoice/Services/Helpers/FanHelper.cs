using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Helpers.UserInformation;

namespace HaveAVoice.Services.Helpers {
    public class FanHelper {
        public static bool IsFan(int aSourceUserId, User aUser) {
            IEnumerable<int> myFanIds =
                (from f in aUser.FanOfPeople
                 select f.SourceUserId).ToList<int>();

            return myFanIds.Contains(aSourceUserId);
        }
    }
}