using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVUserPrivacySettingsRepository {
        void AddDefaultUserPrivacySettings(User aUser);
        UserPrivacySetting FindUserPrivacySettingsForUser(User aUser);
        void UpdatePrivacySettings(User aUser, UserPrivacySetting aUserPrivacySetting);
    }
}