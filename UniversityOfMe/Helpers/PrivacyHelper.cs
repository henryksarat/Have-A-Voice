using Social.Generic.Helpers;
using UniversityOfMe.Models;
using Social.Generic.Models;
using System.Collections.Generic;
using System.Linq;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Helpers {
    public class PrivacyHelper {
        public static bool PrivacyAllows(User aUser, SocialPrivacySetting aPrivacySetting) {
            foreach (UserPrivacySetting myUserPrivacySetting in aUser.UserPrivacySettings) {
                if (myUserPrivacySetting.PrivacySettingName.Equals(aPrivacySetting.ToString())) {
                    return true;
                }

            }

            return false;
        }

        public static bool IsAllowed(User aPrivacyUser, PrivacyAction aPrivacyAction, UserInformationModel<User> aViewingUser) {
            bool myIsAllowed = true;

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