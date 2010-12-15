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
        private static string DELETE_COMMENT = "Comment deleted succesfully.";
        private static string DELETE_COMMENT_ERROR = "An error occurred while deleting the comment. Please try again.";
        private static string EDIT_SUCCESS = "Comment edited successfully!";
        private static string EDIT_COMMENT_LOAD_ERROR = "Error while retrieving your original comment. Please try again.";
        private string EDIT_COMMENT_ERROR = "Error editing the comment. Please try again.";
        
        private IHAVIssueService theService;

        public IssueReplyCommentController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
            theService = new HAVIssueService(new ModelStateWrapper(this.ModelState));
        }

        public IssueReplyCommentController(IHAVIssueService aService, IHAVBaseService baseService)
            : base(baseService) {
            theService = aService;
        }

        public ActionResult Delete(int issueReplyId, int issueReplyCommentId) {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }
            UserInformationModel myUserInfo = GetUserInformatonModel();
            if (!HAVPermissionHelper.AllowedToPerformAction(myUserInfo, HAVPermission.Delete_Issue_Reply_Comment, HAVPermission.Delete_Any_Issue_Reply_Comment)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }
            try {
                theService.DeleteIssueReplyComment(GetUserInformatonModel(), issueReplyCommentId);
                TempData["Message"] = DELETE_COMMENT;
            } catch (Exception myException) {
                LogError(myException, DELETE_COMMENT_ERROR);
                TempData["Message"] = DELETE_COMMENT_ERROR;
            }

            return RedirectToAction("View", "IssueReply", new { id = issueReplyId });
        }

        public ActionResult Edit(int id) {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }
            UserInformationModel myUserInformation = GetUserInformatonModel();
            if (!HAVPermissionHelper.AllowedToPerformAction(myUserInformation, HAVPermission.Edit_Issue_Reply_Comment, HAVPermission.Edit_Any_Issue_Reply_Comment)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }

            try {
                IssueReplyComment myComment = theService.GetIssueReplyComment(id);

                if (myUserInformation.Details.Id == myComment.User.Id || HAVPermissionHelper.AllowedToPerformAction(myUserInformation, HAVPermission.Edit_Any_Issue_Reply_Comment)) {
                    return View("Edit", myComment);
                } else {
                    return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
                }
            } catch (Exception myException) {
                LogError(myException, EDIT_COMMENT_LOAD_ERROR);
                return SendToErrorPage(EDIT_COMMENT_LOAD_ERROR);
            }
        }

        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(IssueReplyComment aComment) {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }
            
            try {
                bool myResult = theService.EditIssueReplyComment(GetUserInformatonModel(), aComment);
                if (myResult) {
                    return SendToResultPage(EDIT_SUCCESS);
                }
            } catch (Exception myException) {
                LogError(myException, EDIT_COMMENT_ERROR);
                ViewData["Message"] = EDIT_COMMENT_ERROR;
            }

            return View("Edit", aComment);
        }
        public override ActionResult SendToResultPage(string aTitle, string aDetails) {
            return SendToResultPage(SiteSectionsEnum.IssueReplyComment, aTitle, aDetails);
        }
    }
}
