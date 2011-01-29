using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {
    public class EditPrivacySettingsModel {
        public Dictionary<string, Pair<PrivacySetting, bool>> PrivacySettings { get; set; }

        public EditPrivacySettingsModel() {
            PrivacySettings = new Dictionary<string, Pair<PrivacySetting, bool>>();
        }
    }
}