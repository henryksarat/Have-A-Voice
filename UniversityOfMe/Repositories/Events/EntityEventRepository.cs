using System;
using System.Collections.Generic;
using System.Linq;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories.Helpers;
using UniversityOfMe.Helpers.Badges;

namespace UniversityOfMe.Repositories.Events {
    public class EntityEventRepository : IEventRepository {
        private UniversityOfMeEntities theEntities = new UniversityOfMeEntities();

        public void AttendEvent(User aUser, int anEventId) {
            EventAttendence myEventAttendence = EventAttendence.CreateEventAttendence(0, anEventId, aUser.Id, true);
            theEntities.AddToEventAttendences(myEventAttendence);

            BadgeHelper.AddNecessaryBadgesAndPoints(theEntities, aUser.Id, BadgeAction.ATTENDED, BadgeSection.EVENT, anEventId);

            theEntities.SaveChanges();
        }

        public Event CreateEvent(User aStartingUser, string aUniversityId, string aTitle, DateTime aStartDate, DateTime aEndDate, string anInformation, bool anEntireSchool) {
            Event myEvent = Event.CreateEvent(0, aUniversityId, aStartingUser.Id, aTitle, aStartDate, aEndDate, anInformation, anEntireSchool, false, DateTime.UtcNow);
            theEntities.AddToEvents(myEvent);
            theEntities.SaveChanges();

            BadgeHelper.AddNecessaryBadgesAndPoints(theEntities, aStartingUser.Id, BadgeAction.CREATED, BadgeSection.EVENT, myEvent.Id);

            EventAttendence myAttendence = EventAttendence.CreateEventAttendence(0, myEvent.Id, aStartingUser.Id, true);
            theEntities.AddToEventAttendences(myAttendence);

            theEntities.SaveChanges();

            return myEvent;
        }

        public void CreateEventBoardMessage(User aPostingUser, int anEventId, string aMessage) {
            EventBoard myEventBoard = EventBoard.CreateEventBoard(0, aPostingUser.Id, anEventId, aMessage, DateTime.UtcNow);
            theEntities.AddToEventBoards(myEventBoard);

            IEnumerable<EventAttendence> myEventAttendences = GetEventAttendences(anEventId);
            DateTime myDateTime = DateTime.UtcNow;

            foreach (EventAttendence myEventAttendence in myEventAttendences) {
                if (myEventAttendence.UserId == aPostingUser.Id) {
                    myEventAttendence.BoardViewed = true;                    
                } else {
                    myEventAttendence.BoardViewed = false;
                }
                myEventAttendence.LastBoardPost = myDateTime;
            }

            //Just in case the founder wasn't added for some reason
            Event myEvent = GetEvent(anEventId);
            EventAttendence myEventFounderAttendence = GetEventAttendence(myEvent.UserId, myEvent.Id);
            if (myEventFounderAttendence == null) {
                myEventFounderAttendence = EventAttendence.CreateEventAttendence(0, myEvent.Id, myEvent.UserId, false);
                myEventFounderAttendence.LastBoardPost = myDateTime;
                theEntities.AddToEventAttendences(myEventFounderAttendence);
            }


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
                    && e.EndDate >= DateTime.UtcNow
                    select e).FirstOrDefault<Event>();
        }

        public IEnumerable<Event> GetEventsForUniversity(string aUniversityId) {
            return (from e in theEntities.Events
                    where e.UniversityId == aUniversityId
                    && e.Deleted == false
                    && e.EndDate >= DateTime.UtcNow
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

        public void MarkBoardAsView(User aUser, int anEventId) {
            EventAttendence myEventAttendence = GetEventAttendence(aUser.Id, anEventId);
            if (myEventAttendence != null) {
                myEventAttendence.BoardViewed = true;
                theEntities.SaveChanges();
            }
        }

        public void UnattendEvent(User aUser, int anEventId) {
            EventAttendence myEventAttendence = GetEventAttendence(aUser.Id, anEventId);
            if (myEventAttendence != null) {
                theEntities.DeleteObject(myEventAttendence);
                theEntities.SaveChanges();
            }
        }

        private EventAttendence GetEventAttendence(int aUserId, int anEventId) {
            return (from ea in theEntities.EventAttendences
                    where ea.UserId == aUserId
                    && ea.EventId == anEventId
                    select ea).FirstOrDefault<EventAttendence>();
        }

        private IEnumerable<EventAttendence> GetEventAttendences(int anEventId) {
            return (from ea in theEntities.EventAttendences
                    where ea.EventId == anEventId
                    select ea);
        }
    }
}