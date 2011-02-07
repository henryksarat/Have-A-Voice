using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Helpers {
    public class HAVConstants {
        public const string BASE_URL = "www.haveavoice.us";
        public const string NOT_CONFIRMED_USER_ROLE = "Not confirmed";
        public const string AUTHORITY_USER_ROLE = "Authority";

        public const string PHOTO_LOCATION_FROM_VIEW = "/Photos/";
        public const string NO_PROFILE_PICTURE_IMAGE = "no_profile_picture.jpg";
        public const string NO_PROFILE_PICTURE_URL = PHOTO_LOCATION_FROM_VIEW + NO_PROFILE_PICTURE_IMAGE;
        public const string ANONYMOUS_PICTURE_URL = PHOTO_LOCATION_FROM_VIEW + "anonymous_picture.jpg";

        public const long SECONDS_BEFORE_USER_TIMEOUT = 60 * 30;
        public static string PAGE_NOT_FOUND = "You do not have access.";
        public const string NOT_ALLOWED = "You are not allowed to do that";
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
            "Investor",
            "Politician Account Creation",
            "Political Candidate Account Creation"
        };

        public static List<string> AUTHORITY_ROLES = new List<string>() {
            "Select",
            Roles.POLITICIAN,
            Roles.POLITICAL_CANDIDATE
        };

        public static List<string> GENDERS = new List<string>() { 
            HAVGender.Select.ToString(),
            HAVGender.Male.ToString(),
            HAVGender.Female.ToString()
        };

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
                "UT",
                "VT",
                "VA",
                "WA",
                "WV",
                "WI",
                "WY"};    
    }
    
	public class UnitedStatesStates
	{
	    public static readonly IDictionary<string, string> StateDictionary = new Dictionary<string, string> {
	        {"ALABAMA", "AL"},
	        {"ALASKA", "AK"},
	        {"AMERICAN SAMOA", "AS"},
	        {"ARIZONA ", "AZ"},
	        {"ARKANSAS", "AR"},
	        {"CALIFORNIA ", "CA"},
	        {"COLORADO ", "CO"},
	        {"CONNECTICUT", "CT"},
	        {"DELAWARE", "DE"},
	        {"DISTRICT OF COLUMBIA", "DC"},
	        {"FEDERATED STATES OF MICRONESIA", "FM"},
	        {"FLORIDA", "FL"},
	        {"GEORGIA", "GA"},
	        {"GUAM ", "GU"},
	        {"HAWAII", "HI"},
	        {"IDAHO", "ID"},
	        {"ILLINOIS", "IL"},
	        {"INDIANA", "IN"},
	        {"IOWA", "IA"},
	        {"KANSAS", "KS"},
	        {"KENTUCKY", "KY"},
	        {"LOUISIANA", "LA"},
	        {"MAINE", "ME"},
	        {"MARSHALL ISLANDS", "MH"},
	        {"MARYLAND", "MD"},
	        {"MASSACHUSETTS", "MA"},
	        {"MICHIGAN", "MI"},
	        {"MINNESOTA", "MN"},
	        {"MISSISSIPPI", "MS"},
	        {"MISSOURI", "MO"},
	        {"MONTANA", "MT"},
	        {"NEBRASKA", "NE"},
	        {"NEVADA", "NV"},
	        {"NEW HAMPSHIRE", "NH"},
	        {"NEW JERSEY", "NJ"},
	        {"NEW MEXICO", "NM"},
	        {"NEW YORK", "NY"},
	        {"NORTH CAROLINA", "NC"},
	        {"NORTH DAKOTA", "ND"},
	        {"NORTHERN MARIANA ISLANDS", "MP"},
	        {"OHIO", "OH"},
	        {"OKLAHOMA", "OK"},
	        {"OREGON", "OR"},
	        {"PALAU", "PW"},
	        {"PENNSYLVANIA", "PA"},
	        {"PUERTO RICO", "PR"},
	        {"RHODE ISLAND", "RI"},
	        {"SOUTH CAROLINA", "SC"},
	        {"SOUTH DAKOTA", "SD"},
	        {"TENNESSEE", "TN"},
	        {"TEXAS", "TX"},
	        {"UTAH", "UT"},
	        {"VERMONT", "VT"},
	        {"VIRGIN ISLANDS", "VI"},
	        {"VIRGINIA", "VA"},
	        {"WASHINGTON", "WA"},
	        {"WEST VIRGINIA", "WV"},
	        {"WISCONSIN", "WI"},
	        {"WYOMING", "WY"}
	    };
	
	    public static SelectList StateSelectList {
	        get { return new SelectList(StateDictionary, "Value", "Key"); }
	    }
	}
    
}
