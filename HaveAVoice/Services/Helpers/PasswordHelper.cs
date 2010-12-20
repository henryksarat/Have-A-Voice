using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Services.Helpers {
    public class PasswordHelper {
        public static string HashPassword(string aPassword) {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(aPassword, "SHA1");
        }
    }
}