using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Services;
using HaveAVoice.Models;
using HaveAVoice.Helpers;
using Social.Generic.Helpers;

namespace HaveAVoice.Repositories.UserFeatures {
    public class EntityHAVUserPrivacySettingsRepository : IHAVUserPrivacySettingsRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public void AddPrivacySettingsForUser(User aTargetUser, SocialPrivacySetting[] aSettings) {
            foreach (SocialPrivacySetting mySetting in aSettings) {
                PrivacySetting myPrivacySetting = FindPrivacySettingByName(mySetting.ToString());
                UserPrivacySetting myUserPrivacySetting = UserPrivacySetting.CreateUserPrivacySetting(0, aTargetUser.Id, myPrivacySetting.Name);

                theEntities.AddToUserPrivacySettings(myUserPrivacySetting);
            }

            theEntities.SaveChanges();
        }

        public IEnumerable<PrivacySetting> FindPrivacySettingsForUser(User aUser) {
            return (from p in theEntities.PrivacySettings
                    join up in theEntities.UserPrivacySettings on p.Name equals up.PrivacySettingName
                    where up.UserId == aUser.Id
                    select p).ToList<PrivacySetting>();
        }

        private PrivacySetting FindPrivacySettingByName(string aName) {
            return (from p in theEntities.PrivacySettings
                    where p.Name == aName
                    select p).FirstOrDefault<PrivacySetting>();
        }

        public IEnumerable<PrivacySetting> GetAllPrivacySettings() {
            return (from p in theEntities.PrivacySettings
                    select p).ToList<PrivacySetting>();
        }

        public void UpdatePrivacySettingsForUser(User aUser, IEnumerable<PrivacySetting> aSettingsToRemove, IEnumerable<PrivacySetting> aSettingsToAdd) {
            foreach (PrivacySetting mySetting in aSettingsToRemove) {
                UserPrivacySetting mySettingToDelete = FindUserPrivacySetting(aUser, mySetting);
                theEntities.DeleteObject(mySettingToDelete);
            }

            foreach (PrivacySetting mySetting in aSettingsToAdd) {
                UserPrivacySetting mySettingToCreate = UserPrivacySetting.CreateUserPrivacySetting(0, aUser.Id, mySetting.Name);
                theEntities.AddToUserPrivacySettings(mySettingToCreate);
            }

            theEntities.SaveChanges();
        }

        public UserPrivacySetting FindUserPrivacySetting(User aUser, PrivacySetting aSetting) {
            return (from u in theEntities.UserPrivacySettings
                    where u.UserId == aUser.Id
                    && u.PrivacySettingName == aSetting.Name
                    select u).FirstOrDefault<UserPrivacySetting>();
        }
    }
}