﻿using System.Collections.Generic;
using HaveAVoice.Models.Validation;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.Repositories;
using HaveAVoice.Models.View;
using HaveAVoice.Models.Repositories.UserFeatures;
using System;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.Services.UserFeatures {
    public class HAVCalendarService : HAVBaseService, IHAVCalendarService {
        private IValidationDictionary theValidationDictionary;
        private IHAVCalendarRepository theRepository;

        public HAVCalendarService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityHAVCalendarRepository(), new HAVBaseRepository()) { }

        public HAVCalendarService(IValidationDictionary aValidationDictionary, IHAVCalendarRepository aRepository,
                               IHAVBaseRepository baseRepository)
            : base(baseRepository) {
            theValidationDictionary = aValidationDictionary;
            theRepository = aRepository;
        }

        private bool ValidateEvent(DateTime aDate, string anEventInformation) {
            if (anEventInformation.Trim().Length == 0) {
                theValidationDictionary.AddError("Information", anEventInformation.Trim(), "You must give some information on the event.");
            }

            if (aDate == null || aDate <= DateTime.UtcNow) {
                theValidationDictionary.AddError("Date", aDate.ToString(), "Invalid date.");
            }
            return theValidationDictionary.isValid;
        }

       
        public bool AddEvent(int aUserId, DateTime aDate, string anInformation) {
            if (!ValidateEvent(aDate, anInformation)) {
                return false;
            }

            theRepository.AddEvent(aUserId, aDate, anInformation);
            return true;
        }

        public void DeleteEvent(UserInformationModel anUserInformation, int anEventId) {
            bool myAdminOverride = HAVPermissionHelper.AllowedToPerformAction(anUserInformation, Helpers.HAVPermission.Delete_Any_Event);
            theRepository.DeleteEvent(anUserInformation.Details, anEventId, myAdminOverride);
        }

        public IEnumerable<Event> GetEventsForUser(int aUserId) {
            return theRepository.FindEvents(aUserId);
        }
    }
}
