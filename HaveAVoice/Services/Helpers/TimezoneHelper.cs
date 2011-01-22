using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace HaveAVoice.Services.Helpers {
    public class TimezoneHelper {
        private static Hashtable theOffsetHashTable = new Hashtable();
        private static Hashtable theTimezoneHashTable = new Hashtable();

        private static void initHashTables() {
            if (theOffsetHashTable.Count == 0 && theTimezoneHashTable.Count == 0) {
                theOffsetHashTable.Add("(GMT-07:00) Mountain Time (US & Canada)", "-07:00:00");
                theOffsetHashTable.Add("(GMT-06:00) Central Time (US & Canada)", "-06:00:00");
                theOffsetHashTable.Add("(GMT-05:00) Eastern Time (US & Canada)", "-05:00:00");

                theTimezoneHashTable.Add("-07:00:00", "(GMT-07:00) Mountain Time (US & Canada)");
                theTimezoneHashTable.Add("-06:00:00", "(GMT-06:00) Central Time (US & Canada)");
                theTimezoneHashTable.Add("-05:00:00", "(GMT-05:00) Eastern Time (US & Canada)");
            }
        }

        public static List<string> GetTimeZones() {
            List<string> timezones = new List<string>();
            timezones.Add("(GMT-07:00) Mountain Time (US & Canada)");
            timezones.Add("(GMT-06:00) Central Time (US & Canada)");
            timezones.Add("(GMT-05:00) Eastern Time (US & Canada)");

            return timezones;
        }

        public static string GetOffset(string timezone) {
            initHashTables();
            return theOffsetHashTable[timezone].ToString();
        }

        public static DateTime ConvertToLocalTimeZone(DateTime aDateTime) {
            TimeZone myTimezone = TimeZone.CurrentTimeZone;
            return myTimezone.ToLocalTime(aDateTime);
        }

        public static string GetTimezone(string offset) {
            initHashTables();
            return theTimezoneHashTable[offset].ToString();
        }
    }
}
