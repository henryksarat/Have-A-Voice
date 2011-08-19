using System.Collections.Generic;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.Badges;

namespace UniversityOfMe.Services.Badges {
    public class BadgeService : IBadgeService {
        private IBadgeRepository theBadgeRepository;

        public BadgeService()
            : this(new EntityBadgeRepository()) { }

        public BadgeService(IBadgeRepository aBadgeRepo) {
            theBadgeRepository = aBadgeRepo;
        }

        public IEnumerable<Badge> GetBadgesForUser(User aUser) {
            return theBadgeRepository.GetBadgesForUser(aUser);
        }
    }
}
