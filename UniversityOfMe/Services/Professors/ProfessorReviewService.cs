using System.Collections.Generic;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.Professors;
using UniversityOfMe.Services.Professors;
using Social.Validation;
using Social.Generic.Models;
using UniversityOfMe.Services;
using UniversityOfMe.Repositories;
using Social.Generic.Exceptions;
using UniversityOfMe.Helpers;

namespace HaveAVoice.Services.Issues {
    public class ProfessorReviewService : IProfessorReviewService {
        private IValidationDictionary theValidationDictionary;
        private IProfessorReviewRepository theProfessorReviewRepository;
        private IUniversityService theUniversityService;

        public ProfessorReviewService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityProfessorReviewRepository(), new UniversityService(new EntityUniversityRepository())) { }

        public ProfessorReviewService(IValidationDictionary aValidationDictionary, IProfessorReviewRepository aProfessorReviewRepo, IUniversityService aUniversityService) {
            theValidationDictionary = aValidationDictionary;
            theProfessorReviewRepository = aProfessorReviewRepo;
            theUniversityService = aUniversityService;
        }

        public bool CreateProfessorReview(UserInformationModel<User> aReviewingUser, ProfessorReview aProfessorReview) {
            if (!ValidateProfessorReview(aProfessorReview)) {
                return false;
            }

            theProfessorReviewRepository.CreateProfessorReview(aReviewingUser.Details, aProfessorReview);

            return true;
        }

        public IEnumerable<ProfessorReview> GetProfessorReviews(UserInformationModel<User> aViewingUser, string aUniversityId, string aProfessorName) {
            bool myIsFromUniversity = theUniversityService.IsFromUniversity(aViewingUser.Details, aUniversityId);

            string myFullName = URLHelper.FromUrlFriendly(aProfessorName);
            string[] mySplitName = myFullName.Split(' ');

            if (mySplitName.Length != 2) {
                throw new CustomException("The professors name doesn't have a first and last name.");
            }
            string myFirstname = mySplitName[0];
            string myLastname = mySplitName[1];

            if (myIsFromUniversity) {
                return theProfessorReviewRepository.GetProfessorReviewsByUnversityAndName(aUniversityId, myFirstname, myLastname);
            }
            throw new CustomException(UOMErrorKeys.NOT_FROM_UNVIERSITY);
        }

        private bool ValidateProfessorReview(ProfessorReview aProfessorReview) {
            if (string.IsNullOrEmpty(aProfessorReview.Review)) {
                theValidationDictionary.AddError("Review", aProfessorReview.Review, "Review is required.");
            }

            if (string.IsNullOrEmpty(aProfessorReview.Class)) {
                theValidationDictionary.AddError("Class", aProfessorReview.Class, "The class in which you had this professor with is required.");
            }

            if (!RangeValidation.IsWithinRange(aProfessorReview.Rating, 0, 5)) {
                theValidationDictionary.AddError("Rating", aProfessorReview.Rating.ToString(), "Incorrect rating, it must be between 0 and 5.");
            }

            if (!DropDownItemValidation.IsValid(aProfessorReview.Year.ToString())) {
                theValidationDictionary.AddError("Year", aProfessorReview.Year.ToString(), "The year you had this professor is required.");
            }

            if (!DropDownItemValidation.IsValid(aProfessorReview.AcademicTermId)) {
                theValidationDictionary.AddError("AcademicTermId", aProfessorReview.AcademicTermId, "An academic term when you had this professor is required.");
            }

            return theValidationDictionary.isValid;
        }
    }
}
