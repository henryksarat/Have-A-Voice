using System;
using System.Collections.Generic;
using HaveAVoice.Exceptions;
using HaveAVoice.Models;
using HaveAVoice.Models.SocialWrappers;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using Social.Admin.Helpers;
using Social.Friend.Services;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.User.Models;
using Social.Validation;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVCalendarService : HAVBaseService, IHAVCalendarService {
        private IValidationDictionary theValidationDictionary;
        private IFriendService<User, Friend> theFriendService;
        private IHAVPhotoService thePhotoService;
        private IHAVCalendarRepository theRepository;

        public HAVCalendarService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new FriendService<User, Friend>(new EntityHAVFriendRepository()), new HAVPhotoService(), new EntityHAVCalendarRepository(), new HAVBaseRepository()) { }

        public HAVCalendarService(IValidationDictionary aValidationDictionary, IFriendService<User, Friend> aFriendService, IHAVPhotoService aPhotoService, 
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

        public void DeleteEvent(UserInformationModel<User> anUserInformation, int anEventId) {
            bool myAdminOverride = PermissionHelper<User>.AllowedToPerformAction(anUserInformation, SocialPermission.Delete_Any_Event);
            theRepository.DeleteEvent(anUserInformation.Details, anEventId, myAdminOverride);
        }

        public IEnumerable<Event> GetEventsForUser(User aViewingUser, int aUserId) {
            AbstractUserModel<User> myAbstractUser = new UserModel(aViewingUser);
 
            if (aViewingUser.Id == aUserId || theFriendService.IsFriend(aUserId, myAbstractUser)) {
                return theRepository.FindEvents(aUserId, DateTime.UtcNow);
            }

            throw new NotFriendException();
        }
    }
}
