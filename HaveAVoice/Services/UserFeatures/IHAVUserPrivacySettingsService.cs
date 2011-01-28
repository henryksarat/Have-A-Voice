using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVUserPrivacySettingsService {
        IEnumerable<PrivacySetting> FindPrivacySettingsForUser(User aUser);
        void AddDefaultPrivacySettingsForUser(User aUser);
    }
}