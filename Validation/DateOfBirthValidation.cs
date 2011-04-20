using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social.Validation {
    public class DateOfBirthValidation {
        public static void ValidDateOfBirth(IValidationDictionary aValidationDictionary, DateTime aDateOfBirth) {
            if (aDateOfBirth.Year == 1) {
                aValidationDictionary.AddError("DateOfBirth", aDateOfBirth.ToString(), "Date of Birth required.");
            }
            if (aDateOfBirth > DateTime.Today.AddYears(-18)) {
                aValidationDictionary.AddError("DateOfBirth", aDateOfBirth.ToString(), "You must be at least 18 years old.");
            }
        }
    }
}
