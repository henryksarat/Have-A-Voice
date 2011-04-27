using System;
using System.Collections.Generic;
using System.Linq;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Dating {
    public class DatingRepository : IDatingRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void CreateDatingLog(int aSourceUserId, int anAskingUserId, bool aResponse) {
            DatingLog myDatingLog = DatingLog.CreateDatingLog(0, aSourceUserId, anAskingUserId, aResponse, false, DateTime.UtcNow);
            theEntities.AddToDatingLogs(myDatingLog);
            theEntities.SaveChanges();
        }

        public IEnumerable<DatingLog> GetYesDatingLogsUserHasBeenAskedAbout(User anAskingUser) {
            return (from d in theEntities.DatingLogs
                    where d.AskedUserId == anAskingUser.Id && d.WantTo == true
                    select d).ToList<DatingLog>();
        }

        public IEnumerable<DatingLog> GetYesDatingLogsUserHasNotSeenYet(User aSourceUser) {
            return (from d in theEntities.DatingLogs
                    where d.SourceUserId == aSourceUser.Id && d.SourceHasSeen == false && d.WantTo == true
                    select d).ToList<DatingLog>();
        }

        public IEnumerable<User> GetSourceUsersWhereSpecifiedUserIsTheAskingUser(User anAskingUser) {
            return (from d in theEntities.DatingLogs
                    where d.AskedUserId == anAskingUser.Id
                    select d.SourceUser).ToList<User>();
        }

        public DatingLog GetUserDatingLog(User aSourceUser, User anAskingUser) {
            return (from d in theEntities.DatingLogs
                    where d.SourceUserId == aSourceUser.Id && d.AskedUserId == anAskingUser.Id
                    select d).FirstOrDefault<DatingLog>();
        }

        public void MarkDatingLogAsSeenBySourceUser(User aSourceUser, int aDatingLogId) {
            DatingLog myDatingLog = GetDatingLog(aSourceUser, aDatingLogId);
            myDatingLog.SourceHasSeen = true;
            theEntities.ApplyCurrentValues(myDatingLog.EntityKey.EntitySetName, myDatingLog);
            theEntities.SaveChanges();
        }

        private DatingLog GetDatingLog(User aSourceUser, int aDatingLogId) {
            return (from d in theEntities.DatingLogs
                    where d.Id == aDatingLogId && d.SourceUserId == aSourceUser.Id
                    select d).FirstOrDefault<DatingLog>();
        }
    }
}