﻿using HaveAVoice.Models.Validation;
using System.Collections.Generic;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.AdminFeatures;
using HaveAVoice.Models;

namespace HaveAVoice.Services.AdminFeatures {
    public class HAVErrorService : HAVBaseService, IHAVErrorService {
        private IValidationDictionary theValidationDictionary;
        private IHAVErrorRepository theRepository;

        public HAVErrorService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityHAVErrorRepository(), new HAVBaseRepository()) { }

        public HAVErrorService(IValidationDictionary aValidationDictionary, IHAVErrorRepository aRepository,
                                           IHAVBaseRepository baseRepository) : base(baseRepository) {
            theValidationDictionary = aValidationDictionary;
            theRepository = aRepository;
        }

        public IEnumerable<ErrorLog> GetAllErrors() {
            return theRepository.GetAllErrors();
        }
    }
}