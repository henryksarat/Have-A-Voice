using System;
using System.Web.Mvc;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Services;
using HaveAVoice.Services.Issues;
using Social.Admin.Helpers;
using Social.Generic.ActionFilters;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;
using Social.Generic.Services;

namespace HaveAVoice.Controllers.Issues {
    public class IssueReplyController : HAVBaseController {
        private static string EDIT_SUCCESS = "Reply edited successfully!";
        private static string DELETE_SUCCESS = "Reply deleted succesfully.";
        private static string POST_COMMENT_SUCCESS = "Comment posted!";
        private static string DISPOSITION_SUCCESS = "Disposition set!";
        private static string REPLY_SUCCESS = "Reply posted successfully!";

        private const string REPLY_ERROR = "Error posting issue reply. Please try again.";
        private static string NON_EXISTENT = "That reply to the issue doesn't exist.";
        private static string VIEW_ERROR = "An error occurred while trying to view the issue reply. Please try again.";
        private static string EDIT_LOAD_ERROR = "Error while retrieving your original reply. Please try again.";
        private static string EDIT_ERROR = "Error editing the reply. Please try again.";
        private static string DELETE_ERROR = "An error occurred while deleting the reply. Please try again.";
        private static string POST_COMMENT_ERROR = "Error posting the comment. Please try again.";
        private static string DISPOSITION_ERROR = "An error occurred while posting your disposition.";

        private IHAVIssueReplyService theIssueReplyService;
        private IHAVIssueReplyCommentService theIssueReplyCommentService;

        public IssueReplyController() {
                IValidationDictionary myModelState = new ModelStateWrapper(this.ModelState);
                theIssueReplyService = new HAVIssueReplyService(myModelState);
                theIssueReplyCommentService = new HAVIssueReplyCommentService(myModelState);
        }

        public IssueReplyController(IHAVIssueReplyService aService) {
            theIssueReplyService = aService;
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(int issueId, string reply, int disposition, bool anonymous) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();

            try {
                if (theIssueReplyService.CreateIssueReply(myUserInformation, issueId, reply, disposition, anonymous)) {
                    TempData["Message"] += MessageHelper.SuccessMessage(REPLY_SUCCESS);
                    return RedirectToProfile();
                }
            } catch (Exception e) {
                LogError(e, REPLY_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(REPLY_ERROR);
            }
            return RedirectToProfile();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id, int issueId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInfo = GetUserInformatonModel();
            if (!PermissionHelper<User>.AllowedToPerformAction(myUserInfo, SocialPermission.Delete_Issue_Reply, SocialPermission.Delete_Any_Issue_Reply)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }

            try {
                theIssueReplyService.DeleteIssueReply(GetUserInformatonModel(), id);
                TempData["Message"] += MessageHelper.SuccessMessage(DELETE_SUCCESS);
            } catch (Exception myException) {
                LogError(myException, DELETE_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(DELETE_ERROR);
            }

            return RedirectToAction("RedirectToDetails", "Issue", new { id = issueId });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            if (!PermissionHelper<User>.AllowedToPerformAction(myUserInformation, SocialPermission.Edit_Issue_Reply, SocialPermission.Edit_Any_Issue_Reply)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }

            try {
                IssueReply myIssueReply = theIssueReplyService.GetIssueReply(id);

                if (myUserInformation.Details.Id == myIssueReply.User.Id || PermissionHelper<User>.AllowedToPerformAction(myUserInformation, SocialPermission.Edit_Any_Issue_Reply)) {
                    return View("Edit", IssueReplyWrapper.Build(myIssueReply));
                } else {
                    return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
                }
            } catch (Exception myException) {
                LogError(myException, EDIT_LOAD_ERROR);
                return SendToErrorPage(EDIT_LOAD_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(IssueReplyWrapper aReplyWrapper) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                bool myResult = theIssueReplyService.EditIssueReply(GetUserInformatonModel(), aReplyWrapper.ToModel());
                if (myResult) {
                    TempData["Message"] += MessageHelper.SuccessMessage(EDIT_SUCCESS);
                    return RedirectToAction("View", new { id = aReplyWrapper.Id });
                }
            } catch (Exception myException) {
                LogError(myException, EDIT_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(EDIT_ERROR);
            }

            return View("Edit", aReplyWrapper);
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Details(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInfo = GetUserInformatonModel();

            if (!PermissionHelper<User>.AllowedToPerformAction(GetUserInformatonModel(), SocialPermission.View_Issue_Reply)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }
            try {
                IssueReply myIssueReply = theIssueReplyService.GetIssueReply(myUserInfo.Details, id);
                if (myIssueReply == null) {
                    return SendToResultPage(NON_EXISTENT);
                }
                return View("Details", myIssueReply);
            } catch (Exception myException) {
                LogError(myException, VIEW_ERROR);
                return SendToErrorPage(VIEW_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Details(int issueReplyId, string comment) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            try {
                if (theIssueReplyCommentService.CreateCommentToIssueReply(myUserInformation, issueReplyId, comment)) {
                    TempData["Message"] += MessageHelper.SuccessMessage(POST_COMMENT_SUCCESS);
                    return RedirectToAction("Details", new { id = issueReplyId });
                }
            } catch (Exception e) {
                LogError(e, POST_COMMENT_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(POST_COMMENT_ERROR);
            }

            return RedirectToAction("Details", new { id = issueReplyId });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Disposition(int id, int issueId, Disposition disposition, SiteSection section, int sourceId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User aUser = GetUserInformaton();
            try {
                bool myResult = theIssueReplyService.AddIssueReplyStance(aUser, id, (int)disposition);
                if (!myResult) {
                    return SendToErrorPage("You can only provide a disposition towards a person's reply to an issue once.");
                }
                TempData["Message"] += MessageHelper.SuccessMessage(DISPOSITION_SUCCESS);
            } catch (Exception e) {
                LogError(e, DISPOSITION_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(DISPOSITION_ERROR);
                return SendToErrorPage(DISPOSITION_ERROR);
            }

            if (section == SiteSection.Profile) {
                return RedirectToAction("Show", "Profile", new { id = sourceId });
            } else if (section == SiteSection.MyProfile) {
                return RedirectToAction("Show", "Profile");
            } else if (section == SiteSection.IssueActivity) {
                return RedirectToAction("IssueActivity", "Profile", new { id = sourceId });
            } else if (section == SiteSection.MyIssueActivity) {
                return RedirectToAction("IssueActivity", "Profile");
            } else if (section == SiteSection.IssueReply) {
                return RedirectToAction("Details", new { id = sourceId });
            } else {
                return RedirectToAction("RedirectToDetails", "Issue", new { id = issueId });
            }
        }
    }
}
