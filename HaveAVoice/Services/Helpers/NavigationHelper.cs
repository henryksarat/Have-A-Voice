using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Models;

namespace HaveAVoice.Services.Helpers {
    public class NavigationHelper {
        public static int NewMessageCount(User aRequestingUser) {
            IHAVNavigationService myService = new HAVNavigationService();
            return myService.NewMessageCount(aRequestingUser);
        }

        public static int PendingFanCount(User aRequestingUser) {
            IHAVNavigationService myService = new HAVNavigationService();
            return myService.PendingFanCount(aRequestingUser);
        }
    }
}