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
        private const string GET_LATEST_ISSUES_ERROR = "Unable to get the latest myIssues.";
        private const string NO_ISSUES = "There are no latest myIssues to display.";

        private const string POST_SUCCESS = "Issue posted succesfully.";
        private const string DELETE_SUCCESS = "Issue deleted succesfully.";
        private const string EDIT_SUCCESS = "Issue edited successfully!";
        private const string REPLY_SUCCESS = "Reply posted successfully!";
        private const string DISPOSITION_SUCCESS = "Disposition added successfully!";

        private const string CREATING_ISSUE_ERROR = "Error creating issue. Please try again.";
        private const string CREATING_COMMENT_ERROR = "Error posting comment for the issue reply. Please try again.";
        private const string DISPOSITION_ERROR = "An error occurred while adding your disposition to the issue.";
        private const string DELETE_ISSUE_ERROR = "An error orror occurred while deleting the issue. Please try again.";
        private const string EDIT_ISSUE_LOAD_ERROR = "Error while retrieving your original issue. Please try again.";
        private const string EDIT_ISSUE_ERROR = "Error editing the issue. Please try again.";

        private const string PERSON_FILTER = "PersonFilter";
        private const string ISSUE_STANCE_FILTER = "IssueStanceFilter";

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
                IEnumerable<IssueReplyModel> registeredUserReplys = theService.GetReplysToIssue(myUser, issue, UserRoleHelper.RegisteredRoles(), PersonFilter.Politicians);
                IEnumerable<IssueReplyModel> officialUserReplys = theService.GetReplysToIssue(myUser, issue, UserRoleHelper.OfficialRoles(), PersonFilter.People);

                List<IssueReplyModel> myMerged = new List<IssueReplyModel>();
                myMerged.AddRange(registeredUserReplys);
                myMerged.AddRange(officialUserReplys);

                IssueModel myIssueModel = new IssueModel(issue, myMerged);

                SaveIssueInformationToTempDataForFiltering(myIssueModel);

                return View("View", myIssueModel);
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

            IssueModel myEditableModel = GetOriginalIssue();
            Dictionary<string, int> myFilter = GetUpdatedFilter(type, filterValue);

            myEditableModel.Replys = FilterReplys(myEditableModel, myFilter);

            return View("View", myEditableModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Disposition(int issueId, Disposition disposition, SiteSection aSection, int sourceId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User aUser = GetUserInformaton();
            try {
                bool myResult = theService.AddIssueDisposition(aUser, issueId, (int)disposition);
                if (!myResult) {
                    return SendToErrorPage("You can only provide a disposition towards an issue once.");
                }
                TempData["Message"] = MessageHelper.SuccessMessage(DISPOSITION_SUCCESS);
            } catch (Exception e) {
                LogError(e, DISPOSITION_ERROR);
                return SendToErrorPage(DISPOSITION_ERROR);
            }

            if (aSection == SiteSection.Profile) {
                return RedirectToAction("Show", "Profile", new { id = sourceId });
            } else if (aSection == SiteSection.MyProfile) {
                return RedirectToAction("Show", "Profile");
            } else if (aSection == SiteSection.IssueActivity) {
                return RedirectToAction("IssueActivity", "Profile");
            } else if (aSection == SiteSection.MyIssueActivity) {
                return RedirectToAction("IssueActivity", "Profile", new { id = sourceId });
            } else {
                return RedirectToAction("View", "Issue", new { id = issueId });
            }
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

        private void SaveIssueInformationToTempDataForFiltering(IssueModel aModel) {
            TempData[HAVConstants.ORIGINAL_ISSUE_TEMP_DATA] = aModel;

            Dictionary<string, int> myFilter = new Dictionary<string, int>();
            myFilter.Add(PERSON_FILTER, (int)PersonFilter.All);
            myFilter.Add(ISSUE_STANCE_FILTER, (int)IssueStanceFilter.All);

            TempData[HAVConstants.FILTER_TEMP_DATA] = myFilter;
        }

        private IssueModel GetOriginalIssue() {
            return ((IssueModel)TempData[HAVConstants.ORIGINAL_ISSUE_TEMP_DATA]).Copy();
        }

        private Dictionary<string, int> GetUpdatedFilter(string type, int filterValue) {
            Dictionary<string, int> myFilter = (Dictionary<string, int>)TempData[HAVConstants.FILTER_TEMP_DATA];
            myFilter.Remove(type);
            myFilter.Add(type, filterValue);
            return myFilter;
        }

        private IEnumerable<IssueReplyModel> FilterReplys(IssueModel myEditableModel, Dictionary<string, int> myFilter) {
            IEnumerable<IssueReplyModel> myReplys = new List<IssueReplyModel>();

            if (myFilter[PERSON_FILTER] != 0 && myFilter[ISSUE_STANCE_FILTER] != 0) {
                myReplys = (from r in myEditableModel.Replys
                            where r.TempPersonFilterHolder == myFilter[PERSON_FILTER]
                            && r.TempDispositionHolder == myFilter[ISSUE_STANCE_FILTER]
                            select r).ToList<IssueReplyModel>();
            } else if (myFilter[PERSON_FILTER] != 0) {
                myReplys = (from r in myEditableModel.Replys
                            where r.TempPersonFilterHolder == myFilter[PERSON_FILTER]
                            select r).ToList<IssueReplyModel>();
            } else if (myFilter[ISSUE_STANCE_FILTER] != 0) {
                myReplys = (from r in myEditableModel.Replys
                            where r.TempDispositionHolder == myFilter[ISSUE_STANCE_FILTER]
                            select r).ToList<IssueReplyModel>();
            } else {
                myReplys = myEditableModel.Replys;
            }

            return myReplys;
        }
    }
}
