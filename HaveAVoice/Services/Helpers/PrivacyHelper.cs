﻿using System.Collections.Generic;
using System.Linq;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Models;
using Social.Admin.Helpers;
using Social.Generic.Helpers;
using Social.Generic.Models;
using HaveAVoice.Helpers;

namespace HaveAVoice.Services.Helpers {
    public class PrivacyHelper {
        public static bool IsAllowed(User aPrivacyUser, PrivacyAction aPrivacyAction) {
            UserInformationModel<User> myUser = HAVUserInformationFactory.GetUserInformation();
            return IsAllowed(aPrivacyUser, aPrivacyAction, myUser);
        }

        public static bool IsAllowed(User aPrivacyUser, PrivacyAction aPrivacyAction, UserInformationModel<User> aViewingUser) {
            bool myIsAllowed = true;
            if ((aViewingUser != null && (aViewingUser.Details.Id == aPrivacyUser.Id)) || (aPrivacyUser.Id == HAVConstants.PRIVATE_USER_ID)) {
                return true;
            }

            IEnumerable<string> myTargetUsersSettings =
                (from p in aPrivacyUser.UserPrivacySettings.ToList<UserPrivacySetting>()
                 select p.PrivacySettingName).ToList<string>();
            IEnumerable<Friend> myTargetUserFriends = aPrivacyUser.Friends.ToList<Friend>();

            if (aPrivacyAction == PrivacyAction.DisplayProfile) {
                if (aViewingUser == null || (aViewingUser != null && !FriendHelper.IsFriend(aPrivacyUser, aViewingUser.Details))) {
                    if (HasPrivacySetting(myTargetUsersSettings, SocialPrivacySetting.Display_Profile_To_Everyone)) {
                        myIsAllowed = true;
                    } else {
                        myIsAllowed = false;
                    }
                }
            }

            return myIsAllowed;
        }

        private static bool HasPrivacySetting(IEnumerable<string> aPrivacySettings, SocialPrivacySetting aPrivacy) {
            return aPrivacySettings.Contains(aPrivacy.ToString());
        }
    }
}