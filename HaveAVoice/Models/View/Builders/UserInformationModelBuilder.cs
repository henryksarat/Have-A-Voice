using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.UserInformation;

namespace HaveAVoice.Models.View.Builders {
    public class UserInformationModelBuilder {
        private User theUser { get; set; }
        private List<Permission> thePermissions { get; set; }
        private Restriction theRestriction { get; set; }
        private List<PrivacySetting> thePrivacySettings { get; set; }

        public UserInformationModelBuilder(User aUser) {
            theUser = aUser;
            thePermissions = new List<Permission>();
            theRestriction = new Restriction();
            thePrivacySettings = new List<PrivacySetting>();
        }

        public UserInformationModelBuilder AddPermissions(IEnumerable<Permission> aPermissions) {
            thePermissions.AddRange(aPermissions);
            return this;
        }

        public UserInformationModelBuilder AddPermission(Permission aPermission) {
            thePermissions.Add(aPermission);
            return this;
        }

        public UserInformationModelBuilder SetRestriction(Restriction aRestriction) {
            theRestriction = aRestriction;
            return this;
        }

        public UserInformationModelBuilder AddPrivacySettings(List<PrivacySetting> aSettings) {
            thePrivacySettings.AddRange(aSettings);
            return this;
        }

        public UserInformationModel Build() {
            return new UserInformationModel(theUser, thePermissions, theRestriction, thePrivacySettings);
        }
    }
}