using System.Collections.Generic;
using UniversityOfMe.Models;

namespace UniversityOfMe.Services.Badges {
    public interface IBadgeService {
        IEnumerable<Badge> GetBadgesForUser(User aUser);
    }
}