using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVCalendarRepository : HAVBaseRepository, IHAVCalendarRepository {
        public void AddEvent(int aUserId, DateTime aDate, string anInformation) {
            IHAVUserRepository myUserRepo = new EntityHAVUserRepository();
            Event myEvent = Event.CreateEvent(0, aUserId, aDate, anInformation, false);
            GetEntities().AddToEvents(myEvent);
            GetEntities().SaveChanges();
        }

        public void DeleteEvent(User aUser, int anEventId, bool anAdminDelete) {
            Event myEvent = GetEvent(anEventId);
            if (myEvent.User.Id == aUser.Id || anAdminDelete) {
                myEvent.Deleted = true;
                myEvent.DeletedUserId = aUser.Id;
                GetEntities().ApplyCurrentValues(myEvent.EntityKey.EntitySetName, myEvent);
                GetEntities().SaveChanges();
            }
        }

        public IEnumerable<Event> FindEvents(int aUserId) {
            List<Event> myEvents = FindEventsForUser(aUserId).ToList<Event>();
            IEnumerable<Event> myUsersFanOfEvents = FindEventsOfFannedUsers(aUserId);
            myEvents.AddRange(myUsersFanOfEvents);
            return myEvents;
        }

        private IEnumerable<Event> FindEventsOfFannedUsers(int aUserId) {
            return (from e in GetEntities().Events
                    join u in GetEntities().Users on e.User.Id equals u.Id
                    join f in GetEntities().Fans on u.Id equals f.SourceUser.Id
                    where f.FanUserId == aUserId
                    && e.Deleted == false
                    select e).ToList<Event>();
        }

        private IEnumerable<Event> FindEventsForUser(int aUserId) {
            return (from e in GetEntities().Events
                    where e.User.Id == aUserId
                    && e.Deleted == false
                    select e).ToList<Event>();
        }

        private Event GetEvent(int anEventId) {
            return (from e in GetEntities().Events
                    where e.Id == anEventId
                    select e).FirstOrDefault<Event>();
        }
    }
}