using System;
using System.Web.Mvc;
using HaveAVoice.Helpers;
using System.Web.Routing;
using HaveAVoice.Models;

namespace HaveAVoice.Helpers.UI {
    public class UserHelper {
        public static String UserAgreement() {
            return "There are the terms, agree to this or die!";
        }
    }
}
