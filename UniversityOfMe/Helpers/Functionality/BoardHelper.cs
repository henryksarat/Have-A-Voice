using Social.Admin.Helpers;
using Social.Generic.Helpers;
using Social.Generic.Models;
using UniversityOfMe.Models;

namespace UniversityOfMe.Helpers.Functionality {
    public static class BoardHelper {
        public static bool IsAllowedToDelete(UserInformationModel<User> aCurrentUser, Board aBoardMessage) {
            return aCurrentUser !=null 
                && (aCurrentUser.Details.Id == aBoardMessage.OwnerUserId 
                || aCurrentUser.Details.Id == aBoardMessage.PostedUserId 
                || PermissionHelper<User>.AllowedToPerformAction(aCurrentUser, SocialPermission.Delete_Any_Board_Message));
        }
    }
}