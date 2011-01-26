using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Models.Wrappers;
using HaveAVoice.Repositories;
using HaveAVoice.Services;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Validation;
using HaveAVoice.Controllers.Helpers;
using System.Collections;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Controllers.Issues {
    public class IssueController : HAVBaseController {
        private static string GET_LATEST_ISSUES_ERROR = "Unable to get the latest myIssues.";
        private static string NO_ISSUES = "There are no latest myIssues to display.";

        private static string POST_SUCCESS = "Issue posted succesfully.";
        private static string DELETE_SUCCESS = "Issue deleted succesfully.";
        private static string EDIT_SUCCESS = "Issue edited successfully!";
        private static string REPLY_SUCCESS = "Reply posted successfully!";
        private static string DISPOSITION_SUCCESS = "Disposition added successfully!";

        private static string CREATING_ISSUE_ERROR = "Error creating issue. Please try again.";
        private static string CREATING_COMMENT_ERROR = "Error posting comment for the issue reply. Please try again.";
        private static string DISPOSITION_ERROR = "An error occurred while adding your disposition to the issue.";
        private static string DELETE_ISSUE_ERROR = "An error orror occurred while deleting the issue. Please try again.";
        private static string EDIT_ISSUE_LOAD_ERROR = "Error while retrieving your original issue. Please try again.";
        private static string EDIT_ISSUE_ERROR = "Error editing the issue. Please try again.";

        private IHAVIssueService theService;

        public IssueController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
            theService = new HAVIssueService(new ModelStateWrapper(this.ModelState));
        }

        public IssueController(IHAVIssueService aService, IHAVBaseService baseService)
            : base(baseService) {
            theService = aService;
        }

        public ActionResult Index() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            IEnumerable<IssueWithDispositionModel> myIssues;
            try {
                myIssues = theService.GetIssues(GetUserInformaton());
            } catch (Exception e) {
                LogError(e, GET_LATEST_ISSUES_ERROR);
                return SendToErrorPage(GET_LATEST_ISSUES_ERROR);
            }

            if (myIssues.Count() == 0) {
                ViewData["Message"] = MessageHelper.NormalMessage(NO_ISSUES);
            }

            return View("Index", myIssues);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            if(!HAVPermissionHelper.AllowedToPerformAction(GetUserInformatonModel(), HAVPermission.Post_Issue)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }

            return View("Create");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(IssueWrapper anIssueWrapper) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            UserInformationModel myUser = GetUserInformatonModel();

            try {
                if (theService.CreateIssue(myUser, anIssueWrapper.ToModel())) {
                    TempData["Message"] = MessageHelper.SuccessMessage(POST_SUCCESS);
                    return RedirectToAction("Index");
                }
            } catch (Exception e) {
                LogError(e, CREATING_ISSUE_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(CREATING_ISSUE_ERROR);
            }
            return View("Create", anIssueWrapper);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult View(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            if(!HAVPermissionHelper.AllowedToPerformAction(GetUserInformatonModel(), HAVPermission.View_Issue)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }

            try {
                Issue issue = theService.GetIssue(id);

                if (issue == null) {
                    return SendToErrorPage("Issue doesn't exist!");
                }

                User myUser = HAVUserInformationFactory.GetUserInformation().Details;
                IEnumerable<IssueReplyModel> registeredUserReplys = theService.GetReplysToIssue(myUser, issue, RoleHelper.RegisteredRoles(), PersonFilter.Politicians);
                IEnumerable<IssueReplyModel> officialUserReplys = theService.GetReplysToIssue(myUser, issue, RoleHelper.OfficialRoles(), PersonFilter.People);

                List<IssueReplyModel> myMerged = new List<IssueReplyModel>();
                myMerged.AddRange(registeredUserReplys);
                myMerged.AddRange(officialUserReplys);

                IssueModel issueModel = new IssueModel(issue, myMerged);
                TempData["OriginalIssue"] = issueModel;
                
                Dictionary<string, int> myFilter = new Dictionary<string, int>();
                myFilter.Add("PersonFilter", (int)PersonFilter.All);
                myFilter.Add("IssueStanceFilter", (int)IssueStanceFilter.All);

                TempData["Filter"] = myFilter;

                return View("View", issueModel);
            } catch (Exception e) {
                string details = "An error occurred while trying to view the issue.";
                LogError(e, details);
                return SendToErrorPage(details);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult View(IssueModel issueModel) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel myUserInformation = GetUserInformatonModel();

            try {
                if (theService.CreateIssueReply(myUserInformation, issueModel)) {
                    TempData["Message"] = MessageHelper.SuccessMessage(REPLY_SUCCESS);
                    return RedirectToAction("View", new { id = issueModel.Issue.Id });
                }
            } catch (Exception e) {
                LogError(e, CREATING_COMMENT_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(CREATING_COMMENT_ERROR);
            }
            return View("View", issueModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult FilterIssue(string type, int filterValue) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            IssueModel myEditableModel = ((IssueModel)TempData["OriginalIssue"]).Copy();
            Dictionary<string, int> myFilter = (Dictionary<string, int>)TempData["Filter"];
            myFilter.Remove(type);
            myFilter.Add(type, filterValue);

            IEnumerable<IssueReplyModel> myReplys = new List<IssueReplyModel>();

            if(myFilter["PersonFilter"] != 0 && myFilter["IssueStanceFilter"] != 0) {
                myReplys = (from r in myEditableModel.Replys
                            where r.TempPersonFilterHolder == myFilter["PersonFilter"]
                            && r.TempDispositionHolder == myFilter["IssueStanceFilter"]
                            select r).ToList<IssueReplyModel>();
            } else if (myFilter["PersonFilter"] != 0) {
                myReplys = (from r in myEditableModel.Replys
                            where r.TempPersonFilterHolder == myFilter["PersonFilter"]
                            select r).ToList<IssueReplyModel>();
            } else if (myFilter["IssueStanceFilter"] != 0) {
                myReplys = (from r in myEditableModel.Replys
                            where r.TempDispositionHolder == myFilter["IssueStanceFilter"]
                            select r).ToList<IssueReplyModel>();
            }

            myEditableModel.Replys = myReplys;

            return View("View", myEditableModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Disposition(int issueId, int disposition) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User aUser = GetUserInformaton();
            try {
                theService.AddIssueDisposition(aUser, issueId, disposition);
            } catch (Exception e) {
                LogError(e, DISPOSITION_ERROR);
                return SendToErrorPage(DISPOSITION_ERROR);
            }

            TempData["Message"] = MessageHelper.SuccessMessage(DISPOSITION_SUCCESS);
            return RedirectToAction("Index");
        }
                                            
        public ActionResult Delete(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            if (!HAVPermissionHelper.AllowedToPerformAction(GetUserInformatonModel(), HAVPermission.Delete_Issue, HAVPermission.Delete_Any_Issue)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }
            UserInformationModel myUserInfo = GetUserInformatonModel();
            try {
                theService.DeleteIssue(myUserInfo, id);
            } catch (Exception myException) {
                LogError(myException, DELETE_ISSUE_ERROR);
                return SendToErrorPage(DELETE_ISSUE_ERROR);
            }

            TempData["Message"] = MessageHelper.SuccessMessage(DELETE_SUCCESS);
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel myUserInformation = GetUserInformatonModel();
            if (!HAVPermissionHelper.AllowedToPerformAction(myUserInformation, HAVPermission.Edit_Issue, HAVPermission.Edit_Any_Issue)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }

            try {
                Issue myIssue = theService.GetIssue(id);
                if (myUserInformation.Details.Id == myIssue.User.Id || HAVPermissionHelper.AllowedToPerformAction(myUserInformation, HAVPermission.Edit_Any_Issue)) {
                    return View("Edit", IssueWrapper.Build(myIssue));
                } else {
                    return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
                }
            } catch (Exception myException) {
                LogError(myException, EDIT_ISSUE_LOAD_ERROR);
                return SendToErrorPage(EDIT_ISSUE_LOAD_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(IssueWrapper anIssueWrapper) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            
            try {
                bool myResult = theService.EditIssue(GetUserInformatonModel(), anIssueWrapper.ToModel());
                if (myResult) {
                    TempData["Message"] = MessageHelper.SuccessMessage(EDIT_SUCCESS);
                    return RedirectToAction("View", new {id = anIssueWrapper.Id});
                }
            } catch (Exception myException) {
                LogError(myException, EDIT_ISSUE_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(EDIT_ISSUE_ERROR);
            }

            return View("Edit", anIssueWrapper);
        }
    }
}
