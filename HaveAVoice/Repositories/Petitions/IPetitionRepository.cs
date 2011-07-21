using System.Collections.Generic;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.Petitions {
    public interface IPetitionRepository {
        void AddSignatureToPetition(User aUserSigning, int aPetitionId, string aComment, string anAddress, string aCity, string aState, string aZip, string anEmail);
        Petition CreatePetition(User aUserCreating, string aTitle, string aDescription, string aCity, string aState, string aZip);
        IEnumerable<Petition> GetPetitions(User aUser);
        Petition GetPetition(User aUser, int aPetitionId);
        PetitionSignature GetPetitionSignature(User aUser, int aPetitionId);
        void SetPetitionAsInactive(User aUser, int aPetitionId);
    }
}
