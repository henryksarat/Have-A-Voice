﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.Repositories.UserFeatures {
    public interface IHAVCalendarRepository {
        void AddEvent(int aUserId, DateTime aDate, string anInformation);
        void DeleteEvent(User aUser, int anEventId, bool anAdminDelete);
        IEnumerable<Event> FindEvents(int aUserId);
    }
}