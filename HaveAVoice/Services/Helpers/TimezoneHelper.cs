using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace HaveAVoice.Services.Helpers {
    public class TimezoneHelper {
        public static DateTime ConvertToLocalTimeZone(DateTime aDateTime) {
            TimeZone myTimezone = TimeZone.CurrentTimeZone;
            return myTimezone.ToLocalTime(aDateTime);
        }
    }
}
