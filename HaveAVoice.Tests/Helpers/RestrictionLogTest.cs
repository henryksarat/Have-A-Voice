using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.UserInformation;

namespace HaveAVoice.Tests.Helpers {
    [TestClass]
    public class RestrictionLogTest {
        private RestrictionLog theRestrictionLog;

        [TestInitialize]
        public void Initialize() {
            theRestrictionLog = RestrictionLog.Create();
        }

        [TestMethod]
        public void TestAddingWithNoPurgeSeconds() {
            long purgeAfterXSeconds = 2;
            theRestrictionLog.AddLog(DateTime.UtcNow, purgeAfterXSeconds);
            theRestrictionLog.AddLog(DateTime.UtcNow, purgeAfterXSeconds);
            theRestrictionLog.AddLog(DateTime.UtcNow, purgeAfterXSeconds);

            Assert.AreEqual(3, theRestrictionLog.TimeLog.Count);
        }

        [TestMethod]
        public void TestAddingWithPurgeSeconds() {
            long purgeAfterXSeconds = 300;
            theRestrictionLog.AddLog(DateTime.UtcNow.AddSeconds(-299), purgeAfterXSeconds);
            theRestrictionLog.AddLog(DateTime.UtcNow.AddSeconds(-30), purgeAfterXSeconds);
            theRestrictionLog.AddLog(DateTime.UtcNow.AddSeconds(-3), purgeAfterXSeconds);

            Assert.AreEqual(3, theRestrictionLog.TimeLog.Count);

            purgeAfterXSeconds = 25;
            theRestrictionLog.AddLog(DateTime.UtcNow, purgeAfterXSeconds);

            Assert.AreEqual(2, theRestrictionLog.TimeLog.Count);
        }

        [TestMethod]
        public void TestFilterLog() {
            long purgeAfterXSeconds = 300;
            DateTime stationaryDateTime = DateTime.UtcNow;
            theRestrictionLog.AddLog(stationaryDateTime.AddSeconds(-300), purgeAfterXSeconds);
            theRestrictionLog.AddLog(stationaryDateTime.AddSeconds(-30), purgeAfterXSeconds);
            theRestrictionLog.AddLog(stationaryDateTime.AddSeconds(-3), purgeAfterXSeconds);

            Assert.AreEqual(3, theRestrictionLog.TimeLog.Count);

            theRestrictionLog.FilterLog(299);

            Assert.AreEqual(2, theRestrictionLog.TimeLog.Count);
        }
    }
}
