using System.Collections.Generic;
using Social.Generic;

namespace Social.User.Models {
    public class UpdatePrivacySettingsModel<T> {
        public IEnumerable<Pair<AbstractPrivacySettingModel<T>, bool>> PrivacySettings { get; set; }

        public UpdatePrivacySettingsModel() {
            PrivacySettings = new List<Pair<AbstractPrivacySettingModel<T>, bool>>();
        }
    }
}
