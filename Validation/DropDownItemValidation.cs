using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Social.Generic.Constants;

namespace Social.Validation {
    public class DropDownItemValidation {
        public static bool IsValid(string aValue) {
            return !string.IsNullOrEmpty(aValue) && !aValue.Equals(Constants.SELECT);
        }
    }
}
