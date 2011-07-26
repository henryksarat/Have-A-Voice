using System;
using System.Collections.Generic;
using System.Linq;
using HaveAVoice.Models;
using Social.User.Repositories;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVCalendarRepository : IHAVCalendarRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public Event AddEvent(User aUser, DateTime aStartDate, DateTime anEndDate, string aTitle, string anInformation) {
            IUserRepository<User, Role, UserRole> myUserRepo = new EntityHAVUserRepository();
            Event myEvent = Event.CreateEvent(0, aUser.Id, aStartDate, anEndDate , aTitle, anInformation, false);
            theEntities.AddToEvents(myEvent);
            theEntities.SaveChanges();

            return myEvent;
        }

        public void DeleteEvent(User aUser, int anEventId, bool anAdminDelete) {
            Event myEvent = GetEvent(anEventId);
            if (myEvent.User.Id == aUser.Id || anAdminDelete) {
                myEvent.Deleted = true;
                myEvent.DeletedUserId = aUser.Id;
                theEntities.ApplyCurrentValues(myEvent.EntityKey.EntitySetName, myEvent);
                theEntities.SaveChanges();
            }
        }

        public IEnumerable<Event> FindEvents(int aUserId) {
            List<Event> myEvents = FindEventsForUser(aUserId).ToList<Event>();
            //IEnumerable<Event> myUsersFanOfEvents = FindEventsOfFannedUsers(aUserId);
            //myEvents.AddRange(myUsersFanOfEvents);
            return myEvents;
        }

        private IEnumerable<Event> FindEventsOfFriendedUsers(int aUserId) {
            return (from e in theEntities.Events
                    join u in theEntities.Users on e.User.Id equals u.Id
                    join f in theEntities.Friends on u.Id equals f.SourceUserId
                    where f.FriendUserId == aUserId
                    && e.Deleted == false
                    select e).ToList<Event>();
        }

        private IEnumerable<Event> FindEventsForUser(int aUserId) {
            return (from e in theEntities.Events
                    where e.User.Id == aUserId
                    && e.Deleted == false
                    && e.StartDate > DateTime.UtcNow 
                    && e.EndDate < DateTime.Now
                    select e).OrderBy(e2 => e2.StartDate).ToList<Event>();
        }

        private Event GetEvent(int anEventId) {
            return (from e in theEntities.Events
                    where e.Id == anEventId
                    select e).FirstOrDefault<Event>();
        }
    }
}