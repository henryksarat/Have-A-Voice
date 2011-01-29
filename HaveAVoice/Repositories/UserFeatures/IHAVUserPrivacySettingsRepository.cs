using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Helpers;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVUserPrivacySettingsRepository {
        IEnumerable<PrivacySetting> FindPrivacySettingsForUser(User aUser);
        void AddPrivacySettingsForUser(User aTargetUser, HAVPrivacySetting[] aSettings);
    }
}