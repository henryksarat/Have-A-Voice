using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Services.Helpers {
    public class HashHelper {
        public static string HashPassword(string aPassword) {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(aPassword, "SHA1");
        }

        public static string HashAuthorityVerificationToken(string aToken) {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(aToken, "SHA1");
        }
    }
}