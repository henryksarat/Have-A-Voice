using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Helpers.UserInformation {
    public class UserInformationHelper {
        public static UserPrivacySetting GetPrivacySettings(User aUser) {
            UserPrivacySetting myUserPrivacy = new UserPrivacySetting();

            foreach (UserPrivacySetting myUserPrivacySetting in aUser.UserPrivacySettings) {
                myUserPrivacy = myUserPrivacySetting;
            }

            return myUserPrivacy;
        }

        public static bool IsPrivacyAllowing(User aTargetUser, PrivacyAction aPrivacyAction) {
            UserPrivacySetting myPrivacySetting = GetPrivacySettings(aTargetUser);
            bool myIsAllowed = false;
            
            if (aPrivacyAction == PrivacyAction.DisplayProfile) {
                if (myPrivacySetting.DisplayProfileToNonLoggedIn) {
                    myIsAllowed = true;
                } else if (myPrivacySetting.DisplayProfileToLoggedInUsers && HAVUserInformationFactory.IsLoggedIn()) {
                    myIsAllowed = true;
                }
                //Check for friendship
            }

            return myIsAllowed;
        }
    }
}