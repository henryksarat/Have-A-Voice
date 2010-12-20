using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVUserPrivacySettingsService {
        void AddDefaultUserPrivacySettings(User aUser);
        UserPrivacySetting FindUserPrivacySettingsForUser(User aUser);
        void UpdatePrivacySettings(User aUser, UserPrivacySetting aUserPrivacySetting);
    }
}