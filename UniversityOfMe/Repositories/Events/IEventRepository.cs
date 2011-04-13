using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Events {
    public interface IEventRepository {
        Event CreateEvent(User aStartingUser, string aUniversityId, string aTitle, DateTime aStartDate, DateTime aEndDate, string anInformation, bool anEntireSchool);
        void CreateEventBoardMessage(User aPostingUser, int anEventId, string aMessage);
        void DeleteEvent(User aDeletingUser, string aUniversityId, int anEventId);
        Event GetEvent(string aUniversityId, int anEventId);
        IEnumerable<Event> GetEventsForUniversity(string aUniversityId);
        void UpdateEvent(Event anEditedEvent);
    }
}
