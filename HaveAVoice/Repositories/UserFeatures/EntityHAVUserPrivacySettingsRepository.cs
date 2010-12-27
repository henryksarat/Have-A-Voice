using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Services;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVUserPrivacySettingsRepository : IHAVUserPrivacySettingsRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public void AddDefaultUserPrivacySettings(User aUser) {
            UserPrivacySetting myPrivacySettings = UserPrivacySetting.CreateUserPrivacySetting(0, aUser.Id, false, false, false);
            theEntities.AddToUserPrivacySettings(myPrivacySettings);
            theEntities.SaveChanges();
        }

        public UserPrivacySetting FindUserPrivacySettingsForUser(User aUser) {
            return (from p in theEntities.UserPrivacySettings
                    where p.User.Id == aUser.Id
                    select p).FirstOrDefault<UserPrivacySetting>();
        }

        public void UpdatePrivacySettings(User aUser, UserPrivacySetting aUserPrivacySetting) {
            UserPrivacySetting mySettings = FindUserPrivacySettings(aUserPrivacySetting);
            theEntities.ApplyCurrentValues(mySettings.EntityKey.EntitySetName, aUserPrivacySetting);
            theEntities.SaveChanges();
        }

        private UserPrivacySetting FindUserPrivacySettings(UserPrivacySetting aUserPrivacySetting) {
            return (from p in theEntities.UserPrivacySettings
                    where p.Id == aUserPrivacySetting.Id
                    select p).FirstOrDefault<UserPrivacySetting>();
        }
    }
}