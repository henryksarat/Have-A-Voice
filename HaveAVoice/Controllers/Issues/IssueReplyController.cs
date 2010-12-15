using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Repositories;
using HaveAVoice.Models.Validation;
using HaveAVoice.Services;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Controllers.Issues {
    public class IssueReplyController : HAVBaseController {
        private static string DELETE_REPLY = "Reply deleted succesfully.";
        private static string DELETE_REPLY_ERROR = "An error occurred while deleting the reply. Please try again.";
        private static string EDIT_SUCCESS = "Reply edited successfully!";
        private static string EDIT_REPLY_LOAD_ERROR = "Error while retrieving your original reply. Please try again.";
        private string EDIT_REPLY_ERROR = "Error editing the reply. Please try again.";
        private static string VIEW_COMMENT_ERROR = "An error occurred while trying to view the issue reply. Please try again.";
        private static string ISSUE_REPLY_COMMENT_ERROR = "Error posting the comment. Please try again.";
        private static string ISSUE_REPLY_COMMENT = "Comment posted!";
        private static string DISPOSITION_ERROR = "An error occurred while posting your disposition.";
        
        private IHAVIssueService theService;

        public IssueReplyController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
            theService = new HAVIssueService(new ModelStateWrapper(this.ModelState));
        }

        public IssueReplyController(IHAVIssueService aService, IHAVBaseService baseService)
            : base(baseService) {
            theService = aService;
        }

        public ActionResult Delete(int issueId, int issueReplyId) {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }
            UserInformationModel myUserInfo = GetUserInformatonModel();
            if (!HAVPermissionHelper.AllowedToPerformAction(myUserInfo, HAVPermission.Delete_Issue_Reply, HAVPermission.Delete_Any_Issue_Reply)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }

            try {
                theService.DeleteIssueReply(GetUserInformatonModel(), issueReplyId);
                TempData["Message"] = DELETE_REPLY;
            } catch (Exception myException) {
                LogError(myException, DELETE_REPLY_ERROR);
                TempData["Message"] = DELETE_REPLY_ERROR;
            }

            return RedirectToAction("View", "Issue", new { id = issueId });
        }

        public ActionResult Edit(int id) {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }
            UserInformationModel myUserInformation = GetUserInformatonModel();
            if (!HAVPermissionHelper.AllowedToPerformAction(myUserInformation, HAVPermission.Edit_Issue_Reply, HAVPermission.Edit_Any_Issue_Reply)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }

            try {
                IssueReply myIssueReply = theService.GetIssueReply(id);

                if (myUserInformation.Details.Id == myIssueReply.User.Id || HAVPermissionHelper.AllowedToPerformAction(myUserInformation, HAVPermission.Edit_Any_Issue_Reply)) {
                    return View("Edit", myIssueReply);
                } else {
                    return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
                }
            } catch (Exception myException) {
                LogError(myException, EDIT_REPLY_LOAD_ERROR);
                return SendToErrorPage(EDIT_REPLY_LOAD_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(IssueReply anIssueReply) {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }
            try {
                bool myResult = theService.EditIssueReply(GetUserInformatonModel(), anIssueReply);
                if (myResult) {
                    return SendToResultPage(EDIT_SUCCESS);
                }
            } catch (Exception myException) {
                LogError(myException, EDIT_REPLY_ERROR);
                ViewData["Message"] = EDIT_REPLY_ERROR;
            }

            return View("Edit", anIssueReply);
        }

        public ActionResult View(int id) {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }
            if (!HAVPermissionHelper.AllowedToPerformAction(GetUserInformatonModel(), HAVPermission.View_Issue_Reply)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }
            try {
                IssueReply myIssueReply = theService.GetIssueReply(id);
                if (myIssueReply == null) {
                    return SendToResultPage("That reply to the issue doesn't exist.");
                }

                IEnumerable<IssueReplyComment> myComments = theService.GetIssueReplyComments(id);
                IssueReplyDetailsModel issueReplyDetails = new IssueReplyDetailsModel(myIssueReply, myComments);
                return View("View", issueReplyDetails);
            } catch (Exception myException) {
                LogError(myException, VIEW_COMMENT_ERROR);
                return SendToErrorPage(VIEW_COMMENT_ERROR);
            }
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult View(IssueReplyDetailsModel issueReplyDetails) {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }
            UserInformationModel myUserInformation = GetUserInformatonModel();

            try {
                if (theService.CreateCommentToIssueReply(myUserInformation, issueReplyDetails)) {
                    ViewData["Message"] = ISSUE_REPLY_COMMENT;
                }
            } catch (Exception e) {
                LogError(e, ISSUE_REPLY_COMMENT_ERROR);
                ViewData["Message"] = ISSUE_REPLY_COMMENT_ERROR;
            }
            return View("View", issueReplyDetails);
        }

        public ActionResult Disposition(int issueReplyId, int disposition) {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }
            User aUser = GetUserInformaton();
            try {
                theService.AddIssueReplyDisposition(aUser, issueReplyId, disposition);
            } catch (Exception e) {
                LogError(e, DISPOSITION_ERROR);
                return SendToErrorPage(DISPOSITION_ERROR);
            }

            return RedirectToAction("Index");
        }

        public override ActionResult SendToResultPage(string aTitle, string aDetails) {
            return SendToResultPage(SiteSectionsEnum.IssueReply, aTitle, aDetails);
        }
    }
}
