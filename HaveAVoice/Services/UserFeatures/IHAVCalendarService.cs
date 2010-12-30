using System.Collections.Generic;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.Enums;
using System;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVCalendarService {
        bool AddEvent(int aUserId, DateTime aDate, string anInformation);
        void DeleteEvent(UserInformationModel aUserInformation, int anEventId);
        LoggedInModel<Event> GetEventsForUser(User aViewingUser, int aUserId);
    }
}