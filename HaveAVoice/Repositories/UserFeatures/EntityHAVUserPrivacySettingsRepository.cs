using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Services;
using HaveAVoice.Models;
using HaveAVoice.Helpers;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVUserPrivacySettingsRepository : IHAVUserPrivacySettingsRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public IEnumerable<PrivacySetting> FindPrivacySettingsForUser(User aUser) {
            return (from p in theEntities.PrivacySettings
                    join up in theEntities.UserPrivacySettings on p.Id equals up.PrivacySettingId
                    where up.UserId == aUser.Id
                    select p).ToList<PrivacySetting>();
        }

        public void AddPrivacySettingsForUser(User aTargetUser, HAVPrivacySetting[] aSettings) {
            foreach (HAVPrivacySetting mySetting in aSettings) {
                PrivacySetting myPrivacySetting = FindPrivacySettingByName(mySetting.ToString());
                UserPrivacySetting myUserPrivacySetting = UserPrivacySetting.CreateUserPrivacySetting(0, aTargetUser.Id, myPrivacySetting.Id);

                theEntities.AddToUserPrivacySettings(myUserPrivacySetting);
            }

            theEntities.SaveChanges();
        }

        private PrivacySetting FindPrivacySettingByName(string aName) {
            return (from p in theEntities.PrivacySettings
                    where p.Name == aName
                    select p).FirstOrDefault<PrivacySetting>();
        }
    }
}