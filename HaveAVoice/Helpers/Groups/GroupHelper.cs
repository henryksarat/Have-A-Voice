using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using Social.Generic.Models;
using Social.Generic.Helpers;
using Social.Admin.Helpers;
using HaveAVoice.Services.Groups;
using Social.Validation;
using System.Web.Mvc;

namespace HaveAVoice.Helpers.Groups {
    public static class GroupHelper {
        public static bool IsAllowedToEdit(UserInformationModel<User> myUserInfo, Group aGroup) {
            int myAdminCount = aGroup.GroupMembers.Where(m => m.UserId == myUserInfo.Details.Id).Where(m => m.Administrator).Count<GroupMember>();
            return myAdminCount > 0 || PermissionHelper<User>.AllowedToPerformAction(myUserInfo, SocialPermission.Edit_Any_Group);
        }

        public static bool IsAdmin(User aUser, int aClubId) {
            IGroupService myGroupService = new GroupService(new ModelStateWrapper(new ModelStateDictionary()));
            return myGroupService.IsAdmin(aUser, aClubId);
        }

        public static bool IsMember(User aUser, int aClubId) {
            IGroupService myClubService = new GroupService(new ModelStateWrapper(new ModelStateDictionary()));
            return myClubService.IsApartOfGroup(aUser.Id, aClubId);
        }

        public static bool IsPending(User aUser, int aClubId) {
            IGroupService myClubService = new GroupService(new ModelStateWrapper(new ModelStateDictionary()));
            return myClubService.IsPendingApproval(aUser.Id, aClubId);
        }
    }
}