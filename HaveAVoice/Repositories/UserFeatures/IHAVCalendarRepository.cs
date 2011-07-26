using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVCalendarRepository {
        Event AddEvent(User aUser, DateTime aStartDate, DateTime anEndDate, string aTitle, string anInformation);
        void DeleteEvent(User aUser, int anEventId, bool anAdminDelete);
        IEnumerable<Event> FindEvents(int aUserId);
    }
}