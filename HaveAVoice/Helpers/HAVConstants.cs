﻿using System;
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
        public const string NO_PROFILE_PICTURE_URL = USER_PICTURE_LOCATION_FROM_VIEW + NO_PROFILE_PICTURE_IMAGE;
        public const long SECONDS_BEFORE_USER_TIMEOUT = 60 * 5;
        public static string PAGE_NOT_FOUND = "You do not have access.";
        public static string ERROR = "An error has occurred. Please try again.";
        public static string NOT_FAN = "You must be a fan of the user.";

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
	
	    public static SelectList StateSelectList
	    {
	        get { return new SelectList(StateDictionary, "Value", "Key"); }
	    }
	}
    
}
