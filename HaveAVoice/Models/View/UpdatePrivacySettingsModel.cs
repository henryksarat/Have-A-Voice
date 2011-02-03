using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {
    public class UpdatePrivacySettingsModel {
        public IEnumerable<Pair<PrivacySetting, bool>> PrivacySettings { get; set; }

        public UpdatePrivacySettingsModel() {
                PrivacySettings = new List<Pair<PrivacySetting, bool>>();
        }
    }
}