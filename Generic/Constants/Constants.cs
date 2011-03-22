using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social.Generic.Constants {
    public class Constants {
        public const string PHOTO_LOCATION_FROM_VIEW = "/Photos/";
        public const string NO_PROFILE_PICTURE_IMAGE = "no_profile_picture.jpg";
        public const string NO_PROFILE_PICTURE_URL = PHOTO_LOCATION_FROM_VIEW + NO_PROFILE_PICTURE_IMAGE;
        public const string ANONYMOUS_PICTURE_URL = NO_PROFILE_PICTURE_URL;
        public const string ANONYMOUS = "Anonymous";

        public const long SECONDS_BEFORE_USER_TIMEOUT = 60 * 30;
        public static string PAGE_NOT_FOUND = "You do not have access.";
        public const string NOT_ALLOWED = "You are not allowed to do that.";
        public static string ERROR = "An error has occurred. Please try again.";
        public static string NOT_FRIEND = "You must be a friend of the user.";

        public const string SELECT = "Select";

        public static List<string> GENDERS = new List<string>() { 
            SELECT,
            Gender.MALE,
            Gender.FEMALE
        };
    }
}
