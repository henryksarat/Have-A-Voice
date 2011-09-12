using UniversityOfMe.Models;
using System.Collections;
using System.Collections.Generic;

namespace UniversityOfMe.Repositories.Dating {
    public interface IFlirtingRepository {
        void CreateFlirt(User aUserPostingFlirt, string aUniversityId, string anAdjective, string aDeliciousTreat, 
            string anAnimal, string aHairColor, string aGender, string aMessage, int aTaggedUserId,
            string aWhere);
        IEnumerable<AnonymousFlirt> GetAnonymousFlirtsWithinUniversity(string aUniversityId);
    }
}
