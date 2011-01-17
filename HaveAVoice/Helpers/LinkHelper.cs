using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Helpers {
    public static class LinkHelper {
        public static string ProfilePage(User aUser) {
            return "/Profile/Show/" + aUser.Id;
        }
    }
}