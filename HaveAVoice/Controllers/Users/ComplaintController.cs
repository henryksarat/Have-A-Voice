﻿
using System;
using System.Web.Mvc;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Helpers;
using HaveAVoice.Validation;
using HaveAVoice.Repositories;
using HaveAVoice.Services;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Controllers.ActionFilters;
using HaveAVoice.Services.Issues;

namespace HaveAVoice.Controllers.Users {
    public class ComplaintController : HAVBaseController {
        private IHAVComplaintService theService;
        private IHAVUserRetrievalService theUserRetrievalService;
        private IHAVIssueService theIssueService;
        private IHAVIssueReplyService theIssueReplyService;
        private IHAVIssueReplyCommentService theIssueReplyCommentService;
        private IHAVPhotoService thePhotoService;

        public ComplaintController()
            : base(new HAVBaseService(new HAVBaseRepository())) {
             ModelStateWrapper myModelWrapper = new ModelStateWrapper(this.ModelState);
            theService = new HAVComplaintService(myModelWrapper);
            theUserRetrievalService = new HAVUserRetrievalService();
            theIssueService = new HAVIssueService(myModelWrapper);
            theIssueReplyService = new HAVIssueReplyService(myModelWrapper);
            theIssueReplyCommentService = new HAVIssueReplyCommentService(myModelWrapper);
            thePhotoService = new HAVPhotoService();
        }

        public ComplaintController(IHAVComplaintService aService, IHAVBaseService aBaseService, 
                                               IHAVUserRetrievalService aUserRetrievalService, IHAVIssueService aIssueService, 
                                               IHAVPhotoService aPhotoService) : base(aBaseService) {
            theService = aService;
            theUserRetrievalService = aUserRetrievalService;
            theIssueService = aIssueService;
            thePhotoService = aPhotoService;
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Create(string complaintType, int sourceId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            User myUser = GetUserInformaton();
            ComplaintType myType = (ComplaintType)Enum.Parse(typeof(ComplaintType), complaintType);
            ComplaintModel.Builder myBuilder = new ComplaintModel.Builder(sourceId, myType);
            try {
                ComplaintHelper.FillComplaintModelBuilder(myBuilder, theUserRetrievalService, theIssueService, theIssueReplyService, theIssueReplyCommentService, thePhotoService);
            } catch (Exception e) {
                LogError(e, String.Format("Unable get complaint info. [complaintModel={0}]", myBuilder.Build().ToString()));
                return SendToErrorPage("Unable to get the necessary information for the complaint. Please try again.");
            }

            return View("Create", myBuilder.Build());
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(ComplaintType complaintType, string complaint, int sourceId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                User myUser = GetUserInformaton();
                switch (complaintType) {
                    case ComplaintType.Issue:
                        if (theService.IssueComplaint(myUser, complaint, sourceId)) {
                             return SuccessfulComplaint();
                        }
                        break;
                    case ComplaintType.IssueReply:
                        if (theService.IssueReplyComplaint(myUser, complaint, sourceId)) {
                             return SuccessfulComplaint();
                        }
                        break;
                    case ComplaintType.IssueReplyComment:
                        if (theService.IssueReplyCommentComplaint(myUser, complaint, sourceId)) {
                             return SuccessfulComplaint();
                        }
                        break;
                    case ComplaintType.ProfileComplaint:
                        if (theService.ProfileComplaint(myUser, complaint, sourceId)) {
                             return SuccessfulComplaint();
                        }
                        break;
                    case ComplaintType.PhotoComplaint:
                        if (theService.PhotoComplaint(myUser, complaint, sourceId)) {
                             return SuccessfulComplaint();
                        }
                        break;
                }
            } catch (Exception e) {
                LogError(e, "Error logging report. Please try again.");
                TempData["Message"] = MessageHelper.ErrorMessage("Error logging report. Please try again.");
                TempData["ComplaintBody"] = complaint;
            }

            return RedirectToAction("Create", new { complaintType = complaintType, sourceId = sourceId });
        }

        private ActionResult SuccessfulComplaint() {
            return SendToResultPage("Complaint logged successfully!");
        }
    }
}
