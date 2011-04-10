using System;
using System.Web.Mvc;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Repositories;
using HaveAVoice.Services;
using HaveAVoice.Services.Issues;
using Social.Admin.Helpers;
using Social.Generic.ActionFilters;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Generic.Services;

namespace HaveAVoice.Controllers.Issues {
    public class IssueReplyCommentController : HAVBaseController {
        private static string COMMENT_SUCCESS = "Comment posted!";
        private static string DELETE_SUCCESS = "Comment deleted succesfully.";
        private static string EDIT_SUCCESS = "Comment edited successfully!";

        private static string COMMENT_ERROR = "Error posting comment to the issue reply.";
        private static string DELETE_ERROR = "An error occurred while deleting the comment. Please try again.";
        private static string EDIT_LOAD_ERROR = "Error while retrieving your original comment. Please try again.";
        private static string EDIT_ERROR = "Error editing the comment. Please try again.";
        
        private IHAVIssueReplyCommentService theService;

        public IssueReplyCommentController() {
            theService = new HAVIssueReplyCommentService(new Social.Validation.ModelStateWrapper(this.ModelState));
        }

        public IssueReplyCommentController(IHAVIssueReplyCommentService aService) {
            theService = aService;
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(int issueReplyId, string comment) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();

            try {
                if (theService.CreateCommentToIssueReply(myUserInformation, issueReplyId, comment)) {
                    TempData["Message"] = MessageHelper.SuccessMessage(COMMENT_SUCCESS);
                    return RedirectToProfile();
                }
            } catch (Exception e) {
                LogError(e, COMMENT_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(COMMENT_ERROR);
            }

            return RedirectToProfile();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id, int replyId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInfo = GetUserInformatonModel();
            if (!PermissionHelper<User>.AllowedToPerformAction(myUserInfo, SocialPermission.Delete_Issue_Reply_Comment, SocialPermission.Delete_Any_Issue_Reply_Comment)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }
            try {
                theService.DeleteIssueReplyComment(GetUserInformatonModel(), id);
                TempData["Message"] = MessageHelper.SuccessMessage(DELETE_SUCCESS);
            } catch (Exception myException) {
                LogError(myException, DELETE_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(DELETE_ERROR);
            }

            return RedirectToAction("View", "IssueReply", new { id = replyId });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            if (!PermissionHelper<User>.AllowedToPerformAction(myUserInformation, SocialPermission.Edit_Issue_Reply_Comment, SocialPermission.Edit_Any_Issue_Reply_Comment)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }

            try {
                IssueReplyComment myComment = theService.GetIssueReplyComment(id);

                if (myUserInformation.Details.Id == myComment.User.Id || PermissionHelper<User>.AllowedToPerformAction(myUserInformation, SocialPermission.Edit_Any_Issue_Reply_Comment)) {
                    return View("Edit", IssueReplyCommentWrapper.Build(myComment));
                } else {
                    return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
                }
            } catch (Exception myException) {
                LogError(myException, EDIT_LOAD_ERROR);
                return SendToErrorPage(EDIT_LOAD_ERROR);
            }
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(IssueReplyCommentWrapper aCommentWrapper) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            
            try {
                bool myResult = theService.EditIssueReplyComment(GetUserInformatonModel(), aCommentWrapper.ToModel());
                if (myResult) {
                    TempData["Message"] = MessageHelper.SuccessMessage(EDIT_SUCCESS);
                    return RedirectToAction("View", "IssueReply", new { id = aCommentWrapper.IssueReplyId});
                }
            } catch (Exception myException) {
                LogError(myException, EDIT_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(EDIT_ERROR);
            }

            return View("Edit", aCommentWrapper);
        }
    }
}
