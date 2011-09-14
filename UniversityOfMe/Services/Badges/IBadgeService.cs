using System.Collections.Generic;
using UniversityOfMe.Models;
using Social.Generic.Models;

namespace UniversityOfMe.Services.Badges {
    public interface IBadgeService {
        Badge GetBadgeByName(string aBadgeName);
        IEnumerable<Badge> GetBadgesForUser(User aUser);
        UserBadge GetLatestUnseenBadgeForUser(User aUser);
        void MarkBadgeAsSeen(UserInformationModel<User> aUserInfo, int aUserBadgeId);
    }
}