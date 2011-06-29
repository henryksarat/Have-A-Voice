using Social.Admin.Helpers;
using Social.Generic.Helpers;
using Social.Generic.Models;
using UniversityOfMe.Models;

namespace UniversityOfMe.Helpers.Functionality {
    public static class ClassBoardReplyHelper {
        public static bool IsAllowedToDelete(UserInformationModel<User> aCurrentUser, ClassBoardReply aBoardReply) {
            return aCurrentUser.Details.Id == aBoardReply.UserId
                || PermissionHelper<User>.AllowedToPerformAction(aCurrentUser, SocialPermission.Delete_Any_Class_Board_Reply);
        }
    }
}