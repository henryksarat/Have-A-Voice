using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityOfMe.Helpers {
    public static class URLHelper {
        public static string ToUrlFriendly(string aValue) {
            return aValue.Replace(' ', '_');
        }

        public static string[] FromUrlFriendly(string aValue) {
            return aValue.Replace('_', ' ').Split(' ');
        }
    }
}