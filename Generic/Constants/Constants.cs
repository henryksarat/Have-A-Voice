﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Social.Generic.Helpers;

namespace Social.Generic.Constants {
    public class Constants {
        public const string PHOTO_LOCATION_FROM_VIEW = "/Photos/";
        public const string NO_PROFILE_PICTURE_IMAGE = "no_profile_picture.jpg";
        public const string NO_PROFILE_PICTURE_URL = PHOTO_LOCATION_FROM_VIEW + NO_PROFILE_PICTURE_IMAGE;
        public const string ANONYMOUS_PICTURE_URL = NO_PROFILE_PICTURE_URL;
        public const string ANONYMOUS = "Anonymous";

        public const string NOT_CONFIRMED_USER_ROLE = "Not confirmed";

        public const long SECONDS_BEFORE_USER_TIMEOUT = 60 * 60;
        public const string PAGE_NOT_FOUND = "You do not have access.";
        public const string NOT_ALLOWED = "You are not allowed to do that.";
        public const string PERMISSION_DENIED = "You are not allowed to perform that action.";
        public const string ERROR = "An error has occurred. Please try again.";
        public const string NOT_FRIEND = "You must be a friend of the user.";

        public const string SELECT = "Select";

        public static List<string> GENDERS = new List<string>() { 
            SELECT,
            Gender.MALE,
            Gender.FEMALE
        };

        public static IDictionary<string, string> GetTimes() {
            IDictionary<string, string> myDictionary = new Dictionary<string, string>();
            myDictionary.Add("12:00 AM", "12:00 AM");

            for(int myCounter = 1 ; myCounter < 12 ; myCounter++) {
                string myTime = myCounter + ":00 AM";
                if (myCounter < 10) {
                    myTime = "0" + myCounter + ":00 AM";
                }
                myDictionary.Add(myTime, myTime);
            }

            myDictionary.Add("12:00 PM", "12:00 PM");

            for (int myCounter = 1; myCounter < 12; myCounter++) {
                string myTime = myCounter + ":00 PM";
                if (myCounter < 10) {
                    myTime = "0" + myCounter + ":00 PM";
                }
                myDictionary.Add(myTime, myTime);
            }

            return myDictionary;
        }
    }
}
