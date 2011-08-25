using Social.Admin.Helpers;
using Social.Generic.Helpers;
using Social.Generic.Models;
using UniversityOfMe.Models;

namespace UniversityOfMe.Helpers.Functionality {
    public static class UserStatusHelper {
        public static bool IsAllowedToDelete(UserInformationModel<User> aCurrentUser, UserStatus aUserStatus) {
            return aCurrentUser != null && aCurrentUser.Details.Id == aUserStatus.UserId;
        }
    }
}