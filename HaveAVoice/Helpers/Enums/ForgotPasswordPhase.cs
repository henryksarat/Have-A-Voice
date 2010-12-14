using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Helpers.Enums {
    public enum ForgotPasswordPhase {
        ENTER_EMAIL = 1,
        CONFIRM_HASH = 2,
        ENTER_NEW_PASSWORD = 3,
        CHANGE_PASSWORD = 4
    }
}