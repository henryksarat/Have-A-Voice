using System;
using System.Linq;
using UniversityOfMe.Models;
using System.Collections.Generic;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Repositories.Badges {
    public class EntityBadgeRepository : IBadgeRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public IEnumerable<Badge> GetBadgesForUser(User aUser) {
            return (from ub in theEntities.UserBadges
                    where ub.UserId == aUser.Id
                    && ub.Received
                    select ub.Badge);
        }
    }
}