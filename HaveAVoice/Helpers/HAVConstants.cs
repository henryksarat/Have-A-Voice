using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;

namespace HaveAVoice.Helpers {
    public class HAVConstants {
        public const string BASE_URL = "www.haveavoice.us";
        public const string NOT_CONFIRMED_USER_ROLE = "Not confirmed";
        public const string USER_PICTURE_LOCATION_FROM_VIEW = "../../UserPictures/";
        public const string NO_PROFILE_PICTURE_IMAGE = "no_profile_picture.jpg";
        public const string NO_PROFILE_PICTURE = USER_PICTURE_LOCATION_FROM_VIEW + NO_PROFILE_PICTURE_IMAGE;
        public const long SECONDS_BEFORE_USER_TIMEOUT = 60 * 5;
        public static string PAGE_NOT_FOUND = "You do not have access.";
        public static string ERROR = "An error has occurred. Please try again.";

        public static List<string> STATES = new List<string>() { 
                "Select",
                "AL",
                "AK",
                "AZ",
                "AR",
                "CA",
                "CO",
                "CT",
                "DE",
                "FL",
                "GA",
                "HI",
                "ID",
                "IL",
                "IN",
                "IA",
                "KS",
                "KY",
                "LA",
                "ME",
                "MD",
                "MA",
                "MI",
                "MN",
                "MS",
                "MO",
                "MT",
                "NE",
                "NV",
                "NH",
                "NJ",
                "NM",
                "NY",
                "NC",
                "ND",
                "OH",
                "OK",
                "OR",
                "PA",
                "RI",
                "SC",
                "SD",
                "TN",
                "TX",
                "UT",",",
                "VT",
                "VA",
                "WA",
                "WV",
                "WI",
                "WY"};    
    }
}
