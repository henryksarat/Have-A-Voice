﻿
using System;
using System.Web.Mvc;
using HaveAVoice.Models.Services.UserFeatures;
using HaveAVoice.Helpers;
using HaveAVoice.Models.Validation;
using HaveAVoice.Models.Repositories;
using HaveAVoice.Models.Services;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Controllers.Users
{
    public class ComplaintController : HAVBaseController {
        private IHAVComplaintService theService;
        private IHAVUserService theUserService;
        private IHAVIssueService theIssueService;

        public ComplaintController()
            : base(new HAVBaseService(new HAVBaseRepository())) {
             ModelStateWrapper myModelWrapper = new ModelStateWrapper(this.ModelState);
            theService = new HAVComplaintService(myModelWrapper);
            theUserService = new HAVUserService(myModelWrapper);
            theIssueService = new HAVIssueService(myModelWrapper);
        }

        public ComplaintController(IHAVComplaintService aService, IHAVBaseService aBaseService, 
            IHAVUserService aUserService, IHAVIssueService aIssueService)
            : base(aBaseService) {
            theService = aService;
            theUserService = aUserService;
            theIssueService = aIssueService;
        }

        public ActionResult Complaint(string complaintType, int sourceId) {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }

            User myUser = GetUserInformaton();
            ComplaintType myType = (ComplaintType)Enum.Parse(typeof(ComplaintType), complaintType);
            ComplaintModel.Builder myBuilder = new ComplaintModel.Builder(sourceId, myType);
            try {
                ComplaintHelper.FillComplaintModelBuilder(myBuilder, theUserService, theIssueService);
            } catch (Exception e) {
                LogError(e, String.Format("Unable get complaint info. [complaintModel={0}]", myBuilder.Build().ToString()));
                return SendToErrorPage("Unable to get the necessary information for the complaint. Please try again.");
            }

            return View("Complaint", myBuilder.Build());
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Complaint(ComplaintModel aComplaintModel) {
            if (!IsLoggedIn()) {
                ViewData["Message"] = "You arn't logged in. Save your complaint, login, and please try again!";
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
                    case ComplaintType.MergeComplaint:
                        //Not implimented yet
                        break;
                    case ComplaintType.ProfileComplaint:
                        if (theService.ProfileComplaint(myUser, aComplaintModel.Complaint, aComplaintModel.SourceId)) {
                             return SuccessfulComplaint();
                        }
                        break;
                    case ComplaintType.UserPictureComplaint:
                        if (theService.UserPictureComplaint(myUser, aComplaintModel.Complaint, aComplaintModel.SourceId)) {
                             return SuccessfulComplaint();
                        }
                        break;
                }
            } catch (Exception e) {
                LogError(e, String.Format("Unable to post complaint. [complaintModel={0}]", aComplaintModel.ToString()));
                ViewData["Message"] = "Error logging report. Please try again.";
            }


            return View("Complaint", aComplaintModel);
        }

        private ActionResult SuccessfulComplaint() {
            return SendToResultPage("Complaint logged successfully!");
        }

        public override ActionResult SendToResultPage(string title, string details) {
            return SendToResultPage(SiteSectionsEnum.Complaint, title, details);
        }
    }
}
