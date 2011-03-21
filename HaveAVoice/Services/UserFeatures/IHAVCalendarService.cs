using System.Collections.Generic;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.Enums;
using System;
using HaveAVoice.Models;
using Social.Generic.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVCalendarService {
        bool AddEvent(int aUserId, DateTime aDate, string anInformation);
        void DeleteEvent(UserInformationModel<User> aUserInformation, int anEventId);
        IEnumerable<Event> GetEventsForUser(User aViewingUser, int aUserId);
    }
}