using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Models.View;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Services;
using HaveAVoice.Repositories;
using HaveAVoice.Validation;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using System.ComponentModel.DataAnnotations;

namespace HaveAVoice.Controllers.Issues {
    public class IssueReplyCommentController : HAVBaseController {
        private static string DELETE_SUCCESS = "Comment deleted succesfully.";
        private static string EDIT_SUCCESS = "Comment edited successfully!";

        private static string DELETE_ERROR = "An error occurred while deleting the comment. Please try again.";
        private static string EDIT_LOAD_ERROR = "Error while retrieving your original comment. Please try again.";
        private static string EDIT_ERROR = "Error editing the comment. Please try again.";
        
        private IHAVIssueService theService;

        public IssueReplyCommentController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
            theService = new HAVIssueService(new ModelStateWrapper(this.ModelState));
        }

        public IssueReplyCommentController(IHAVIssueService aService, IHAVBaseService baseService)
            : base(baseService) {
            theService = aService;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id, int replyId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel myUserInfo = GetUserInformatonModel();
            if (!HAVPermissionHelper.AllowedToPerformAction(myUserInfo, HAVPermission.Delete_Issue_Reply_Comment, HAVPermission.Delete_Any_Issue_Reply_Comment)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }
            try {
                theService.DeleteIssueReplyComment(GetUserInformatonModel(), id);
                TempData["Message"] = DELETE_SUCCESS;
            } catch (Exception myException) {
                LogError(myException, DELETE_ERROR);
                TempData["Message"] = DELETE_ERROR;
            }

            return RedirectToAction("View", "IssueReply", new { id = replyId });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel myUserInformation = GetUserInformatonModel();
            if (!HAVPermissionHelper.AllowedToPerformAction(myUserInformation, HAVPermission.Edit_Issue_Reply_Comment, HAVPermission.Edit_Any_Issue_Reply_Comment)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }

            try {
                IssueReplyComment myComment = theService.GetIssueReplyComment(id);

                if (myUserInformation.Details.Id == myComment.User.Id || HAVPermissionHelper.AllowedToPerformAction(myUserInformation, HAVPermission.Edit_Any_Issue_Reply_Comment)) {
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
                    TempData["Message"] = EDIT_SUCCESS;
                    return RedirectToAction("View", "IssueReply", new { id = aCommentWrapper.IssueReplyId});
                }
            } catch (Exception myException) {
                LogError(myException, EDIT_ERROR);
                ViewData["Message"] = EDIT_ERROR;
            }

            return View("Edit", aCommentWrapper);
        }
        protected override ActionResult SendToResultPage(string aTitle, string aDetails) {
            return SendToResultPage(SiteSectionsEnum.IssueReplyComment, aTitle, aDetails);
        }
    }
}
