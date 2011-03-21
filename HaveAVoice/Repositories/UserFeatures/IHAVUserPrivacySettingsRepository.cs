using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Helpers;
using Social.Generic.Helpers;

namespace HaveAVoice.Repositories.UserFeatures {
    public interface IHAVUserPrivacySettingsRepository {
        void AddPrivacySettingsForUser(User aTargetUser, SocialPrivacySetting[] aSettings);
        IEnumerable<PrivacySetting> FindPrivacySettingsForUser(User aUser);
        IEnumerable<PrivacySetting> GetAllPrivacySettings();
        void UpdatePrivacySettingsForUser(User aUser, IEnumerable<PrivacySetting> aSettingsToRemove, IEnumerable<PrivacySetting> aSettingsToAdd);
    }
}