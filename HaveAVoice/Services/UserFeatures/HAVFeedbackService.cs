using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models.Validation;
using HaveAVoice.Models.Repositories.UserFeatures;
using HaveAVoice.Models.Repositories;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVFeedbackService : HAVBaseService, IHAVFeedbackService {
        private IValidationDictionary theValidationDictionary;
        private IHAVFeedbackRepository theRepository;

        public HAVFeedbackService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityHAVFeedbackRepository(), new HAVBaseRepository()) { }

        public HAVFeedbackService(IValidationDictionary aValidationDictionary, IHAVFeedbackRepository aRepository, 
                                  IHAVBaseRepository baseRepository) : base(baseRepository) {
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