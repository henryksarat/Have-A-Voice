﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Services.Helpers {
    public class PrivacyHelper {
        public static bool IsAllowed(User aTargetUser, PrivacyAction aPrivacyAction) {
            UserPrivacySetting myPrivacySetting = GetPrivacySettings(aTargetUser);
            bool myIsAllowed = false;
            bool myIsLoggedIn = HAVUserInformationFactory.IsLoggedIn();

            if (aPrivacyAction == PrivacyAction.DisplayProfile) {
                if (myPrivacySetting.DisplayProfileToNonLoggedIn) {
                    myIsAllowed = true;
                } else if (myPrivacySetting.DisplayProfileToLoggedInUsers && myIsLoggedIn) {
                    myIsAllowed = true;
                } else if (myPrivacySetting.DisplayProfileToFriends && myIsLoggedIn) {
                    IHAVFanService myFanService = new HAVFanService();
                    myIsAllowed = myFanService.IsFan(aTargetUser.Id, HAVUserInformationFactory.GetUserInformation().Details);
                }
            }

            return myIsAllowed;
        }

        private static UserPrivacySetting GetPrivacySettings(User aUser) {
            UserPrivacySetting myUserPrivacy = new UserPrivacySetting();

            foreach (UserPrivacySetting myUserPrivacySetting in aUser.UserPrivacySettings) {
                myUserPrivacy = myUserPrivacySetting;
            }

            return myUserPrivacy;
        }
    }
}