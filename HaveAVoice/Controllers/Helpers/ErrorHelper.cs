using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace HaveAVoice.Controllers.Helpers {
    public class ErrorHelper {
        public static string ErrorString(string aFormattedString, params object[] anArgs) {
            return new StringBuilder().AppendFormat(aFormattedString, anArgs).ToString();
        }
    }
}