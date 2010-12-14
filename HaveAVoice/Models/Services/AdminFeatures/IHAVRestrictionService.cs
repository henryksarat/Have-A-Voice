using System.Collections.Generic;
using HaveAVoice.Models.View;

namespace HaveAVoice.Models.Services.AdminFeatures {
    public interface IHAVRestrictionService {
        Restriction GetRestriction(int restrictionId);
        IEnumerable<Restriction> GetAllRestrictions();
        bool CreateRestriction(UserInformationModel aCreatedByUser, Restriction restrictionToCreate);
        bool EditRestriction(UserInformationModel anEditedByUser, Restriction restrictionToEdit);
        bool DeleteRestriction(UserInformationModel aDeletedByUser, Restriction restrictionDelete);
    }
}