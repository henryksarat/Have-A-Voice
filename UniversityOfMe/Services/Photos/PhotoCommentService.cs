using Social.Validation;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.Photos;

namespace UniversityOfMe.Services.Photos {
    public class PhotoCommentService : IPhotoCommentService {
        private IValidationDictionary theValidationDictionary;
        private IUofMePhotoRepository thePhotoRepo;

        public PhotoCommentService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityPhotoRepository()) { }

        public PhotoCommentService(IValidationDictionary aValidationDictionary, IUofMePhotoRepository aPhotoRepo) {
            theValidationDictionary = aValidationDictionary;
            thePhotoRepo = aPhotoRepo;
        }

        public bool AddCommentToPhoto(User aCommentingUser, int aPhotoId, string aComment) {
            if (!IsValidComment(aComment)) {
                return false;
            }
            thePhotoRepo.AddPhotoComment(aCommentingUser, aPhotoId, aComment);
            return true;
        }

        private bool IsValidComment(string aComment) {
            if (string.IsNullOrEmpty(aComment)) {
                theValidationDictionary.AddError("Comment", aComment, "A comment is required.");
            }

            return theValidationDictionary.isValid;
        }
    }
}