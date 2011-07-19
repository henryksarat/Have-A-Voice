using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Services.Groups;
using Social.Admin.Helpers;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;

namespace HaveAVoice.Helpers.Groups {
    public static class GroupHelper {
        public static bool IsAllowedToEdit(UserInformationModel<User> myUserInfo, Group aGroup) {
            int myAdminCount = aGroup.GroupMembers.Where(m => m.MemberUserId == myUserInfo.Details.Id).Where(m => m.Administrator).Count<GroupMember>();
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

        public static IEnumerable<GroupAdminFeed> GetAdminFeed(Group aGroup, int aLimit) {
            IEnumerable<GroupMember> myGroupMembers = aGroup.GroupMembers.OrderByDescending(gm => gm.DateTimeStamp);
            List<GroupAdminFeed> myFeed = new List<GroupAdminFeed>();

            myFeed.AddRange(from gm in myGroupMembers.Where(gm2 => gm2.Approved == HAVConstants.PENDING)
                            select new GroupAdminFeed() {
                                GroupMemberId = gm.Id,
                                GroupId = gm.GroupId,
                                MemberUser = gm.MemberUser,
                                DateTimeStamp = gm.DateTimeStamp,
                                HasDetails = true,
                                Status = Status.Pending,
                                FeedType = FeedType.Member
                            });
            myFeed.AddRange(from gm in myGroupMembers.Where(gm2 => gm2.Approved == HAVConstants.DENIED)
                            select new GroupAdminFeed() {
                                GroupMemberId = gm.Id,
                                GroupId = gm.GroupId,
                                MemberUser = gm.MemberUser,
                                AdminUser = gm.DeniedByUser,
                                DateTimeStamp = gm.DeniedByDateTimeStamp,
                                Status = Status.Denied,
                                FeedType = FeedType.Member
                            });
            myFeed.AddRange(from gm in myGroupMembers.Where(gm2 => gm2.Approved == HAVConstants.APPROVED).Where(gm2 => !gm2.AutoAccepted)
                            select new GroupAdminFeed() {
                                GroupMemberId = gm.Id,
                                GroupId = gm.GroupId,
                                MemberUser = gm.MemberUser,
                                AdminUser = gm.ApprovedByUser,
                                DateTimeStamp = gm.ApprovedDateTimeStamp,
                                Status = Status.Approved,
                                FeedType = FeedType.Member
                            });
            myFeed.AddRange(from gm in myGroupMembers.Where(gm2 => gm2.Approved == HAVConstants.APPROVED).Where(gm2 => gm2.AutoAccepted)
                            select new GroupAdminFeed() {
                                GroupMemberId = gm.Id,
                                GroupId = gm.GroupId,
                                MemberUser = gm.MemberUser,
                                DateTimeStamp = gm.DateTimeStamp,
                                Status = Status.Approved,
                                FeedType = FeedType.AutoAcceptedMember
                            });
            if (aGroup.LastEditedByUser != null) {
                myFeed.Add(new GroupAdminFeed() {
                    AdminUser = aGroup.LastEditedByUser,
                    DateTimeStamp = aGroup.LastEditedDateTimeStamp,
                    FeedType = FeedType.Edited
                });
            }

            if (aGroup.DeactivatedByUser != null) {
                myFeed.Add(new GroupAdminFeed() {
                    AdminUser = aGroup.DeactivatedByUser,
                    DateTimeStamp = aGroup.DeactivatedDateTimeStamp,
                    FeedType = FeedType.Deactivated
                });
            }

            return myFeed.OrderByDescending(f => f.DateTimeStamp).Take<GroupAdminFeed>(aLimit);
        }
    }
}