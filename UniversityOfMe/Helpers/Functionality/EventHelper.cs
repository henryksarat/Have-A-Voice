using System.Linq;
using UniversityOfMe.Models;

namespace UniversityOfMe.Helpers.Functionality {
    public class EventHelper {
        public static bool IsAttending(User aUser, Event anEvent) {
            return (anEvent.UserId == aUser.Id) || 
                   (from e in anEvent.EventAttendences
                    where e.UserId == aUser.Id
                    && e.EventId == anEvent.Id
                    select e).Count<EventAttendence>() > 0 ? true : false;
        }
    }
}