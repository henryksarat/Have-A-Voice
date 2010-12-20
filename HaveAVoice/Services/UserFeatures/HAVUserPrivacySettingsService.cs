using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Repositories;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVUserPrivacySettingsService : HAVBaseService, IHAVUserPrivacySettingsService {
        private IHAVUserPrivacySettingsRepository thePrivacySettingsRepo;

        public HAVUserPrivacySettingsService()
            : this(new HAVBaseRepository(), new EntityHAVUserPrivacySettingsRepository()) { }

        public HAVUserPrivacySettingsService(IHAVBaseRepository baseRepository, IHAVUserPrivacySettingsRepository aPrivacySettingsRepo)
            : base(baseRepository) {
            thePrivacySettingsRepo = aPrivacySettingsRepo;
        }

        public void AddDefaultUserPrivacySettings(User aUser) {
            thePrivacySettingsRepo.AddDefaultUserPrivacySettings(aUser);
        }

        public void UpdatePrivacySettings(User aUser, UserPrivacySetting aUserPrivacySetting) {
            thePrivacySettingsRepo.UpdatePrivacySettings(aUser, aUserPrivacySetting);
        }

        public UserPrivacySetting FindUserPrivacySettingsForUser(User aUser) {
            return thePrivacySettingsRepo.FindUserPrivacySettingsForUser(aUser);
        }
    }
}