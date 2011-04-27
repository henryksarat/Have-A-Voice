using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Dating {
    public interface IDatingRepository {
        void CreateDatingLog(int aSourceUserId, int anAskingUserId, bool aResponse);
        IEnumerable<DatingLog> GetYesDatingLogsUserHasBeenAskedAbout(User anAskingUser);
        IEnumerable<DatingLog> GetYesDatingLogsUserHasNotSeenYet(User aSourceUser);
        IEnumerable<User> GetSourceUsersWhereSpecifiedUserIsTheAskingUser(User anAskingUser);
        DatingLog GetUserDatingLog(User aSourceUser, User anAskingUser);
        void MarkDatingLogAsSeenBySourceUser(User aSourceUser, int aDatingLog);
    }
}
