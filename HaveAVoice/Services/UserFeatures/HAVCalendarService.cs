﻿using System.Collections.Generic;
using HaveAVoice.Validation;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Repositories;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories.UserFeatures;
using System;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Models;
using HaveAVoice.Helpers;
using HaveAVoice.Exceptions;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVCalendarService : HAVBaseService, IHAVCalendarService {
        private IValidationDictionary theValidationDictionary;
        private IHAVFriendService theFriendService;
        private IHAVPhotoService thePhotoService;
        private IHAVCalendarRepository theRepository;

        public HAVCalendarService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new HAVFriendService(), new HAVPhotoService(), new EntityHAVCalendarRepository(), new HAVBaseRepository()) { }

        public HAVCalendarService(IValidationDictionary aValidationDictionary, IHAVFriendService aFriendService, IHAVPhotoService aPhotoService, 
                                  IHAVCalendarRepository aRepository, IHAVBaseRepository baseRepository)
            : base(baseRepository) {
            theValidationDictionary = aValidationDictionary;
            thePhotoService = aPhotoService;
            theFriendService = aFriendService;
            theRepository = aRepository;
        }

        private bool ValidateEvent(DateTime aDate, string anEventInformation) {
            if (anEventInformation.Trim().Length == 0) {
                theValidationDictionary.AddError("Information", anEventInformation.Trim(), "You must give some information on the event.");
            }

            if (aDate == null) {
                theValidationDictionary.AddError("Date", aDate.ToString(), "Invalid date.");
            } else if(aDate <= DateTime.UtcNow) {
                theValidationDictionary.AddError("Date", aDate.ToString(), "The date must be later than now.");
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
            bool myAdminOverride = HAVPermissionHelper.AllowedToPerformAction(anUserInformation, HAVPermission.Delete_Any_Event);
            theRepository.DeleteEvent(anUserInformation.Details, anEventId, myAdminOverride);
        }

        public IEnumerable<Event> GetEventsForUser(User aViewingUser, int aUserId) {
            if (aViewingUser.Id == aUserId || theFriendService.IsFriend(aUserId, aViewingUser)) {
                return theRepository.FindEvents(aUserId, DateTime.UtcNow);
            }

            throw new NotFriendException();
        }
    }
}
