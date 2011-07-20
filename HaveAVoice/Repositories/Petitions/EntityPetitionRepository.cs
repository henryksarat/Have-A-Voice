using System;
using System.Collections.Generic;
using System.Linq;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.Petitions {
    public class EntityPetitionRepository : IPetitionRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public void AddSignatureToPetition(User aUserSigning, int aPetitionId, string anAlias, string aComment, string anAddress, string aCity, string aState, string aZip, string aPhoneNumber) {
            PetitionSignature myPetitionSignature = PetitionSignature.CreatePetitionSignature(0, aPetitionId, aUserSigning.Id, anAlias, aComment, anAddress, aCity, aState, aZip, DateTime.UtcNow);
            if(string.IsNullOrEmpty(aPhoneNumber)) {
                myPetitionSignature.PhoneNumber = aPhoneNumber;
            }
            theEntities.AddToPetitionSignatures(myPetitionSignature);
            theEntities.SaveChanges();
        }

        public Petition CreatePetition(User aUserCreating, string aTitle, string aBody, string aCity, string aState, string aZip) {
            Petition myPetition = Petition.CreatePetition(0, aUserCreating.Id, aTitle, aBody, aCity, aState, aZip, DateTime.UtcNow, true);
            theEntities.AddToPetitions(myPetition);
            theEntities.SaveChanges();
            return myPetition;
        }

        public IEnumerable<Petition> GetPetitions() {
            return (from p in theEntities.Petitions
                    where p.Active
                    select p);
        }

        public Petition GetPetition(int aPetitionId) {
            return (from p in theEntities.Petitions
                    where p.Active
                    && p.Id == aPetitionId
                    select p).FirstOrDefault<Petition>();
        }

        public void SetPetitionAsInactive(User aUser, int aPetitionId) {
            Petition myPetition = GetPetition(aPetitionId);
            myPetition.Active = false;
            myPetition.DeactivatedByUserId = aUser.Id;
            myPetition.DeactivatedDateTimeStamp = DateTime.UtcNow;
        }
    }
}