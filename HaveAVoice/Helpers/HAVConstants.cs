using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Helpers {
    public class HAVConstants {
        public const string BASE_URL = "http://www.haveavoice.com";
        public const string NOT_CONFIRMED_USER_ROLE = "Not confirmed";
        public const string AUTHORITY_USER_ROLE = "Authority";

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

        public const string FILTER_TEMP_DATA = "Filter";
        public const string ORIGINAL_ISSUE_TEMP_DATA = "OriginalIssue";
        public const string ORIGINAL_MYPROFILE_FEED_TEMP_DATA = "OriginalFeed";

        public const bool MALE = true;
        public const bool FEMALE = false;


        public static List<string> INQUIRY_TYPES = new List<string>() {
            "Select",
            "Feedback",
            "Bug Report",
            "Privacy",
            "Investment",
            "Politician Account Creation",
            "Political Candidate Account Creation"
        };

        public static List<string> AUTHORITY_ROLES = new List<string>() {
            "Select",
            Roles.POLITICIAN,
            Roles.POLITICAL_CANDIDATE
        };
    }
}
