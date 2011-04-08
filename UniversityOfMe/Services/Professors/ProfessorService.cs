using System.Collections.Generic;
using Social.Generic.Constants;
using Social.Generic.Models;
using Social.Validation;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories;
using UniversityOfMe.Repositories.Professors;
using UniversityOfMe.Services;
using UniversityOfMe.Services.Professors;
using UniversityOfMe.Helpers;

namespace HaveAVoice.Services.Issues {
    public class ProfessorService : IProfessorService {
        private IValidationDictionary theValidationDictionary;
        private IProfessorRepository theProfessorRepository;
        private IUniversityService theUniversityService;

        public ProfessorService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityProfessorRepository(), new UniversityService(new EntityUniversityRepository())) { }

        public ProfessorService(IValidationDictionary aValidationDictionary, IProfessorRepository aProfessorRepo, IUniversityService aUniversityService) {
            theValidationDictionary = aValidationDictionary;
            theProfessorRepository = aProfessorRepo;
            theUniversityService = aUniversityService;
        }

        public Professor GetProfessor(int aProfessorId) {
            return theProfessorRepository.GetProfessor(aProfessorId);
        }

        public IEnumerable<Professor> GetProfessorsForUniversity(string aUniversityId) {
            return theProfessorRepository.GetProfessorsByUniversity(aUniversityId);
        }

        public bool CreateProfessor(UserInformationModel<User> aCreatingUser, Professor aProfessor) {
            if (!ValidateProfessor(aProfessor)) {
                return false;
            }

            theProfessorRepository.CreateProfessor(aCreatingUser.Details, aProfessor);
            return true;
        }

        public bool IsProfessorExists(string aUniversityId, string aFullname) {
            string[] mySplitName = URLHelper.FromUrlFriendly(aFullname);

            if (mySplitName.Length != 2) {
                return false;
            }
            string myFirstname = mySplitName[0];
            string myLastname = mySplitName[1];

            Professor myProfessor = theProfessorRepository.GetProfessor(aUniversityId, myFirstname, myLastname);

            return myProfessor == null ? false : true;
        }

        private bool ValidateProfessor(Professor aProfessor) {
            if (string.IsNullOrEmpty(aProfessor.FirstName)) {
                theValidationDictionary.AddError("FirstName", aProfessor.FirstName, "First name is required.");
            }

            if (string.IsNullOrEmpty(aProfessor.LastName)) {
                theValidationDictionary.AddError("LastName", aProfessor.LastName, "Last name is required.");
            }

            if (string.IsNullOrEmpty(aProfessor.UniversityId) || aProfessor.UniversityId.Equals(Constants.SELECT)) {
                theValidationDictionary.AddError("UniversityId", aProfessor.UniversityId, "A university is required.");
            }

            return theValidationDictionary.isValid;
        }
    }
}
