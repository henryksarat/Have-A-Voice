using System.Collections.Generic;
using Social.Generic;

namespace Social.BaseWebsite.Models {
    public class DisplayPrivacySettingsModel<T> {
        public Dictionary<string, IEnumerable<Pair<T, bool>>> PrivacySettings { get; set; }

        public DisplayPrivacySettingsModel() {
            PrivacySettings = new Dictionary<string, IEnumerable<Pair<T, bool>>>();
        }
    }
}