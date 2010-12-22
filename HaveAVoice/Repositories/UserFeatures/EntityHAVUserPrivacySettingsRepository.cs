using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Services;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVUserPrivacySettingsRepository : HAVBaseRepository, IHAVUserPrivacySettingsRepository {
        public void AddDefaultUserPrivacySettings(User aUser) {
            UserPrivacySetting myPrivacySettings = UserPrivacySetting.CreateUserPrivacySetting(0, false, false, false);
            myPrivacySettings.User = FindUser(aUser.Id);
            GetEntities().AddToUserPrivacySettings(myPrivacySettings);
            GetEntities().SaveChanges();
        }

        public UserPrivacySetting FindUserPrivacySettingsForUser(User aUser) {
            return (from p in GetEntities().UserPrivacySettings
                    where p.User.Id == aUser.Id
                    select p).FirstOrDefault<UserPrivacySetting>();
        }

        public void UpdatePrivacySettings(User aUser, UserPrivacySetting aUserPrivacySetting) {
            UserPrivacySetting mySettings = FindUserPrivacySettings(aUserPrivacySetting);
            GetEntities().ApplyCurrentValues(mySettings.EntityKey.EntitySetName, aUserPrivacySetting);
            GetEntities().SaveChanges();
        }

        private UserPrivacySetting FindUserPrivacySettings(UserPrivacySetting aUserPrivacySetting) {
            return (from p in GetEntities().UserPrivacySettings
                    where p.Id == aUserPrivacySetting.Id
                    select p).FirstOrDefault<UserPrivacySetting>();
        }

        private User FindUser(int aUserId) {
            IHAVUserRetrievalRepository myUserRetrievalRepo = new EntityHAVUserRetrievalRepository();
            return myUserRetrievalRepo.GetUser(aUserId);
        }
    }
}