using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Badges {
    public interface IBadgeRepository {
        Badge GetBadgeByName(string aBadgeName);
        IEnumerable<Badge> GetBadgesForUser(User aUser);
        UserBadge GetLatestUnseenBadgeForUser(User aUser);
        void MarkUserBadgeAsSeen(User aUser, int aUserBadgeId);
    }
}
