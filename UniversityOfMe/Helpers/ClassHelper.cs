using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityOfMe.Models;
using Social.Generic.Models;

namespace UniversityOfMe.Helpers {
    public static class ClassHelper {
        public static string CreateClassString(Class aClass) {
            return aClass.Subject + aClass.Course + "-" + aClass;
        }
    }
}