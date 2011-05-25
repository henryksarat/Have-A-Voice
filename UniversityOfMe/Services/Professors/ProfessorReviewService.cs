using Social.Generic.Models;
using Social.Validation;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.Professors;

namespace UniversityOfMe.Services.Professors {
    public class ProfessorReviewService : IProfessorReviewService {
        private IValidationDictionary theValidationDictionary;
        private IProfessorReviewRepository theProfessorReviewRepository;

        public ProfessorReviewService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityProfessorReviewRepository()) { }

        public ProfessorReviewService(IValidationDictionary aValidationDictionary, IProfessorReviewRepository aProfessorReviewRepo) {
            theValidationDictionary = aValidationDictionary;
            theProfessorReviewRepository = aProfessorReviewRepo;
        }

        public bool CreateProfessorReview(UserInformationModel<User> aReviewingUser, ProfessorReview aProfessorReview) {
            if (!ValidateProfessorReview(aProfessorReview)) {
                return false;
            }

            theProfessorReviewRepository.CreateProfessorReview(aReviewingUser.Details, aProfessorReview);

            return true;
        }

        private bool ValidateProfessorReview(ProfessorReview aProfessorReview) {
            if (string.IsNullOrEmpty(aProfessorReview.Review)) {
                theValidationDictionary.AddError("Review", aProfessorReview.Review, "Review is required.");
            }

            if (string.IsNullOrEmpty(aProfessorReview.Class)) {
                theValidationDictionary.AddError("Class", aProfessorReview.Class, "The class in which you had this professor with is required.");
            }

            if (!RangeValidation.IsWithinRange(aProfessorReview.Rating, 1, 5)) {
                theValidationDictionary.AddError("Rating", aProfessorReview.Rating.ToString(), "Incorrect rating, it must be between 1 and 5.");
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
