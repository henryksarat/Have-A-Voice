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

namespace HaveAVoice.Controllers.Users
{
    public class ComplaintController : HAVBaseController {
        private IHAVComplaintService theService;
        private IHAVUserRetrievalService theUserRetrievalService;
        private IHAVIssueService theIssueService;
        private IHAVPhotoService thePhotoService;

        public ComplaintController()
            : base(new HAVBaseService(new HAVBaseRepository())) {
             ModelStateWrapper myModelWrapper = new ModelStateWrapper(this.ModelState);
            theService = new HAVComplaintService(myModelWrapper);
            theUserRetrievalService = new HAVUserRetrievalService();
            theIssueService = new HAVIssueService(myModelWrapper);
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

        public ActionResult Complaint(string complaintType, int sourceId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            User myUser = GetUserInformaton();
            ComplaintType myType = (ComplaintType)Enum.Parse(typeof(ComplaintType), complaintType);
            ComplaintModel.Builder myBuilder = new ComplaintModel.Builder(sourceId, myType);
            try {
                ComplaintHelper.FillComplaintModelBuilder(myBuilder, theUserRetrievalService, theIssueService, thePhotoService);
            } catch (Exception e) {
                LogError(e, String.Format("Unable get complaint info. [complaintModel={0}]", myBuilder.Build().ToString()));
                return SendToErrorPage("Unable to get the necessary information for the complaint. Please try again.");
            }

            return View("Complaint", myBuilder.Build());
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Complaint(ComplaintModel aComplaintModel) {
            if (!IsLoggedIn()) {
                ViewData["Message"] = MessageHelper.NormalMessage("You arn't logged in. Save your complaint, login, and please try again!");
                return View("Complaint");
            }

            try {
                User myUser = GetUserInformaton();
                switch (aComplaintModel.ComplaintType) {
                    case ComplaintType.Issue:
                        if (theService.IssueComplaint(myUser, aComplaintModel.Complaint, aComplaintModel.SourceId)) {
                             return SuccessfulComplaint();
                        }
                        break;
                    case ComplaintType.IssueReply:
                        if (theService.IssueReplyComplaint(myUser, aComplaintModel.Complaint, aComplaintModel.SourceId)) {
                             return SuccessfulComplaint();
                        }
                        break;
                    case ComplaintType.IssueReplyComment:
                        if (theService.IssueReplyCommentComplaint(myUser, aComplaintModel.Complaint, aComplaintModel.SourceId)) {
                             return SuccessfulComplaint();
                        }
                        break;
                    case ComplaintType.ProfileComplaint:
                        if (theService.ProfileComplaint(myUser, aComplaintModel.Complaint, aComplaintModel.SourceId)) {
                             return SuccessfulComplaint();
                        }
                        break;
                    case ComplaintType.PhotoComplaint:
                        if (theService.PhotoComplaint(myUser, aComplaintModel.Complaint, aComplaintModel.SourceId)) {
                             return SuccessfulComplaint();
                        }
                        break;
                }
            } catch (Exception e) {
                LogError(e, String.Format("Unable to post complaint. [complaintModel={0}]", aComplaintModel.ToString()));
                ViewData["Message"] = MessageHelper.ErrorMessage("Error logging report. Please try again.");
            }


            return View("Complaint", aComplaintModel);
        }

        private ActionResult SuccessfulComplaint() {
            return SendToResultPage("Complaint logged successfully!");
        }
    }
}
