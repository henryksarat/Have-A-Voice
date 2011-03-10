using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Models;

namespace HaveAVoice.Services.Helpers {
    public class NavigationCountHelper {
        private const string OPENING_SPAN = "<span class=\"alert\">";
        private const string CLOSING_SPAN = "</span>";

        public static string NewMessageCount(User aRequestingUser) {
            IHAVNavigationService myService = new HAVNavigationService();
            return BuildSpanReturn(myService.NewMessageCount(aRequestingUser));
        }

        public static string PendingFriendCount(User aRequestingUser) {
            IHAVNavigationService myService = new HAVNavigationService();
            return BuildSpanReturn(myService.PendingFriendCount(aRequestingUser));
        }

        public static string NotificationCount(User aRequestingUser) {
            IHAVNotificationService myService = new HAVNotificationService();
            return BuildSpanReturn(myService.GetNotificationCount(aRequestingUser));
        }

        private static string BuildSpanReturn(int aCount) {
            if (aCount < 1) {
                return string.Empty;
            } else {
                return OPENING_SPAN + aCount.ToString() + CLOSING_SPAN;
            }
        }
    }
}