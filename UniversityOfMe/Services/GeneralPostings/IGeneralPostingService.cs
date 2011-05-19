﻿using System.Collections.Generic;
using Social.Generic.Models;
using UniversityOfMe.Models;

namespace UniversityOfMe.Services.GeneralPostings {
    public interface IGeneralPostingService {
        GeneralPosting CreateGeneralPosting(UserInformationModel<User> aCreatedByUser, string aUniversityId, string aTitle, string aBody);
        bool CreateGeneralPostingReply(UserInformationModel<User> aPostedByUser, int aGeneralPostingId, string aReply);
        GeneralPosting GetGeneralPosting(int aGeneralPostingId);
        IEnumerable<GeneralPosting> GetGeneralPostingsForUniversity(string aUniversityId);
    }
}