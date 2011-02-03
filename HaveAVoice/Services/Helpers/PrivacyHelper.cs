﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Helpers;
using HaveAVoice.Models.View;

namespace HaveAVoice.Services.Helpers {
    public class PrivacyHelper {
        public static bool IsAllowed(User aTargetUser, PrivacyAction aPrivacyAction) {
            bool myIsAllowed = false;
            UserInformationModel myUser = HAVUserInformationFactory.GetUserInformation();

            IEnumerable<string> myTargetUsersSettings = 
                (from p in aTargetUser.UserPrivacySettings.ToList<UserPrivacySetting>()
                  select p.PrivacySettingName);
            IEnumerable<Friend> myTargetUserFriends = aTargetUser.Friends.ToList<Friend>();

            if (aPrivacyAction == PrivacyAction.DisplayProfile) {
                if (myUser == null && HasPrivacySetting(myTargetUsersSettings, HAVPrivacySetting.Display_Profile_To_Not_Logged_In)) {
                    myIsAllowed = true;
                } else if (myUser != null && HasPrivacySetting(myTargetUsersSettings, HAVPrivacySetting.Display_Profile_To_Not_Friend)) {
                    myIsAllowed = true;
                } else if (myUser != null && RoleHelper.IsPolitician(myUser) && HasPrivacySetting(myTargetUsersSettings, HAVPrivacySetting.Display_Profile_To_Politician)) {
                    myIsAllowed = true;
                } else if (myUser != null && RoleHelper.IsPolitician(myUser) && HasPrivacySetting(myTargetUsersSettings, HAVPrivacySetting.Display_Profile_To_Politician)) {
                    myIsAllowed = true;
                }
            }
            
            return myIsAllowed;
        }

        private static bool HasPrivacySetting(IEnumerable<string> aPrivacySettings, HAVPrivacySetting aPrivacy) {
            return aPrivacySettings.Contains(aPrivacy.ToString());
        }
    }
}