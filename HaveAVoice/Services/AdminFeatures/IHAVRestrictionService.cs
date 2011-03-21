using System.Collections.Generic;
using HaveAVoice.Models.View;
using HaveAVoice.Models;
using Social.Generic.Models;

namespace HaveAVoice.Services.AdminFeatures {
    public interface IHAVRestrictionService {
        Restriction GetRestriction(int restrictionId);
        IEnumerable<Restriction> GetAllRestrictions();
        bool CreateRestriction(UserInformationModel<User> aCreatedByUser, Restriction restrictionToCreate);
        bool EditRestriction(UserInformationModel<User> anEditedByUser, Restriction restrictionToEdit);
        bool DeleteRestriction(UserInformationModel<User> aDeletedByUser, Restriction restrictionDelete);
    }
}