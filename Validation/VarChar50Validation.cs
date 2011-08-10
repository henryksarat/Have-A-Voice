using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social.Validation {
    public class VarCharValidation {
        public static bool Valid50Length(string aText) {
            return aText.Length <= 50;
        }

        public static bool Valid100Length(string aText) {
            return aText.Length <= 100;
        }

        public static bool Valid15Length(string aText) {
            return aText.Length <= 15;
        }
    }
}
