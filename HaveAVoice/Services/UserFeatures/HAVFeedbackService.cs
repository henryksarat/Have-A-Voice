using System.Collections.Generic;
using HaveAVoice.Models;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using Social.Validation;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVFeedbackService : IHAVFeedbackService {
        private IValidationDictionary theValidationDictionary;
        private IHAVFeedbackRepository theRepository;

        public HAVFeedbackService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityHAVFeedbackRepository()) { }

        public HAVFeedbackService(IValidationDictionary aValidationDictionary, IHAVFeedbackRepository aRepository) {
            theValidationDictionary = aValidationDictionary;
            theRepository = aRepository;
        }
        
        public bool  AddFeedback(User aUser, string aFeedback) {
            if (!ValidFeedback(aFeedback)) {
                return false;
            }
            theRepository.AddFeedback(aUser, aFeedback);
            return true;
        }

        public IEnumerable<Feedback> GetAllFeedback() {
            return theRepository.GetAllFeedback();
        }

        private bool ValidFeedback(string aFeedback) {
            if (string.IsNullOrEmpty(aFeedback)) {
                theValidationDictionary.AddError("Feedback", aFeedback, "You must provide feedback.");
            }

            return theValidationDictionary.isValid;
        }
    }
}