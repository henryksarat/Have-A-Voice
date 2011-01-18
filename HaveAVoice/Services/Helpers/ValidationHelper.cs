using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Validation;

namespace HaveAVoice.Services.Helpers {
    public static class ValidationHelper {
        public static bool IsValidEmail(string anEmail) {
            return !String.IsNullOrEmpty(anEmail);
        }
    }
}