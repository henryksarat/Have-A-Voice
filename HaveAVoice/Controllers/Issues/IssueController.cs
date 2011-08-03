using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Helpers.Search;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Models.Wrappers;
using HaveAVoice.Services.Issues;
using Social.Admin.Helpers;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;

namespace HaveAVoice.Controllers.Issues {
    public class IssueController : HAVBaseController {
        private const string GET_LATEST_ISSUES_ERROR = "Unable to get the latest Issues.";
        private const string NO_ISSUES = "There are no issues to display.";
        private const string ISSUE_REFRESHED = "The issue has been refreshed. Please filter it again.";

        private const string POST_SUCCESS = "Issue posted succesfully.";
        private const string DELETE_SUCCESS = "Issue deleted succesfully.";
        private const string EDIT_SUCCESS = "Issue edited successfully!";
        private const string REPLY_SUCCESS = "Reply posted successfully!";
        private const string DISPOSITION_SUCCESS = "Disposition added successfully!";
        private const string ISSUE_DOESNT_EXIST = "Issue doesn't exist";
        private const string ISSUE_CREATE_TIMEOUT = "It seems like your session timed out. Please save your progress, relogin, and post again.";

        private const string CREATING_ISSUE_ERROR = "Error creating issue. Please try again.";
        private const string CREATING_COMMENT_ERROR = "Error posting comment for the issue reply. Please try again.";
        private const string DISPOSITION_ERROR = "An error occurred while adding your disposition to the issue.";
        private const string DELETE_ISSUE_ERROR = "An error orror occurred while deleting the issue. Please try again.";
        private const string EDIT_ISSUE_LOAD_ERROR = "Error while retrieving your original issue. Please try again.";
        private const string EDIT_ISSUE_ERROR = "Error editing the issue. Please try again.";
        private const string REDIRECT_ERROR = "Error redirecting you to the issue.";

        private const string PERSON_FILTER = "PersonFilter";
        private const string ISSUE_STANCE_FILTER = "IssueStanceFilter";

        private const string REDIRECT_TO_DETAILS_VIEW = "RedirectToDetails";
        private const string DETAILS_VIEW = "Details";
        private const string EDIT_VIEW = "Edit";

        private IHAVIssueService theIssueService;
        private IHAVIssueReplyService theIssueReplyService;
        private IValidationDictionary theValidationDictionary;

        public IssueController() {
            theValidationDictionary = new ModelStateWrapper(this.ModelState);

            theIssueService = new HAVIssueService(theValidationDictionary);
            theIssueReplyService = new HAVIssueReplyService(theValidationDictionary);
        }

        public IssueController(IHAVIssueService aService) {
            theIssueService = aService;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult List() {
            return Search(SearchBy.All, OrderBy.LastReplyDate, string.Empty);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search(SearchBy searchBy, OrderBy orderBy, string searchTerm) {
            return SearchIssues(searchBy, orderBy, searchTerm);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            if(!PermissionHelper<User>.AllowedToPerformAction(GetUserInformatonModel(), SocialPermission.Post_Issue)) {
                return SendToErrorPage(HAVConstants.PAGE_NOT_FOUND);
            }

            return View("Create");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(IssueWrapper anIssueWrapper) {
            if (!IsLoggedIn()) {
                ViewData["Message"] = MessageHelper.NormalMessage(ISSUE_CREATE_TIMEOUT);
                return View("Create", anIssueWrapper);
            }

            UserInformationModel<User> myUser = GetUserInformatonModel();

            try {

                if (theIssueService.CreateIssue(myUser, anIssueWrapper.ToModel())) {
                    TempData["Message"] += MessageHelper.SuccessMessage(POST_SUCCESS);
                    return RedirectToAction(DETAILS_VIEW, new { title = IssueTitleHelper.ConvertForUrl(anIssueWrapper.Title) });
                }
            } catch (Exception e) {
                LogError(e, CREATING_ISSUE_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(CREATING_ISSUE_ERROR);
            }
            return View("Create", anIssueWrapper);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult RedirectToDetails(int id) {
            Issue myIssue;
            try {
                myIssue = theIssueService.GetIssue(id, GetUserInformatonModel());
            } catch (Exception e) {
                TempData["Message"] += MessageHelper.ErrorMessage(REDIRECT_ERROR);
                LogError(e, REDIRECT_ERROR);
                return RedirectToProfile();
            }
            if (myIssue != null) {
                return RedirectToAction(DETAILS_VIEW, new { title = IssueTitleHelper.ConvertForUrl(myIssue.Title) });
            } else {
                TempData["Message"] += MessageHelper.ErrorMessage(REDIRECT_ERROR);
                return RedirectToProfile();
            }
        }
        
        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Details(string title) {
            try {
                title = IssueTitleHelper.ConvertFromUrl(title);
                IssueModel myIssueModel;

                if (IsLoggedIn()) {
                    myIssueModel = theIssueService.CreateIssueModel(GetUserInformatonModel().Details,title);
                } else {
                    myIssueModel = theIssueService.CreateIssueModel(title);
                }

                if (myIssueModel == null) {
                    return SendToErrorPage(ISSUE_DOESNT_EXIST);
                }

                SaveIssueInformationToTempDataForFiltering(myIssueModel);

                return View(DETAILS_VIEW, myIssueModel);
            } catch (Exception e) {
                string details = "An error occurred while trying to view the issue.";
                LogError(e, details);
                return SendToErrorPage(details);
            }
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Details(IssueModel issueModel) {
            try {
                if (theIssueReplyService.CreateIssueReply(issueModel)) {
                    TempData["Message"] += MessageHelper.SuccessMessage(REPLY_SUCCESS);
                }
            } catch (Exception e) {
                LogError(e, CREATING_COMMENT_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(CREATING_COMMENT_ERROR);
                theValidationDictionary.ForceModleStateExport();
            }
            return RedirectToAction(REDIRECT_TO_DETAILS_VIEW, new { id = issueModel.IssueId });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Disposition(int issueId, Disposition disposition, SiteSection section, int sourceId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User aUser = GetUserInformaton();
            try {
                bool myResult = theIssueService.AddIssueStance(aUser, issueId, (int)disposition);
                if (!myResult) {
                    return SendToErrorPage("You can only provide a disposition towards an issue once.");
                }
                TempData["Message"] += MessageHelper.SuccessMessage(DISPOSITION_SUCCESS);
            } catch (Exception e) {
                LogError(e, DISPOSITION_ERROR);
                return SendToErrorPage(DISPOSITION_ERROR);
            }

            if (section == SiteSection.Profile) {
                return RedirectToAction("Show", "Profile", new { id = sourceId });
            } else if (section == SiteSection.MyProfile) {
                return RedirectToAction("Show", "Profile");
            } else if (section == SiteSection.IssueActivity) {
                return RedirectToAction("IssueActivity", "Profile", new { id = sourceId });
            } else if (section == SiteSection.MyIssueActivity) {
                return RedirectToAction("IssueActivity", "Profile");
            } else if(section == SiteSection.IssueReply) {
                return RedirectToAction("Details", "IssueReply", new { id = sourceId });
            } else {
                return RedirectToAction(REDIRECT_TO_DETAILS_VIEW, "Issue", new { id = issueId });
            }
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData] 
        public ActionResult Delete(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            if (!PermissionHelper<User>.AllowedToPerformAction(GetUserInformatonModel(), SocialPermission.Delete_Issue, SocialPermission.Delete_Any_Issue)) {
                return SendToErrorPage(HAVConstants.NOT_ALLOWED);
            }
            UserInformationModel<User> myUserInfo = GetUserInformatonModel();
            try {
                bool myResult = theIssueService.DeleteIssue(myUserInfo, id);
                if (myResult) {
                    TempData["Message"] += MessageHelper.SuccessMessage(DELETE_SUCCESS);
                    return RedirectToAction("Index", "Issue");
                }
            } catch (Exception myException) {
                LogError(myException, DELETE_ISSUE_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(DELETE_ISSUE_ERROR);
            }

            return RedirectToAction(REDIRECT_TO_DETAILS_VIEW, new { id = id });
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Edit(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            if (!PermissionHelper<User>.AllowedToPerformAction(myUserInformation, SocialPermission.Edit_Issue, SocialPermission.Edit_Any_Issue)) {
                return SendToErrorPage(HAVConstants.NOT_ALLOWED);
            }

            try {
                Issue myIssue = theIssueService.GetIssue(id, myUserInformation);
                if (myUserInformation.Details.Id == myIssue.User.Id || PermissionHelper<User>.AllowedToPerformAction(myUserInformation, SocialPermission.Edit_Any_Issue)) {
                    return View(EDIT_VIEW, myIssue);
                } else {
                    return SendToErrorPage(HAVConstants.NOT_ALLOWED);
                }
            } catch (Exception myException) {
                LogError(myException, EDIT_ISSUE_LOAD_ERROR);
                return SendToErrorPage(EDIT_ISSUE_LOAD_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Edit(IssueWrapper anIssueWrapper) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            
            try {
                bool myResult = theIssueService.EditIssue(GetUserInformatonModel(), anIssueWrapper.ToModel());
                if (myResult) {
                    TempData["Message"] += MessageHelper.SuccessMessage(EDIT_SUCCESS);
                    return RedirectToAction(REDIRECT_TO_DETAILS_VIEW, new { id = anIssueWrapper.Id });
                }
            } catch (Exception myException) {
                LogError(myException, EDIT_ISSUE_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(EDIT_ISSUE_ERROR);
            }

            return RedirectToAction(EDIT_VIEW, new { id = anIssueWrapper.Id });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult FilterIssueByPersonFilter(PersonFilter filterValue, int id) {
            return FilterIssue(PERSON_FILTER, filterValue.ToString(), id);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult FilterIssueByIssueStanceFilter(IssueStanceFilter filterValue, int id) {
            return FilterIssue(ISSUE_STANCE_FILTER, filterValue.ToString(), id);
        }

        private ActionResult FilterIssue(string aFilterType, string aFilterValue, int anId) {
            IssueModel myOriginalModel = GetOriginalIssue();
            if (myOriginalModel == null) {
                TempData["Message"] += MessageHelper.NormalMessage(ISSUE_REFRESHED);
                return RedirectToAction(REDIRECT_TO_DETAILS_VIEW, new { id = anId });
            }
            Dictionary<string, string> myFilter = GetUpdatedFilter(aFilterType, aFilterValue);

            IEnumerable<IssueReplyModel> myFilteredReplys = FilterReplys(myOriginalModel, myFilter);
            IssueModel mynewModel = new IssueModel(myOriginalModel.Issue, myFilteredReplys);

            SaveOriginalIssue(myOriginalModel);

            mynewModel.States = new SelectList(UnitedStates.STATES, Constants.SELECT);
            return View("Details", mynewModel);
        }

        private ActionResult SearchIssues(SearchBy aSearchBy, OrderBy anOrderBy, string aSearchTerm) {
            IEnumerable<IssueWithDispositionModel> myIssues = new List<IssueWithDispositionModel>(); ;
            try {
                myIssues = theIssueService.GetIssues(GetUserInformaton(), aSearchBy, anOrderBy, aSearchTerm);
            } catch (Exception e) {
                LogError(e, GET_LATEST_ISSUES_ERROR);
                return SendToErrorPage(GET_LATEST_ISSUES_ERROR);
            }

            if (myIssues.Count() == 0) {
                ViewData["Message"] = MessageHelper.NormalMessage(NO_ISSUES);
            }

            SearchModel<IssueWithDispositionModel> mySearchModel = new SearchModel<IssueWithDispositionModel>() {
                SearchResults = myIssues,
                SearchByOptions = new SelectList(theIssueService.SearchByOptions(), "Value", "Key", aSearchBy),
                OrderByOptions = new SelectList(theIssueService.OrderByOptions(), "Value", "Key", anOrderBy)
            };

            return View("List", mySearchModel);
        }

        private void SaveIssueInformationToTempDataForFiltering(IssueModel aModel) {
            SaveOriginalIssue(aModel);

            Dictionary<string, string> myFilter = new Dictionary<string, string>();
            myFilter.Add(PERSON_FILTER, PersonFilter.All.ToString());
            myFilter.Add(ISSUE_STANCE_FILTER, IssueStanceFilter.All.ToString());

            TempData[HAVConstants.FILTER_TEMP_DATA] = myFilter;
        }

        private void SaveOriginalIssue(IssueModel aModel) {
            TempData[HAVConstants.ORIGINAL_ISSUE_TEMP_DATA] = aModel;
        }

        private IssueModel GetOriginalIssue() {
            return ((IssueModel)TempData[HAVConstants.ORIGINAL_ISSUE_TEMP_DATA]);
        }

        private Dictionary<string, string> GetUpdatedFilter(string aType, string aFilterValue) {
            Dictionary<string, string> myFilter = (Dictionary<string, string>)TempData[HAVConstants.FILTER_TEMP_DATA];
            myFilter.Remove(aType);
            myFilter.Add(aType, aFilterValue);
            return myFilter;
        }

        private IEnumerable<IssueReplyModel> FilterReplys(IssueModel myEditableModel, Dictionary<string, string> myFilter) {
            IEnumerable<IssueReplyModel> myReplys = new List<IssueReplyModel>();

            if (myFilter[PERSON_FILTER] != PersonFilter.All.ToString() && myFilter[ISSUE_STANCE_FILTER] != IssueStanceFilter.All.ToString()) {
                myReplys = (from r in myEditableModel.Replys
                            where r.PersonFilter.ToString() == myFilter[PERSON_FILTER]
                            && r.IssueStanceFilter.ToString() == myFilter[ISSUE_STANCE_FILTER]
                            select r).ToList<IssueReplyModel>();
            } else if (myFilter[PERSON_FILTER] != PersonFilter.All.ToString()) {
                myReplys = (from r in myEditableModel.Replys
                            where r.PersonFilter.ToString() == myFilter[PERSON_FILTER]
                            select r).ToList<IssueReplyModel>();
            } else if (myFilter[ISSUE_STANCE_FILTER] != IssueStanceFilter.All.ToString()) {
                myReplys = (from r in myEditableModel.Replys
                            where r.IssueStanceFilter.ToString() == myFilter[ISSUE_STANCE_FILTER]
                            select r).ToList<IssueReplyModel>();
            } else {
                myReplys = myEditableModel.Replys;
            }

            return myReplys;
        }
    }
}
