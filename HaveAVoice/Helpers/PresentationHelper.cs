using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace HaveAVoice.Helpers {
    public static class PresentationHelper {
        public static string ReplaceCarriageReturnWithBR(string aString) {
            Regex regex = new Regex(@"\r\n");
            return regex.Replace(aString, "<br />");
        }
    }
}