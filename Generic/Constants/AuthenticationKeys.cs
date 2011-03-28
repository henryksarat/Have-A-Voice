using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social.Generic.Constants {
    public class AuthenticationKeys {
        public const string ACCOUNT_ACTIVATED_TITLE = "Account activated!";
        public const string LOGGED_OUT = "You have logged out successfully.";

        public static string AUTHENTICAITON_ERROR = "Error authenticating. Please try again.";
        public static string INCORRECT_LOGIN = "Incorrect username and password combination.";
        public static string INVALID_ACTIVATION_CODE = "Invalid activation code.";
        public static string SPECIFIC_ROLE_ERROR = "Unable to activate the account because of a role issue.";
        public static string OUR_ERROR = "Couldn't activate the account because of something on our end. Please try again later.";
        public static string ACTIVATION_ERROR = "Error while activating your account. Please try again.";
    }
}
