using System.Collections.Generic;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.Badges;
using Social.Generic.Models;

namespace UniversityOfMe.Services.Badges {
    public class BadgeService : IBadgeService {
        private IBadgeRepository theBadgeRepository;

        public BadgeService()
            : this(new EntityBadgeRepository()) { }

        public BadgeService(IBadgeRepository aBadgeRepo) {
            theBadgeRepository = aBadgeRepo;
        }

        public Badge GetBadgeByName(string aBadgeName) {
            return theBadgeRepository.GetBadgeByName(aBadgeName);
        }

        public IEnumerable<Badge> GetBadgesForUser(User aUser) {
            return theBadgeRepository.GetBadgesForUser(aUser);
        }

        public UserBadge GetLatestUnseenBadgeForUser(User aUser) {
            return theBadgeRepository.GetLatestUnseenBadgeForUser(aUser);
        }

        public void MarkBadgeAsSeen(UserInformationModel<User> aUserInfo, int aUserBadgeId) {
            theBadgeRepository.MarkUserBadgeAsSeen(aUserInfo.Details, aUserBadgeId);
        }
    }
}
