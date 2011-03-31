using Social.Site.Repositories;
using Social.Validation;

namespace Social.Site.Services {
    public class ContactUsService : IContactUsService {
        private IContactUsRepository theContactUsRepo;
        private IValidationDictionary theValidationDictionary;

        public ContactUsService(IValidationDictionary aValidationDictionary, IContactUsRepository aContactUsRepo) {
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