using System.Web.Mvc;
using HaveAVoice.Models;
using HaveAVoice.Services.Petitions;
using Social.Generic.Models;
using Social.Validation;

namespace HaveAVoice.Helpers.Petitions {
    public static class PetitionHelper {
        public static bool HasSigned(UserInformationModel<User> aUser, int aPetitionId) {
            IPetitionService myPetitionService = new PetitionService(new ModelStateWrapper(new ModelStateDictionary()));
            return myPetitionService.HasSignedPetition(aUser, aPetitionId);
        }

        public static bool IsOwner(UserInformationModel<User> aUser, Petition aPetition) {
            IPetitionService myPetitionService = new PetitionService(new ModelStateWrapper(new ModelStateDictionary()));
            return myPetitionService.CanView(aUser, aPetition);
        }
    }
}