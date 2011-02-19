using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Helpers {
    public class HAVPrivacyHelper {
        public static HAVPrivacySetting[] GetDefaultPrivacySettings() {
            return new HAVPrivacySetting[] {
                HAVPrivacySetting.Display_Profile_To_Friend,
                HAVPrivacySetting.Display_Profile_To_Politician,
                HAVPrivacySetting.Display_Profile_To_Political_Candidate
            };
        }

        public static HAVPrivacySetting[] GetAuthorityPrivacySettings() {
            return new HAVPrivacySetting[] {
                HAVPrivacySetting.Display_Profile_To_Friend,
                HAVPrivacySetting.Display_Profile_To_Politician,
                HAVPrivacySetting.Display_Profile_To_Not_Friend,
                HAVPrivacySetting.Display_Profile_To_Political_Candidate
            };
        }
    }
}