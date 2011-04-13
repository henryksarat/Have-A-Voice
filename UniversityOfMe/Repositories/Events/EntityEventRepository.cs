using System;
using System.Collections.Generic;
using System.Linq;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Events {
    public class EntityEventRepository : IEventRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public Event CreateEvent(User aStartingUser, string aUniversityId, string aTitle, DateTime aStartDate, DateTime aEndDate, string anInformation, bool anEntireSchool) {
            Event myEvent = Event.CreateEvent(0, aUniversityId, aStartingUser.Id, aTitle, aStartDate, aEndDate, anInformation, anEntireSchool, false);
            theEntities.AddToEvents(myEvent);
            theEntities.SaveChanges();
            return myEvent;
        }

        public void CreateEventBoardMessage(User aPostingUser, int anEventId, string aMessage) {
            EventBoard myEventBoard = EventBoard.CreateEventBoard(0, aPostingUser.Id, anEventId, aMessage, DateTime.UtcNow);
            theEntities.AddToEventBoards(myEventBoard);
            theEntities.SaveChanges();
        }

        public void DeleteEvent(User aDeletingUser, string aUniversityId, int anEventId) {
            Event myEvent = GetEvent(aUniversityId, anEventId);
            myEvent.Deleted = true;
            myEvent.DeletedUserId = aDeletingUser.Id;
            theEntities.ApplyCurrentValues(myEvent.EntityKey.EntitySetName, myEvent);
            theEntities.SaveChanges();
        }

        public Event GetEvent(string aUniversityId, int anEventId) {
            return (from e in theEntities.Events
                    where e.Id == anEventId
                    && e.UniversityId == aUniversityId
                    && e.Deleted == false
                    select e).FirstOrDefault<Event>();
        }

        public IEnumerable<Event> GetEventsForUniversity(string aUniversityId) {
            return (from e in theEntities.Events
                    where e.UniversityId == aUniversityId
                    && e.Deleted == false
                    select e).ToList<Event>();
        }

        public void UpdateEvent(Event anEditedEvent) {
            Event myOriginal = GetEvent(anEditedEvent.Id);
            theEntities.ApplyCurrentValues(myOriginal.EntityKey.EntitySetName, anEditedEvent);
            theEntities.SaveChanges();
        }

        public Event GetEvent(int anEventId) {
            return (from e in theEntities.Events
                    where e.Id == anEventId
                    && e.Deleted == false
                    select e).FirstOrDefault<Event>();
        }
    }
}