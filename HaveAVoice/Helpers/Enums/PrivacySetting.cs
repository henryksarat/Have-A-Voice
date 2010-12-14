using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Helpers.Enums {
    public enum PrivacySetting {
        DisplayProfileToNonLoggedIn = 0,
        DisplayProfileToLoggedInUsers = 1,
        DisplayProfileToFriends = 2
    }
}
