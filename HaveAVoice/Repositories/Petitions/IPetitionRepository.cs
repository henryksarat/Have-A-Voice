using System.Collections.Generic;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.Petitions {
    public interface IPetitionRepository {
        void AddSignatureToPetition(User aUserSigning, string anAlias, int aPetitionId, string aComment, string anAddress, string aCity, string aState, string aZip, string aPhoneNumber);
        Petition CreatePetition(User aUserCreating, string aTitle, string aDescription, string aCity, string aState, string aZip);
        IEnumerable<Petition> GetPetitions();
        Petition GetPetition(int aPetitionId);
        void SetPetitionAsInactive(User aUser, int aPetitionId);
    }
}
