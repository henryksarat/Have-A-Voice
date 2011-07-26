using System.Collections.Generic;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.Enums;
using System;
using HaveAVoice.Models;
using Social.Generic.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVCalendarService {
        Event AddEvent(UserInformationModel<User> aUserInfo, EventViewModel anEventModel);
        void DeleteEvent(UserInformationModel<User> aUserInformation, int anEventId);
        IEnumerable<Event> GetEventsForUser(User aViewingUser, int aUserId);
    }
}