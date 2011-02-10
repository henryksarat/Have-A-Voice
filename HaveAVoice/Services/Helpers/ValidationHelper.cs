using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Validation;
using System.Text.RegularExpressions;

namespace HaveAVoice.Services.Helpers {
    public static class ValidationHelper {
        public static bool IsValidEmail(string anEmail) {
            anEmail = anEmail == null ? string.Empty : anEmail;

            string myRegexExpression = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex myRegex = new Regex(myRegexExpression);
            if (myRegex.IsMatch(anEmail)) {
                return (true);
            } else {
                return (false);
            }
        }
    }
}