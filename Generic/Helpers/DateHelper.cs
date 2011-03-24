using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Social.Generic.Helpers {
    public class DateHelper {
        public static DateTime ConvertToLocalTime(DateTime dateTimeToConvert) {
            TimeZone myTimezone = TimeZone.CurrentTimeZone;
            int utcOffset =  myTimezone.GetUtcOffset(DateTime.Now).Hours;
            return dateTimeToConvert.AddHours(utcOffset);
        }
    }
}
