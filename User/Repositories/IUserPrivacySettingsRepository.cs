using System.Collections.Generic;
using Social.Generic.Helpers;
using Social.User.Models;

namespace Social.User.Repositories {
    public interface IUserPrivacySettingsRepository<T, U> {
        void AddPrivacySettingsForUser(T aTargetUser, SocialPrivacySetting[] aSettings);
        IEnumerable<AbstractPrivacySettingModel<U>> FindPrivacySettingsForUser(T aUser);
        IEnumerable<AbstractPrivacySettingModel<U>> GetAllPrivacySettings();
        void UpdatePrivacySettingsForUser(T aUser, IEnumerable<U> aSettingsToRemove, IEnumerable<U> aSettingsToAdd);
    }
}