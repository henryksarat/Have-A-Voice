﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Models;

namespace HaveAVoice.Services.Helpers {
    public class NavigationHelper {
        private const string OPENING_SPAN = "<span class=\"alert\">";
        private const string CLOSING_SPAN = "</span>";

        public static string NewMessageCount(User aRequestingUser) {
            IHAVNavigationService myService = new HAVNavigationService();
            return BuildSpanReturn(myService.NewMessageCount(aRequestingUser));
        }

        public static string PendingFanCount(User aRequestingUser) {
            IHAVNavigationService myService = new HAVNavigationService();
            return BuildSpanReturn(myService.PendingFanCount(aRequestingUser));
        }

        public static string NotificationCount(User aRequestingUser) {
            IHAVNavigationService myService = new HAVNavigationService();
            return BuildSpanReturn(myService.NotificationCount(aRequestingUser));
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