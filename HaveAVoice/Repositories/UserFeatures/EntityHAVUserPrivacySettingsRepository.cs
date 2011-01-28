using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Services;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVUserPrivacySettingsRepository : IHAVUserPrivacySettingsRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public IEnumerable<PrivacySetting> FindPrivacySettingsForUser(User aUser) {
            return (from p in theEntities.PrivacySettings
                    join up in theEntities.UserPrivacySettings on p.Id equals up.PrivacySettingId
                    where up.UserId == aUser.Id
                    select p).ToList<PrivacySetting>();
        }
    }
}