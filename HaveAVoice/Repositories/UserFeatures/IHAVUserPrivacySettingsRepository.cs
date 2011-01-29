using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Helpers;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVUserPrivacySettingsRepository {
        void AddPrivacySettingsForUser(User aTargetUser, HAVPrivacySetting[] aSettings);
        IEnumerable<PrivacySetting> FindPrivacySettingsForUser(User aUser);
        IEnumerable<PrivacySetting> GetAllPrivacySettings(); 
    }
}