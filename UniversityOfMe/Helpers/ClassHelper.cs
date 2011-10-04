using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityOfMe.Models;
using Social.Generic.Models;

namespace UniversityOfMe.Helpers {
    public static class ClassHelper {
        public static string CreateClassString(Class aClass) {
            return aClass.Subject + aClass.Course + "-" + aClass.Section;
        }

        public static bool IsEnrolled(UserInformationModel<User> aUser, Class aClass) {
            if (aUser != null) {
                ClassEnrollment myEnrollment = (from ce in aClass.ClassEnrollments
                                                where ce.UserId == aUser.Details.Id
                                                select ce).FirstOrDefault<ClassEnrollment>();
                return myEnrollment == null ? false : true;
            } else {
                return false;
            }
        }
    }
}