using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Helpers {
    public class RegexHelper {
        public static string IssueTitleRegex() {
            return @"^[a-zA-Z0-9]+(([\'\,\.\.\! -a-zA-Z0-9])?[a-zA-Z0-9]*)*$";
        }

        public static string OnlyCharactersAndNumbers() {
            return @"^[a-zA-Z0-9\.]+$";
        }
    }
}