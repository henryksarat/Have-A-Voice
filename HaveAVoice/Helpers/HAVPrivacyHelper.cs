using Social.Generic.Helpers;

namespace HaveAVoice.Helpers {
    public class HAVPrivacyHelper {
        public static SocialPrivacySetting[] GetDefaultPrivacySettings() {
            return new SocialPrivacySetting[] {
                SocialPrivacySetting.Display_Profile_To_Friend,
                SocialPrivacySetting.Display_Profile_To_Politician,
                SocialPrivacySetting.Display_Profile_To_Political_Candidate
            };
        }

        public static SocialPrivacySetting[] GetAuthorityPrivacySettings() {
            return new SocialPrivacySetting[] {
                SocialPrivacySetting.Display_Profile_To_Friend,
                SocialPrivacySetting.Display_Profile_To_Politician,
                SocialPrivacySetting.Display_Profile_To_Not_Friend,
                SocialPrivacySetting.Display_Profile_To_Political_Candidate
            };
        }
    }
}