﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Social.Generic.Constants {
    public class UnitedStates {
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

        public static SelectList STATE_SELECTION_LIST {
            get { return new SelectList(STATE_DICTIONARY, "Value", "Key"); }
        }

        private static readonly IDictionary<string, string> STATE_DICTIONARY = new Dictionary<string, string> {
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
    }
}