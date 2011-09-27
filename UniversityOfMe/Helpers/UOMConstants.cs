﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Social.Generic.Constants;
using System.Web.Mvc;

namespace UniversityOfMe.Helpers {
    public class UOMConstants {
        public const string TITLE = "University of Me";
        public const string BASE_URL = "http://www.universityof.me";
        public const string ACTIVATION_BODY = "Hello!<br /><br />To finalize completion of your UniversityOf.Me account, please click following link or copy and paste it into your browser: ";
        public const string ACTIVATION_SUBJECT = "universityOf.Me | account activation";

        public static List<string> ACADEMIC_TERMS = new List<string>() { 
            Social.Generic.Constants.Constants.SELECT,
            "Spring",
            "Summer",
            "Fall",
            "Winter"
        };

        public static List<int> ACADEMIC_YEARS = new List<int>() { 
            DateTime.UtcNow.Year,
            DateTime.UtcNow.Year - 1,
            DateTime.UtcNow.Year - 2,
            DateTime.UtcNow.Year - 3,
            DateTime.UtcNow.Year - 4,
            DateTime.UtcNow.Year - 5
        };

        public const string NOT_APART_OF_UNIVERSITY = "You are not apart of this university.";

        public const string DEFAULT_PROFILE_PHOTO_ALBUM = "My First Album";

        public const string UNVIERSITY_MAIN_CONTROLLER = "University";
        public const string UNVIERSITY_MAIN_VIEW = "Main";

        public const int APPROVED = 1;
        public const int DENIED = 0;
        public const int PENDING = 2;

        public const string URL_PREPEND = "";

        public static List<string> INQUIRY_TYPES = new List<string>() {
            "Select",
            "Feedback",
            "Bug Report",
            "Privacy",
            "Investment"
        };
    }
}