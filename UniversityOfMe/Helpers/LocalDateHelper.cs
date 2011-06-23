using System;

namespace UniversityOfMe.Helpers {
    public static class LocalDateHelper {
        private const string TODAY_FORMAT = "{0: hh:mm tt }";
        private const string YESTERDAYS_FORMAT = "{0: hh:mm tt }";
        private const string THIS_WEEK_FORMAT = "{0: dddd hh:mm tt }";
        private const string AFTER_THIS_WEEK_FORMAT = "{0: MM/dd/yyyy hh:mm tt }";

        public static string ToFormattedLocalTime(DateTime aDateTime, string aFormat) {
            TimeZone localZone = TimeZone.CurrentTimeZone;
            return string.Format(aFormat, localZone.ToLocalTime(aDateTime));
        }

        public static string ToLocalTime(DateTime aDateTime) {
            return ToLocalTime(aDateTime, DateTime.UtcNow, AFTER_THIS_WEEK_FORMAT);
        }

        public static string ToLocalTime(DateTime aDateTime, string aFormat) {
            return ToLocalTime(aDateTime, DateTime.UtcNow, aFormat);
        }

        public static string ToLocalTime(DateTime aDateTime, DateTime aCurrentUtcTime, string aFormat) {
            TimeZone myLocalZone = TimeZone.CurrentTimeZone;
            DateTime myLocalDateTime = myLocalZone.ToLocalTime(aDateTime);
            DateTime myLocalCurrentTime = myLocalZone.ToLocalTime(aCurrentUtcTime);
            
            string myFormattedDate = string.Empty;

            if (myLocalDateTime.Date > myLocalCurrentTime.Date) {
                myFormattedDate = string.Format(aFormat, myLocalZone.ToLocalTime(aDateTime)).Trim();
            }  else if (myLocalCurrentTime.Date.Equals(myLocalDateTime.Date)) {
                myFormattedDate = string.Format(TODAY_FORMAT, myLocalZone.ToLocalTime(aDateTime)).Trim();
            } else if (myLocalCurrentTime.Subtract(TimeSpan.FromDays(1.0)).Date.Equals(myLocalDateTime.Date)) {
                myFormattedDate = "Yesterday " + string.Format(YESTERDAYS_FORMAT, myLocalZone.ToLocalTime(aDateTime)).Trim();
            } else if (myLocalCurrentTime.Subtract(TimeSpan.FromDays(7.0)).Date < myLocalDateTime.Date) {
                myFormattedDate = string.Format(THIS_WEEK_FORMAT, myLocalZone.ToLocalTime(aDateTime)).Trim();
            } else {
                myFormattedDate = string.Format(aFormat, myLocalZone.ToLocalTime(aDateTime)).Trim();
            }

            return myFormattedDate;
        }

        public static string GetTodaysFormat() {
            return TODAY_FORMAT;
        }

        public static string GetYesterdaysFormat() {
            return YESTERDAYS_FORMAT;
        }

        public static string GetThisWeekFormat() {
            return THIS_WEEK_FORMAT;
        }

        public static string GetAfterThisWeekFormat() {
            return AFTER_THIS_WEEK_FORMAT;
        }
    }
}