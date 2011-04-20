using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social.Validation {
    public class PasswordValidation {
        public static bool ValidPassword(IValidationDictionary aValidationDictionary, string aPassword, string aRetypedPassword) {
            if (aPassword.Trim().Length == 0) {
                aValidationDictionary.AddError("Password", "", "Password is required.");
            }
            if (aRetypedPassword == null || aRetypedPassword.Trim().Length == 0) {
                aValidationDictionary.AddError("RetypedPassword", "", "Please type your password again.");
            } else if (!aPassword.Equals(aRetypedPassword)) {
                aValidationDictionary.AddError("RetypedPassword", "", "Passwords must match.");
            }

            return aValidationDictionary.isValid;
        }
    }
}
