using System;
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
        public static bool IsAllowed(User aPrivacyUser, PrivacyAction aPrivacyAction) {
            UserInformationModel myUser = HAVUserInformationFactory.GetUserInformation();
            return IsAllowed(aPrivacyUser, aPrivacyAction, myUser);
        }

        public static bool IsAllowed(User aPrivacyUser, PrivacyAction aPrivacyAction, UserInformationModel aViewingUser) {
            bool myIsAllowed = true;
            if (aViewingUser != null && (aViewingUser.Details.Id == aPrivacyUser.Id)) {
                return true;
            }

            IEnumerable<string> myTargetUsersSettings =
                (from p in aPrivacyUser.UserPrivacySettings.ToList<UserPrivacySetting>()
                 select p.PrivacySettingName).ToList<string>();
            IEnumerable<Friend> myTargetUserFriends = aPrivacyUser.Friends.ToList<Friend>();

            if (aPrivacyAction == PrivacyAction.DisplayProfile) {
                if (aViewingUser == null) {
                    if (HasPrivacySetting(myTargetUsersSettings, HAVPrivacySetting.Display_Profile_To_Not_Logged_In)) {
                        myIsAllowed = true;
                    } else {
                        myIsAllowed = false;
                    }
                }

                if (aViewingUser != null && !FriendHelper.IsFriend(aPrivacyUser, aViewingUser.Details)) {
                    if (HasPrivacySetting(myTargetUsersSettings, HAVPrivacySetting.Display_Profile_To_Not_Friend)) {
                        myIsAllowed = true;
                    } else {
                        myIsAllowed = false;
                    }
                }
                if (aViewingUser != null && HAVPermissionHelper.HasPermission(aViewingUser, HAVPermission.Confirmed_Politician)) {
                    if (HasPrivacySetting(myTargetUsersSettings, HAVPrivacySetting.Display_Profile_To_Politician)) {
                        myIsAllowed = true;
                    } else {
                        myIsAllowed = false;
                    }
                }
                if (aViewingUser != null && HAVPermissionHelper.HasPermission(aViewingUser, HAVPermission.Confirmed_Political_Candidate)) {
                    if (HasPrivacySetting(myTargetUsersSettings, HAVPrivacySetting.Display_Profile_To_Political_Candidate)) {
                        myIsAllowed = true;
                    } else {
                        myIsAllowed = false;
                    }
                }
            }

            return myIsAllowed;
        }

        private static bool HasPrivacySetting(IEnumerable<string> aPrivacySettings, HAVPrivacySetting aPrivacy) {
            return aPrivacySettings.Contains(aPrivacy.ToString());
        }
    }
}