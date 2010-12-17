﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Repositories;
using HaveAVoice.Validation;
using HaveAVoice.Services;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.Wrappers;

namespace HaveAVoice.Controllers.Issues {
    public class IssueReplyController : HAVBaseController {
        private static string EDIT_SUCCESS = "Reply edited successfully!";
        private static string DELETE_SUCCESS = "Reply deleted succesfully.";
        private static string POST_COMMENT_SUCCESS = "Comment posted!";
        private static string DISPOSITION_SUCCESS = "Disposition set!";

        private static string NON_EXISTENT = "That reply to the issue doesn't exist.";
        private static string VIEW_ERROR = "An error occurred while trying to view the issue reply. Please try again.";
        private static string EDIT_LOAD_ERROR = "Error while retrieving your original reply. Please try again.";
        private static string EDIT_ERROR = "Error editing the reply. Please try again.";
        private static string DELETE_ERROR = "An error occurred while deleting the reply. Please try again.";
        private static string POST_COMMENT_ERROR = "Error posting the comment. Please try again.";
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

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id, int issueId) {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }
            UserInformationModel myUserInfo = GetUserInformatonModel();
            if (!HAVPermissionHelper.AllowedToPerformAction(myUserInfo, HAVPermission.Delete_Issue_Reply, HAVPermission.Delete_Any_Issue_Reply)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }

            try {
                theService.DeleteIssueReply(GetUserInformatonModel(), id);
                TempData["Message"] = DELETE_SUCCESS;
            } catch (Exception myException) {
                LogError(myException, DELETE_ERROR);
                TempData["Message"] = DELETE_ERROR;
            }

            return RedirectToAction("View", "Issue", new { id = issueId });
        }

        [AcceptVerbs(HttpVerbs.Get)]
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
                return RedirectToAction("Login", "User");
            }
            try {
                bool myResult = theService.EditIssueReply(GetUserInformatonModel(), aReplyWrapper.ToModel());
                if (myResult) {
                    TempData["Message"] = EDIT_SUCCESS;
                    return RedirectToAction("View", new { id = aReplyWrapper.Id });
                }
            } catch (Exception myException) {
                LogError(myException, EDIT_ERROR);
                ViewData["Message"] = EDIT_ERROR;
            }

            return View("Edit", aReplyWrapper);
        }

        [AcceptVerbs(HttpVerbs.Get)]
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
                    return SendToResultPage(NON_EXISTENT);
                }

                IEnumerable<IssueReplyComment> myComments = theService.GetIssueReplyComments(id);
                IssueReplyDetailsModel myIssueModelDetails = new IssueReplyDetailsModel(myIssueReply, myComments);
                return View("View", myIssueModelDetails);
            } catch (Exception myException) {
                LogError(myException, VIEW_ERROR);
                return SendToErrorPage(VIEW_ERROR);
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
                    TempData["Message"] = POST_COMMENT_SUCCESS;
                    return RedirectToAction("View", issueReplyDetails);
                }
            } catch (Exception e) {
                LogError(e, POST_COMMENT_ERROR);
                ViewData["Message"] = POST_COMMENT_ERROR;
            }
            return View("View", issueReplyDetails);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Disposition(int id, int issueId, int disposition) {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }
            User aUser = GetUserInformaton();
            try {
                theService.AddIssueReplyDisposition(aUser, id, disposition);
                TempData["Message"] = DISPOSITION_SUCCESS;
            } catch (Exception e) {
                LogError(e, DISPOSITION_ERROR);
                TempData["Message"] = DISPOSITION_ERROR;
                return SendToErrorPage(DISPOSITION_ERROR);
            }

            return RedirectToAction("View", "Issue", new { id = issueId });
        }

        public override ActionResult SendToResultPage(string aTitle, string aDetails) {
            return SendToResultPage(SiteSectionsEnum.IssueReply, aTitle, aDetails);
        }
    }
}
