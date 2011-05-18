using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityOfMe.Models;
using UniversityOfMe.Services.UserFeatures;

namespace UniversityOfMe.Helpers {
    public class NavigationCountHelper {
        private const string OPENING_SPAN = "<span class=\"alert\">";
        private const string CLOSING_SPAN = "</span>";

        public static string NewMessageCount(User aRequestingUser) {
            INavigationService myService = new NavigationService();
            return myService.NewMessageCount(aRequestingUser).ToString();
        }

        public static string PendingFriendCount(User aRequestingUser) {
            INavigationService myService = new NavigationService();
            return myService.PendingFriendCount(aRequestingUser).ToString();
        }
    }
}