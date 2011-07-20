using System.Collections.Generic;
using HaveAVoice.Models;
using Social.Generic.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.Search;

namespace HaveAVoice.Services.Petitions {
    public interface IPetitionService {
        Petition CreatePetition(UserInformationModel<User> aUserInformation, CreatePetitionModel aCreatePetitionModel);
        bool SignPetition(UserInformationModel<User> aUserInformation, CreatePetitionSignatureModel aCreatePetitionSignatureModel);
        void SetPetitionAsInactive(UserInformationModel<User> aUserInformation, int aPetitionId);
        IEnumerable<Petition> GetPetitions();
        Petition GetPetition(int aPetitionId);

    }
}