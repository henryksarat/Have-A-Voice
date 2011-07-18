using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using Social.Generic.Models;
using Social.Generic.Helpers;
using Social.Admin.Helpers;

namespace HaveAVoice.Helpers.Groups {
    public static class GroupHelper {
        public static bool IsAllowedToEdit(UserInformationModel<User> myUserInfo, Group aGroup) {
            int myAdminCount = aGroup.GroupMembers.Where(m => m.UserId == myUserInfo.Details.Id).Where(m => m.Administrator).Count<GroupMember>();
            return myAdminCount > 0 || PermissionHelper<User>.AllowedToPerformAction(myUserInfo, SocialPermission.Edit_Any_Group);
        }
    }
}