using Social.Admin.Helpers;
using Social.Generic.Helpers;
using Social.Generic.Models;
using UniversityOfMe.Models;

namespace UniversityOfMe.Helpers.Functionality {
    public static class BoardReplyHelper {
        public static bool IsAllowedToDelete(UserInformationModel<User> aCurrentUser, BoardReply aBoardReply) {
            return aCurrentUser.Details.Id == aBoardReply.Board.OwnerUserId
                || aCurrentUser.Details.Id == aBoardReply.UserId
                || PermissionHelper<User>.AllowedToPerformAction(aCurrentUser, SocialPermission.Delete_Any_Board_Reply);
        }
    }
}