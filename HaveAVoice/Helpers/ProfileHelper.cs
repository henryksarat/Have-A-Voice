using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Helpers {
    public class ProfileHelper {
        public static User GetAnonymousProfile() {
            return User.CreateUser(0, string.Empty, string.Empty, HAVConstants.ANONYMOUS, string.Empty, 
                string.Empty, string.Empty, DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow, 
                string.Empty, string.Empty, string.Empty);
        }
    }
}