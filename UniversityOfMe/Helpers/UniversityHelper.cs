using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityOfMe.Models;

namespace UniversityOfMe.Helpers {
    public static class UniversityHelper {
        public static bool IsFromUniversity(User aFrom, string aUniversityId) {
            if (aUniversityId == null) {
                return false;
            }

            bool myIsFromUniversity = false;
            foreach (UserUniversity myUserUniversity in aFrom.UserUniversities) {
                if (myUserUniversity.UniversityEmail.UniversityId.ToUpper().Equals(aUniversityId.ToUpper())) {
                    myIsFromUniversity = true;
                    break;
                }
            }
            return myIsFromUniversity;
        }

    }
}
