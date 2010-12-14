using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Helpers.UserInformation {
    public class RestrictionLog {
        public List<DateTime> TimeLog;

        private RestrictionLog() {
            TimeLog = new List<DateTime>();
        }

        public static RestrictionLog Create() {
            return new RestrictionLog();
        }

        public void AddLog(DateTime dateTimeStamp, long purgeAfterSeconds) {
            FilterLog(purgeAfterSeconds);
            TimeLog.Add(dateTimeStamp);
        }

        public void FilterLog(long purgeAfterSeconds) {
            DateTime latestDateToKeep = DateTime.UtcNow;
            latestDateToKeep = latestDateToKeep.AddSeconds(purgeAfterSeconds * -1);
            int keepEntriesUntilHere = 0;
            for (int i = TimeLog.Count-1; i >= 0; i--) {
                if (isBefore(latestDateToKeep, i)) {
                    keepEntriesUntilHere = i + 1;
                    break;
                }
            }
            TimeLog.RemoveRange(0, keepEntriesUntilHere);
        }

        private bool isBefore(DateTime latestDateToKeep, int i) {
            return TimeLog[i].CompareTo(latestDateToKeep) < 0;
        }
    }
}