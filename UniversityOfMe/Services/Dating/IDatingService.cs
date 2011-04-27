using System.Collections.Generic;
using Social.User.Services;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;

namespace UniversityOfMe.Services.Dating {
    public interface IDatingService {
        void AddDatingResult(int aSourceUserId, int anAskingUserId, bool aResponse);
        User GetDatingMember(User anAskingUser);
        void MarkDatingLogAsSeenBySourceUser(User aSourceUser, int aDatingLogId);
        DatingLog UserDatingMatch(User anAskingUser);
    }
}
