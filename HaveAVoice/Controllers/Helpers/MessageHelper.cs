using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace HaveAVoice.Controllers.Helpers {
    public class MessageHelper {
        private const string DIV_END = "</div>";
        public static string SuccessMessage(string aMessage) {
            return CreateDivStart("success") + aMessage + DIV_END;
        }

        public static string ErrorMessage(string aMessage) {
            return CreateDivStart("error") + aMessage + DIV_END;
        }

        public static string NormalMessage(string aMessage) {
            return CreateDivStart("normal") + aMessage + DIV_END;
        }

        private static string CreateDivStart(string aClass) {
            return new StringBuilder().AppendFormat("<div class=\"{0}\">", aClass).ToString();
        }
    }
}