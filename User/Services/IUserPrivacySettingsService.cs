using System.Collections.Generic;
using Social.User.Models;
using Social.Generic.Helpers;
using Social.Generic;

namespace Social.User.Services {
    public interface IUserPrivacySettingsService<T, U> {
        void AddPrivacySettingsForUser(T aUser, SocialPrivacySetting[] aPrivacySettings);
        IEnumerable<U> FindPrivacySettingsForUser(T aUser);
        Dictionary<string, IEnumerable<Pair<U, bool>>> GetPrivacySettingsGrouped(T aUser);
        void UpdatePrivacySettings(T aUser, UpdatePrivacySettingsModel<U> aPrivacySettings);
    }
}