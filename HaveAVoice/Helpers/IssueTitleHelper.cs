using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Helpers {
    public static class IssueTitleHelper {
        public static string ConvertForUrl(string aTitle) {
            return aTitle.Replace(' ', '-');
        }

        public static string ConvertFromUrl(string aUrl) {
            return aUrl.Replace('-', ' ');
        }
    }
}