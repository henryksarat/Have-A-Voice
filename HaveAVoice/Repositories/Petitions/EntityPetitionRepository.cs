using System;
using System.Collections.Generic;
using System.Linq;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.Petitions {
    public class EntityPetitionRepository : IPetitionRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public void AddSignatureToPetition(User aUserSigning, int aPetitionId, string aComment, string anAddress, string aCity, string aState, string aZip, string anEmail) {
            PetitionSignature myPetitionSignature = PetitionSignature.CreatePetitionSignature(0, aPetitionId, aUserSigning.Id, anAddress, aCity, aState, aZip, DateTime.UtcNow);
            if(!string.IsNullOrEmpty(anEmail)) {
                myPetitionSignature.Email = anEmail;
            }
            if (!string.IsNullOrEmpty(aComment)) {
                myPetitionSignature.Comment = aComment;
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
                    select p).OrderBy(p => p.Title);
        }

        public Petition GetPetition(int aPetitionId) {
            return (from p in theEntities.Petitions
                    where p.Id == aPetitionId
                    select p).FirstOrDefault<Petition>();
        }

        public PetitionSignature GetPetitionSignature(User aUser, int aPetitionId) {
            return (from s in theEntities.PetitionSignatures
                    where s.PetitionId == aPetitionId
                    && s.UserId == aUser.Id
                    select s).FirstOrDefault<PetitionSignature>();
        }

        public void SetPetitionAsInactive(User aUser, int aPetitionId) {
            Petition myPetition = GetPetition(aPetitionId);
            myPetition.Active = false;
            myPetition.DeactivatedByUserId = aUser.Id;
            myPetition.DeactivatedDateTimeStamp = DateTime.UtcNow;
            theEntities.SaveChanges();
        }
    }
}