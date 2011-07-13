using Social.User.Models;

namespace UniversityOfMe.Models.SocialModels {
    public class SocialPrivacySettingModel : AbstractPrivacySettingModel<PrivacySetting> {
        public static AbstractPrivacySettingModel<PrivacySetting> Create(PrivacySetting anExternal) {
            if (anExternal != null) {
                return new SocialPrivacySettingModel(anExternal);
            }
            return null;
        }

        public override PrivacySetting FromModel() {
            return PrivacySetting.CreatePrivacySetting(Name, CreatedByUserId, DisplayName, Description, PrivacyGroup, ListOrder);
        }

        private SocialPrivacySettingModel(PrivacySetting anExternal) {
            Name = anExternal.Name;
            CreatedByUserId = anExternal.CreatedByUserId;
            DisplayName = anExternal.DisplayName;
            Description = anExternal.Description;
            PrivacyGroup = anExternal.PrivacyGroup;
            ListOrder = anExternal.ListOrder;
        }
    }
}