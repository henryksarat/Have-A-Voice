using System;
using System.Collections.Generic;
using HaveAVoice.Models;
using HaveAVoice.Models.SocialWrappers;
using HaveAVoice.Repositories.UserFeatures;
using Social.Admin.Helpers;
using Social.Friend.Exceptions;
using Social.Friend.Services;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Photo.Services;
using Social.Validation;
using HaveAVoice.Models.View;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVCalendarService : IHAVCalendarService {
        private IValidationDictionary theValidationDictionary;
        private IFriendService<User, Friend> theFriendService;
        private IPhotoService<User, PhotoAlbum, Photo, Friend> thePhotoService;
        private IHAVCalendarRepository theRepository;

        public HAVCalendarService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new FriendService<User, Friend>(new EntityHAVFriendRepository()),
                                                                          new PhotoService<User, PhotoAlbum, Photo, Friend>(new FriendService<User, Friend>(new EntityHAVFriendRepository()), new EntityHAVPhotoAlbumRepository(), new EntityHAVPhotoRepository()),
                                                                          new EntityHAVCalendarRepository()) { }

        public HAVCalendarService(IValidationDictionary aValidationDictionary, IFriendService<User, Friend> aFriendService, IPhotoService<User, PhotoAlbum, Photo, Friend> aPhotoService, 
                                  IHAVCalendarRepository aRepository) {
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


        public Event AddEvent(UserInformationModel<User> aUserInfo, EventViewModel anEventModel) {
            if (!ValidEvent(anEventModel)) {
                return null;
            }

            DateTime myStartDateUtc = TimeZoneInfo.ConvertTimeToUtc(anEventModel.GetStartDate());
            DateTime myEndDateUtc = TimeZoneInfo.ConvertTimeToUtc(anEventModel.GetEndDate());

            return theRepository.AddEvent(aUserInfo.Details, myStartDateUtc,
                myEndDateUtc, anEventModel.Title, anEventModel.Information);
        }

        public void DeleteEvent(UserInformationModel<User> anUserInformation, int anEventId) {
            bool myAdminOverride = PermissionHelper<User>.AllowedToPerformAction(anUserInformation, SocialPermission.Delete_Any_Event);
            theRepository.DeleteEvent(anUserInformation.Details, anEventId, myAdminOverride);
        }

        public IEnumerable<Event> GetEventsForUser(User aViewingUser, int aUserId) {
            AbstractUserModel<User> myAbstractUser = SocialUserModel.Create(aViewingUser);
 
            if (aViewingUser.Id == aUserId || theFriendService.IsFriend(aUserId, myAbstractUser)) {
                return theRepository.FindEvents(aUserId);
            }

            throw new NotFriendException();
        }

        private bool ValidEvent(EventViewModel aCreateEventModel) {
            DateTime myStartTimeUtc = TimeZoneInfo.ConvertTimeToUtc(aCreateEventModel.GetStartDate());
            DateTime myEndTimeUtc = TimeZoneInfo.ConvertTimeToUtc(aCreateEventModel.GetEndDate());

            if (string.IsNullOrEmpty(aCreateEventModel.Title)) {
                theValidationDictionary.AddError("Title", aCreateEventModel.Title, "A title for the event is required.");
            }

            if (string.IsNullOrEmpty(aCreateEventModel.Information)) {
                theValidationDictionary.AddError("Information", aCreateEventModel.Information, "Information for the event is required.");
            }

            if (aCreateEventModel.StartDate == null) {
                theValidationDictionary.AddError("StartDate", aCreateEventModel.StartDate.ToString(), "Invalid date.");
            } else if (myStartTimeUtc <= DateTime.UtcNow) {
                theValidationDictionary.AddError("StartDate", aCreateEventModel.StartDate.ToString(), "The start date must be later than now.");
            }

            if (aCreateEventModel.EndDate == null) {
                theValidationDictionary.AddError("EndDate", aCreateEventModel.EndDate.ToString(), "Invalid date.");
            } else if (myEndTimeUtc <= myStartTimeUtc) {
                theValidationDictionary.AddError("EndDate", aCreateEventModel.EndDate.ToString(), "The end date must be later than the start date.");
            }

            return theValidationDictionary.isValid;
        }
    }
}
