using System;
using System.Collections.Generic;
using System.Linq;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.GeneralPostings;

namespace UniversityOfMe.Repositories.Classes {
    public class EntityGeneralPostingRepository : IGeneralPostingRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public GeneralPosting CreateGeneralPosting(User aCreatedByUser, string aUniversityId, string aTitle, string aBody) {
            GeneralPosting myGeneralPosting = GeneralPosting.CreateGeneralPosting(0, aCreatedByUser.Id, aUniversityId, aTitle, aBody, DateTime.UtcNow);
            theEntities.AddToGeneralPostings(myGeneralPosting);
            theEntities.SaveChanges();
            return myGeneralPosting;
        }

        public void CreateGeneralPostingReply(User aPostedByUser, int aGeneralPostingId, string aReply) {
            GeneralPostingReply myGeneralPosting = GeneralPostingReply.CreateGeneralPostingReply(0, aGeneralPostingId, aPostedByUser.Id, aReply, DateTime.UtcNow);
            theEntities.AddToGeneralPostingReplies(myGeneralPosting);
            theEntities.SaveChanges();
        }

        public GeneralPosting GetGeneralPosting(int aGeneralPostingId) {
            return (from g in theEntities.GeneralPostings
                    where g.Id == aGeneralPostingId
                    select g).FirstOrDefault<GeneralPosting>();
        }

        public IEnumerable<GeneralPosting> GetGeneralPostingsForUniversity(string aUniversityId) {
            return (from g in theEntities.GeneralPostings
                    where g.UniversityId == aUniversityId
                    select g).ToList<GeneralPosting>();
        }
    }
}