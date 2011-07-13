using System;
using System.Collections.Generic;
using System.Linq;
using Social.Admin.Exceptions;
using Social.Admin.Helpers;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories.Events;

namespace UniversityOfMe.Services.Events {
    public class EventService : IEventService {
        private IValidationDictionary theValidationDictionary;
        private IEventRepository theEventRepository;

        public EventService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityEventRepository()) { }

        public EventService(IValidationDictionary aValidationDictionary, IEventRepository aProfessorRepo) {
            theValidationDictionary = aValidationDictionary;
            theEventRepository = aProfessorRepo;
        }

        public IDictionary<string, string> CreateAllPrivacyOptionsEntry() {
            IDictionary<string, string> myDictionary = DictionaryHelper.DictionaryWithSelect();
            myDictionary.Add("Entire school", "true");
            myDictionary.Add("Only friends", "false");
            return myDictionary;
        }

        public Event CreateEvent(UserInformationModel<User> aStartingUser, EventViewModel aCreateEventModel) {
            if (!ValidEvent(aCreateEventModel)) {
                return null;
            }

            DateTime myStartDateUtc = TimeZoneInfo.ConvertTimeToUtc(aCreateEventModel.GetStartDate());
            DateTime myEndDateUtc = TimeZoneInfo.ConvertTimeToUtc(aCreateEventModel.GetEndDate());

            return theEventRepository.CreateEvent(aStartingUser.Details, aCreateEventModel.UniversityId, aCreateEventModel.Title, myStartDateUtc,
                                                  myEndDateUtc, aCreateEventModel.Information, Boolean.Parse(aCreateEventModel.EventPrivacyOption));
        }

        public bool PostToEventBoard(UserInformationModel<User> aPostingUser, int anEventId, string aMessage) {
            if (!ValidBoardMessage(aMessage)) {
                return false;
            }

            theEventRepository.CreateEventBoardMessage(aPostingUser.Details, anEventId, aMessage);

            return true;
        }

        public void DeleteEvent(UserInformationModel<User> aDeletingUser, string aUniversityId, int anEventId) {
            Event myEvent = GetEvent(aDeletingUser, aUniversityId, anEventId);

            if (myEvent.UserId != aDeletingUser.UserId) {
                if (!PermissionHelper<User>.AllowedToPerformAction(theValidationDictionary, aDeletingUser, SocialPermission.Delete_Any_Event)) {
                    throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
                }
            }

            theEventRepository.DeleteEvent(aDeletingUser.Details, aUniversityId, anEventId);
        }

        public Event GetEvent(UserInformationModel<User> aUser, string aUniversityId, int anEventId) {
            theEventRepository.MarkBoardAsView(aUser.Details, anEventId);
            return theEventRepository.GetEvent(aUniversityId, anEventId);
        }

        public Event GetEventForEdit(UserInformationModel<User> aUser, string aUniversityId, int anEventId) {
            Event myEvent = theEventRepository.GetEvent(aUniversityId, anEventId);

            if (myEvent.UserId != aUser.Details.Id) {
                if (!PermissionHelper<User>.AllowedToPerformAction(theValidationDictionary, aUser, SocialPermission.Edit_Any_Event)) {
                    throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
                }
            }
            return myEvent;
        }

        public IEnumerable<Event> GetEventsForUniversity(User aUser, string aUniversityId) {
            IEnumerable<Event> myEvents = theEventRepository.GetEventsForUniversity(aUniversityId);
            myEvents = (from e in myEvents
                        where (e.EntireSchool == true
                        || (!e.EntireSchool && FriendHelper.IsFriend(aUser, e.User)))
                        select e).OrderBy(e => e.StartDate);
            return myEvents;
        }

        public bool EditEvent(UserInformationModel<User> aStartingUser, EventViewModel anEditViewModel) {
            if (!ValidEvent(anEditViewModel)) {
                return false;
            }

            Event myEvent = theEventRepository.GetEvent(anEditViewModel.UniversityId, anEditViewModel.Id);

            if (myEvent == null) {
                theValidationDictionary.AddError("Event", string.Empty, "The original event doesn't seem to exist.");
                return false;
            }

            DateTime myStartDateUtc = TimeZoneInfo.ConvertTimeToUtc(anEditViewModel.GetStartDate());
            DateTime myEndDateUtc = TimeZoneInfo.ConvertTimeToUtc(anEditViewModel.GetEndDate());

            myEvent.Title = anEditViewModel.Title;
            myEvent.StartDate = myStartDateUtc;
            myEvent.EndDate = myEndDateUtc;
            myEvent.Information = anEditViewModel.Information;
            myEvent.EntireSchool = Boolean.Parse(anEditViewModel.EventPrivacyOption);

            theEventRepository.UpdateEvent(myEvent);

            return true;
        }

        public void Attend(UserInformationModel<User> aUser, int anEventId) {
            theEventRepository.AttendEvent(aUser.Details, anEventId);
        }

        public void Unattend(UserInformationModel<User> aUser, int anEventId) {
            theEventRepository.UnattendEvent(aUser.Details, anEventId);
        }

        private bool ValidBoardMessage(string aMessage) {
            if (string.IsNullOrEmpty(aMessage)) {
                theValidationDictionary.AddError("BoardMessage", aMessage, "A message is required to post on the event board.");
            }

            return theValidationDictionary.isValid;
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

            if (string.IsNullOrEmpty(aCreateEventModel.UniversityId) || aCreateEventModel.UniversityId.Equals(Constants.SELECT)) {
                theValidationDictionary.AddError("UniversityId", aCreateEventModel.UniversityId, "A university is required.");
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

            if (aCreateEventModel.EventPrivacyOption.Equals(Constants.SELECT)) {
                theValidationDictionary.AddError("EventPrivacyOption", aCreateEventModel.EventPrivacyOption, "An event privacy option is required.");
            }

            return theValidationDictionary.isValid;
        }
    }
}
