using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Repositories;
using HaveAVoice.Helpers;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVUserPrivacySettingsService : HAVBaseService, IHAVUserPrivacySettingsService {
        private IHAVUserPrivacySettingsRepository thePrivacySettingsRepo;

        private HAVPrivacySetting[] DEFAULT_PRIVACY_SETTINGS = new HAVPrivacySetting[] { HAVPrivacySetting.Display_Profile_To_Friend };

        public HAVUserPrivacySettingsService()
            : this(new HAVBaseRepository(), new EntityHAVUserPrivacySettingsRepository()) { }

        public HAVUserPrivacySettingsService(IHAVBaseRepository baseRepository, IHAVUserPrivacySettingsRepository aPrivacySettingsRepo)
            : base(baseRepository) {
            thePrivacySettingsRepo = aPrivacySettingsRepo;
        }

        public IEnumerable<PrivacySetting> FindPrivacySettingsForUser(User aUser) {
            return thePrivacySettingsRepo.FindPrivacySettingsForUser(aUser);
        }

        public void AddDefaultPrivacySettingsForUser(User aUser) {
            thePrivacySettingsRepo.AddPrivacySettingsForUser(aUser, DEFAULT_PRIVACY_SETTINGS);
        }

        public void UpdatePrivacySettings(User aUser, IEnumerable<Models.DataStructures.Pair<PrivacySetting>> aPrivacySettings) {
            throw new NotImplementedException();
        }
    }
}