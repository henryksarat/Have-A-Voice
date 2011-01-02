using System.Collections.Generic;
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
        private IHAVFanService theFanService;
        private IHAVUserPictureService theUserPictureService;
        private IHAVCalendarRepository theRepository;

        public HAVCalendarService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new HAVFanService(), new HAVUserPictureService(), new EntityHAVCalendarRepository(), new HAVBaseRepository()) { }

        public HAVCalendarService(IValidationDictionary aValidationDictionary, IHAVFanService aFanService, IHAVUserPictureService aUserPictureService, 
                                  IHAVCalendarRepository aRepository, IHAVBaseRepository baseRepository)
            : base(baseRepository) {
            theValidationDictionary = aValidationDictionary;
            theUserPictureService = aUserPictureService;
            theFanService = aFanService;
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

        public LoggedInModel<Event> GetEventsForUser(User aViewingUser, int aUserId) {
            if (aViewingUser.Id == aUserId || theFanService.IsFan(aUserId, aViewingUser)) {
                return new LoggedInModel<Event>(aViewingUser) {
                    Models = theRepository.FindEvents(aUserId)
                };
            }

            throw new NotFanException();
        }
    }
}
