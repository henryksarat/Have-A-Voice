using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {
    public class DisplayPrivacySettingsModel : HaveAVoice.Models.View.LoggedInModel {
        public Dictionary<string, IEnumerable<Pair<PrivacySetting, bool>>> PrivacySettings { get; set; }

        public DisplayPrivacySettingsModel(User aUser)
            : base(aUser, SiteSection.EditPrivacy) {
                PrivacySettings = new Dictionary<string, IEnumerable<Pair<PrivacySetting, bool>>>();
        }
    }
}