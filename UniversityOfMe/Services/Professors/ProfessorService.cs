using System.Collections.Generic;
using Social.Generic.Constants;
using Social.Generic.Exceptions;
using Social.Generic.Models;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.Professors;

namespace UniversityOfMe.Services.Professors {
    public class ProfessorService : IProfessorService {
        private IValidationDictionary theValidationDictionary;
        private IProfessorRepository theProfessorRepository;

        public ProfessorService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityProfessorRepository()) { }

        public ProfessorService(IValidationDictionary aValidationDictionary, IProfessorRepository aProfessorRepo) {
            theValidationDictionary = aValidationDictionary;
            theProfessorRepository = aProfessorRepo;
        }

        public Professor GetProfessor(int aProfessorId) {
            return theProfessorRepository.GetProfessor(aProfessorId);
        }

        public Professor GetProfessor(UserInformationModel<User> aViewingUser, string aUniversityId, string aProfessorName) {
            bool myIsFromUniversity = UniversityHelper.IsFromUniversity(aViewingUser.Details, aUniversityId);

            string[] mySplitName = URLHelper.FromUrlFriendly(aProfessorName);

            if (mySplitName.Length != 2) {
                throw new CustomException("The professors name doesn't have a first and last name.");
            }
            string myFirstname = mySplitName[0];
            string myLastname = mySplitName[1];

            if (myIsFromUniversity) {
                return theProfessorRepository.GetProfessor(aUniversityId, myFirstname, myLastname);
            }
            throw new CustomException(UOMErrorKeys.NOT_FROM_UNVIERSITY);
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
