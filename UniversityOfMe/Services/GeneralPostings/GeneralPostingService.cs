using System.Collections.Generic;
using Social.Generic.Constants;
using Social.Generic.Models;
using Social.Validation;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.Classes;
using UniversityOfMe.Repositories.GeneralPostings;
using UniversityOfMe.Services.GeneralPostings;

namespace UniversityOfMe.Services.GeneralPostings {
    public class GeneralPostingService : IGeneralPostingService {
        private IValidationDictionary theValidationDictionary;
        private IGeneralPostingRepository theGeneralPostingRepository;

        public GeneralPostingService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityGeneralPostingRepository()) { }

        public GeneralPostingService(IValidationDictionary aValidationDictionary, IGeneralPostingRepository aGeneralPostingRepo) {
            theValidationDictionary = aValidationDictionary;
            theGeneralPostingRepository = aGeneralPostingRepo;
        }

        public GeneralPosting CreateGeneralPosting(UserInformationModel<User> aCreatedByUser, string aUniversityId, string aTitle, string aBody) {
            if (!ValidGeneralPosting(aUniversityId, aTitle, aBody)) {
                return null;
            }

            return theGeneralPostingRepository.CreateGeneralPosting(aCreatedByUser.Details, aUniversityId, aTitle, aBody);
        }

        public bool CreateGeneralPostingReply(UserInformationModel<User> aPostedByUser, int aGeneralPostingId, string aReply) {
            if (!ValidGeneralPostingReply(aReply)) {
                return false;
            }

            theGeneralPostingRepository.CreateGeneralPostingReply(aPostedByUser.Details, aGeneralPostingId, aReply);
            return true;
        }

        public GeneralPosting GetGeneralPosting(int aGeneralPostingId) {
            return theGeneralPostingRepository.GetGeneralPosting(aGeneralPostingId);
        }

        public IEnumerable<GeneralPosting> GetGeneralPostingsForUniversity(string aUniversityId) {
            return theGeneralPostingRepository.GetGeneralPostingsForUniversity(aUniversityId);
        }

        private bool ValidGeneralPosting(string aUniversity, string aTitle, string aBody) {
            if (string.IsNullOrEmpty(aUniversity) || aUniversity.Equals(Constants.SELECT)) {
                theValidationDictionary.AddError("UniversityId", aUniversity, "A university is required.");
            }

            if (string.IsNullOrEmpty(aTitle)) {
                theValidationDictionary.AddError("Title", aTitle, "A title is required.");
            }

            if (string.IsNullOrEmpty(aBody)) {
                theValidationDictionary.AddError("Body", aBody, "A body is required for the posting.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidGeneralPostingReply(string aReply) {
            if (string.IsNullOrEmpty(aReply)) {
                theValidationDictionary.AddError("Reply", aReply, "A reply is required.");
            }

            return theValidationDictionary.isValid;
        }
    }
}
