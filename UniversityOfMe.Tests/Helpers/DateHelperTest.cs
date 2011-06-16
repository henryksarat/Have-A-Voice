using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniversityOfMe.Helpers;
using System;

namespace UniversityOfMe.Tests.Helpers {
    [TestClass]
    public class DateHelperTest {
        private static int UTC_HOUR = 6;
        private static DateTime TODAYS_DATE_TIME_IN_UTC = new DateTime(2011, 5, 18, 10, 3, 3);
        private static int UTC_OFFSET = TimeZone.CurrentTimeZone.GetUtcOffset(TODAYS_DATE_TIME_IN_UTC).Hours;
        private static string EXPECTED_HOUR = "0" + (UTC_HOUR + UTC_OFFSET);

        [TestMethod]
        public void TodaysDateIsInCorrectFormat() {
            DateTime myPostedDateTimeInUtc = new DateTime(2011, 5, 18, UTC_HOUR, 3, 3);

            string myExpected = EXPECTED_HOUR + ":03 AM";

            Assert.AreEqual(myExpected, DateHelper.ToLocalTime(myPostedDateTimeInUtc, TODAYS_DATE_TIME_IN_UTC));
        }

        [TestMethod]
        public void YesterdaysDateIsInCorrectFormat() {
            DateTime myPostedDateTimeInUtc = new DateTime(2011, 5, 17, UTC_HOUR, 3, 3);

            string myExpected = "Yesterday " + EXPECTED_HOUR + ":03 AM";

            Assert.AreEqual(myExpected, DateHelper.ToLocalTime(myPostedDateTimeInUtc, TODAYS_DATE_TIME_IN_UTC));
        }

        [TestMethod]
        public void ThisWeeksDateIsInCorrectFormat() {
            DateTime myPostedDateTimeInUtc = new DateTime(2011, 5, 12, UTC_HOUR, 3, 3);

            string myExpected = "Thursday " + EXPECTED_HOUR + ":03 AM";

            Assert.AreEqual(myExpected, DateHelper.ToLocalTime(myPostedDateTimeInUtc, TODAYS_DATE_TIME_IN_UTC));
        }

        [TestMethod]
        public void ExactlySevenBeforeTheAfterThisWeekDateIsInCorrectFormat() {
            DateTime myPostedDateTimeInUtc = new DateTime(2011, 5, 11, UTC_HOUR, 3, 3);

            string myExpected = "05/11/2011 " + EXPECTED_HOUR + ":03 AM";

            Assert.AreEqual(myExpected, DateHelper.ToLocalTime(myPostedDateTimeInUtc, TODAYS_DATE_TIME_IN_UTC));
        }

        [TestMethod]
        public void AfterThisWeekDateIsInCorrectFormat() {
            DateTime myPostedDateTimeInUtc = new DateTime(2011, 5, 10, UTC_HOUR, 3, 3);

            string myExpected = "05/10/2011 " + EXPECTED_HOUR + ":03 AM";

            Assert.AreEqual(myExpected, DateHelper.ToLocalTime(myPostedDateTimeInUtc, TODAYS_DATE_TIME_IN_UTC));
        }

        [TestMethod]
        public void FutureDateIsInCorrectFormat() {
            DateTime myPostedDateTimeInUtc = new DateTime(2011, 6, 11, UTC_HOUR, 3, 3);

            string myExpected = "06/11/2011 " + EXPECTED_HOUR + ":03 AM";

            Assert.AreEqual(myExpected, DateHelper.ToLocalTime(myPostedDateTimeInUtc, TODAYS_DATE_TIME_IN_UTC));
        }
    }
}
