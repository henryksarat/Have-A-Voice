using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityOfMe.Models;

namespace UniversityOfMe.Helpers {
    public static class URLHelper {
        public static string ToUrlFriendly(string aValue) {
            return aValue.Replace(' ', '_');
        }

        public static string[] FromUrlFriendly(string aValue) {
            return aValue.Replace('_', ' ').Split(' ');
        }

        public static string BuildClassUrl(Class aClass) {
            return "/" + aClass.UniversityId + "/Class/Details/" + String.Join("_", aClass.ClassCode, aClass.AcademicTermId, aClass.Year);
        }
    }
}