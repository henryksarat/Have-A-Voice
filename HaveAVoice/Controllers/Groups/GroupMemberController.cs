using System;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Generic.Models;
using Social.Users.Services;
using Social.Validation;
using System.Collections.Generic;
using HaveAVoice.Controllers;
using HaveAVoice.Services.Groups;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Models;
using Social.Generic.Helpers;

namespace HaveAVoice.Controllers.Groups {
    public class GroupMemberController : HAVBaseController {
        private const string PENDING = "You are already pending to join the group.";
        private const string APPROVED = "You are already apart of the group.";

        private const string USER_REMVOED = "User removed successfully!";
        private const string USER_QUIT = "You have left the group successfull!";
        private const string CANCEL_REQUEST = "Your request to join the group has been cancelled.";
        private const string GROUP_MEMBER_APPROVED = "The group member has been approved to join the group!";
        private const string GROUP_MEMBER_DENIED = "The group member has been denied to join the group!";
        private const string GROUP_MEMBER_EDITED = "The group member has been edited!";

        private const string CANCEL_REQUEST_ERROR = "An error occurred while canceling your request to join the group. Please try again.";
        private const string REQUEST_ERROR = "An error occurred while submitted your request. Please try again.";
        private const string REMOVE_ERROR = "Error while removing the member from the group. Please try again.";
        private const string GROUP_MEMBER_ERROR = "Unable to get the group member information. Please try again.";
        private const string GROUP_MEMBER_VERDICT_ERROR = "An error occurred while approving or denying the group member.";
        private const string GROUP_MEMBER_LIST = "An error occurred while getting the list of members for the group.";
        private const string GROUP_MEMBER_EDITED_FAIL = "An error occurred while editing the group member!";

        IValidationDictionary theValidationDictionary;
        IGroupService theGroupService;

        public GroupMemberController() {
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theGroupService = new GroupService(theValidationDictionary);
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Cancel(int groupId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUserInformation = GetUserInformatonModel();

                theGroupService.CancelRequestToJoin(myUserInformation, groupId);
                TempData["Message"] += MessageHelper.SuccessMessage(CANCEL_REQUEST);
            } catch (Exception myException) {
                LogError(myException, REMOVE_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(REMOVE_ERROR);
            }

            return RedirectToAction("Details", "Group", new { id = groupId });
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Details(int groupId, int groupMemberId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUserInformation = GetUserInformatonModel();
                GroupMember myGroupMember = theGroupService.GetGroupMember(myUserInformation, groupMemberId);
                if (myGroupMember != null) {
                    return View("Details", myGroupMember);
                }
                TempData["Message"] += MessageHelper.ErrorMessage(GROUP_MEMBER_ERROR);
            } catch (Exception myException) {
                LogError(myException, GROUP_MEMBER_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(GROUP_MEMBER_ERROR);
            }

            return RedirectToAction("Details", "Group", new { id = groupId });
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Edit(int groupMemberId, int groupId, string title, bool administrator) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUserInformation = GetUserInformatonModel();
                bool myResult = theGroupService.EditGroupMember(myUserInformation, groupId, groupMemberId, title, administrator);
                if (myResult) {
                    TempData["Message"] += MessageHelper.NormalMessage(GROUP_MEMBER_EDITED);
                }
                return RedirectToAction("Details", "Group", new { id = groupId });
            } catch (Exception myException) {
                LogError(myException, GROUP_MEMBER_EDITED_FAIL);
                TempData["Message"] += MessageHelper.ErrorMessage(GROUP_MEMBER_EDITED_FAIL);
                theValidationDictionary.ForceModleStateExport();
                return RedirectToAction("Details", new { groupId = groupId, groupMemberId = groupMemberId });
            }
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult List(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUserInformation = GetUserInformatonModel();
                IEnumerable<GroupMember> myGroupMembers = theGroupService.GetActiveGroupMembers(id);
                return View("List", myGroupMembers);
            } catch (Exception myException) {
                LogError(myException, GROUP_MEMBER_LIST);
                TempData["Message"] += MessageHelper.ErrorMessage(GROUP_MEMBER_LIST);
                return RedirectToAction("List", "Group", new { id = id });
            }
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Quit(int groupId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUser = GetUserInformatonModel();
            return RemoveMember(myUser, groupId, myUser.Details.Id, USER_QUIT);
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Remove(int groupId, int userRemovingId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUser = GetUserInformatonModel();
            return RemoveMember(myUser, groupId, userRemovingId, USER_REMVOED);
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult RequestToJoin(int groupId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUserInformation = GetUserInformatonModel();

                if (theGroupService.IsPendingApproval(myUserInformation.Details.Id, groupId)) {
                    TempData["Message"] += MessageHelper.NormalMessage(PENDING);
                } else if (theGroupService.IsApartOfGroup(myUserInformation.Details.Id, groupId)) {
                    TempData["Message"] += MessageHelper.NormalMessage(APPROVED);
                } else {
                    string myMessage;
                    theGroupService.RequestToJoinGroup(myUserInformation, groupId, out myMessage);
                    TempData["Message"] += MessageHelper.SuccessMessage(myMessage);
                }
            } catch (Exception myException) {
                LogError(myException, REMOVE_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(REQUEST_ERROR);
            }

            return RedirectToAction("Details", "Group", new { id = groupId });
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Verdict(int groupMemberId, int groupId, StatusAction approved, string title, bool administrator) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUserInformation = GetUserInformatonModel();
                bool myResult = false;
                if (approved == StatusAction.Approve) {
                    myResult = theGroupService.ApproveGroupMember(myUserInformation, groupMemberId, title, administrator);
                    if (myResult) {
                        TempData["Message"] += MessageHelper.NormalMessage(GROUP_MEMBER_APPROVED);
                    }
                } else {
                    theGroupService.DenyGroupMember(myUserInformation, groupMemberId);
                    myResult = true;
                    TempData["Message"] += MessageHelper.NormalMessage(GROUP_MEMBER_DENIED);
                }

                if (myResult) {
                    return RedirectToAction("Details", "Group", new { id = groupId });
                }
            } catch (Exception myException) {
                LogError(myException, GROUP_MEMBER_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(GROUP_MEMBER_VERDICT_ERROR);
            }

            //Force for radio button value export
            theValidationDictionary.ForceModleStateExport();

            return RedirectToAction("Details", new { groupId = groupId, groupMemberId = groupMemberId });
        }

        private ActionResult RemoveMember(UserInformationModel<User> aUserInfo, int aGroupId, int aUserRemovingId, string aMessage) {
            try {
                bool myResult = theGroupService.RemoveGroupMember(aUserInfo, aUserRemovingId, aGroupId);

                if (myResult) {
                    TempData["Message"] += MessageHelper.SuccessMessage(aMessage);
                }
            } catch (Exception myException) {
                LogError(myException, REMOVE_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(REMOVE_ERROR);
            }

            return RedirectToAction("Details", "Group", new { id = aGroupId });
        }
    }
}
