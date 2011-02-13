﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Repositories.SiteFeatures;
using HaveAVoice.Validation;
using HaveAVoice.Services.Helpers;

namespace HaveAVoice.Services.SiteFeatures {
    public class HAVContactUsService : HAVBaseService, IHAVContactUsService {
        private IHAVContactUsRepository theContactUsRepo;
        private IValidationDictionary theValidationDictionary;

        public HAVContactUsService(IValidationDictionary validationDictionary)
            : this(validationDictionary, new HAVBaseRepository(), new EntityHAVContactUsRepository()) { }

        public HAVContactUsService(IValidationDictionary aValidationDictionary, IHAVBaseRepository aBaseRepository, IHAVContactUsRepository aContactUsRepo)
            : base(aBaseRepository) {
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
            if (!ValidationHelper.IsValidEmail(anEmail)) {
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