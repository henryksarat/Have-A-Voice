using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Helpers;
using HaveAVoice.Models.View;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVUserPrivacySettingsService {
        EditPrivacySettingsModel GetPrivacySettingsForEdit(User aUser);
        IEnumerable<PrivacySetting> FindPrivacySettingsForUser(User aUser);
        void AddDefaultPrivacySettingsForUser(User aUser);
        void UpdatePrivacySettings(User aUser, EditPrivacySettingsModel aPrivacySettings);
    }
}