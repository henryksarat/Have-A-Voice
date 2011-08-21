using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityOfMe.Helpers {
    public static class RegexHelper {
        public static string OnlyCharactersAndNumbersAndPeriods() {
            return @"^[a-zA-Z0-9\.]+$";
        }
    }
}