using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Helpers {
    public class HAVPrivacyHelper {

        private static Dictionary<HAVPrivacySetting, string> GROUPINGS = new Dictionary<HAVPrivacySetting, string>() {
            { HAVPrivacySetting.Display_Profile_To_Politician, "Display Profile"},
            { HAVPrivacySetting.Display_Profile_To_Friend, "Display Profile"},
            { HAVPrivacySetting.Display_Profile_To_Not_Friend, "Display Profile"},
            { HAVPrivacySetting.Display_Profile_To_Not_Logged_In, "Display Profile"}
        };

        public static HAVPrivacySetting[] GetDefaultPrivacySettings() {
            return new HAVPrivacySetting[] {
                HAVPrivacySetting.Display_Profile_To_Friend,
                HAVPrivacySetting.Display_Profile_To_Politician
            };
        }

        public static HAVPrivacySetting[] GetPoliticianPrivacySettings() {
            return new HAVPrivacySetting[] {
                HAVPrivacySetting.Display_Profile_To_Friend,
                HAVPrivacySetting.Display_Profile_To_Politician,
                HAVPrivacySetting.Display_Profile_To_Not_Friend,
                HAVPrivacySetting.Display_Profile_To_Not_Logged_In
            };
        }

    }
}