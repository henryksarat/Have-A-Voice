using System.Collections.Generic;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.GeneralPostings {
    public interface IGeneralPostingRepository {
        GeneralPosting CreateGeneralPosting(User aCreatedByUser, string aUniversityId, string aTitle, string aBody);
        void CreateGeneralPostingReply(User aPostedByUser, int aGeneralPostingId, string aReply);
        GeneralPosting GetGeneralPosting(int aGeneralPostingId);
        IEnumerable<GeneralPosting> GetGeneralPostingsForUniversity(string aUniversityId);
        void MarkGeneralPostingAsView(User aUser, int aGeneralPostingId);
        void Subscribe(User aUser, int aGeneralPostingId);
        void Unsubscribe(User aUser, int aGeneralPostingId);
    }
}
