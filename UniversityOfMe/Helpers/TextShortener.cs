using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityOfMe.Helpers {
    public class TextShortener {
        public static string Shorten(string aText, int aLength) {
            if (aText.Length >= aLength) {
                return aText.Substring(0, aLength) + "...";
            }

            return aText;
        }
    }
}