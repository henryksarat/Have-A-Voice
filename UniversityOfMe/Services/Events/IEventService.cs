using System.Collections.Generic;
using System.Web;
using Social.Generic.Models;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using System;

namespace UniversityOfMe.Services.Events {
    public interface IEventService {
        IDictionary<string, string> CreateAllPrivacyOptionsEntry();
        Event CreateEvent(UserInformationModel<User> aStartingUser, EventViewModel aCreateEventModel);
        bool PostToEventBoard(UserInformationModel<User> aPostingUser, int anEventId, string aMessage);
        void DeleteEvent(UserInformationModel<User> aDeletingUser, string aUniversityId, int anEventId);
        Event GetEvent(string aUniversityId, int anEventId);
        Event GetEventForEdit(UserInformationModel<User> aUser, string aUniversityId, int anEventId);
        IEnumerable<Event> GetEventsForUniversity(User aUser, string aUniversityId);
        bool EditEvent(UserInformationModel<User> aStartingUser, EventViewModel aCreateEventModel);
    }
}