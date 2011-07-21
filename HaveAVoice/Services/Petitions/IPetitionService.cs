using System.Collections.Generic;
using HaveAVoice.Models;
using Social.Generic.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.Search;

namespace HaveAVoice.Services.Petitions {
    public interface IPetitionService {
        bool CanView(UserInformationModel<User> aUser, Petition aPetition);
        Petition CreatePetition(UserInformationModel<User> aUserInformation, CreatePetitionModel aCreatePetitionModel);
        bool SignPetition(UserInformationModel<User> aUserInformation, CreatePetitionSignatureModel aCreatePetitionSignatureModel);
        bool SetPetitionAsInactive(UserInformationModel<User> aUserInformation, int aPetitionId);
        IEnumerable<Petition> GetPetitions(UserInformationModel<User> aUser);
        DisplayPetitionModel GetPetition(UserInformationModel<User> aUser, int aPetitionId);
        bool HasSignedPetition(UserInformationModel<User> aUser, int aPetitionId);
    }
}