using Social.Admin.Helpers;
using Social.Generic.Helpers;
using Social.Generic.Models;
using UniversityOfMe.Models;

namespace UniversityOfMe.Helpers.Functionality {
    public static class ClassBoardHelper {
        public static bool IsAllowedToDelete(UserInformationModel<User> aCurrentUser, ClassBoard aBoardMessage) {
            return aCurrentUser != null && (aCurrentUser.Details.Id == aBoardMessage.UserId 
                || PermissionHelper<User>.AllowedToPerformAction(aCurrentUser, SocialPermission.Delete_Any_Class_Board));
        }
    }
}