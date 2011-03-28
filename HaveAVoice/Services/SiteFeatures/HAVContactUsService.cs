using HaveAVoice.Repositories;
using HaveAVoice.Repositories.SiteFeatures;
using HaveAVoice.Services.Helpers;
using Social.Validation;
using HaveAVoice.Models;
using Social.Generic.Repositories;

namespace HaveAVoice.Services.SiteFeatures {
    public class HAVContactUsService : IHAVContactUsService {
        private IHAVContactUsRepository theContactUsRepo;
        private IValidationDictionary theValidationDictionary;

        public HAVContactUsService(IValidationDictionary validationDictionary)
            : this(validationDictionary, new EntityHAVContactUsRepository()) { }

        public HAVContactUsService(IValidationDictionary aValidationDictionary, IHAVContactUsRepository aContactUsRepo) {
                theContactUsRepo = aContactUsRepo;
                theValidationDictionary = aValidationDictionary;
        }

        public bool AddContactUsInquiry(string anEmail, string anInquiryType, string anInquiry) {
            if (!ValidContactUs(anEmail, anInquiryType, anInquiry)) {
                return false;
            }
            theContactUsRepo.AddContactUserInquiry(anEmail, anInquiryType, anInquiry);

            return true;
        }

        private bool ValidContactUs(string anEmail, string anInquiryType, string anInquiry) {
            if (!EmailValidation.IsValidEmail(anEmail)) {
                theValidationDictionary.AddError("Email", anEmail, "Email is not valid.");
            }
            if (anInquiryType.ToUpper().Equals("SELECT")) {
                theValidationDictionary.AddError("InquiryType", anInquiryType, "Inquiry type is required.");
            }
            if (anInquiry.Trim().Length == 0) {
                theValidationDictionary.AddError("Inquiry", anInquiry, "Inquiry is required.");
            }

            return theValidationDictionary.isValid;
        }
    }
}