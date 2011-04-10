using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.Issues;
using HaveAVoice.Services.UserFeatures;
using Social.User.Services;
using Social.Validation;

namespace HaveAVoice.Controllers.Issues {
    public class SearchController : HAVBaseController {
        private const string NO_MATCH = "Nothing matches your search query.";
        private const string USER_RESULTS = "UserResults";
        private const string ISSUE_RESULTS = "IssueResults";
        private const string SEARCH = "Search";
        
        private IHAVSearchService theService;
        private IUserRetrievalService<User> theUserRetrievalService;
        private IHAVIssueService theIssueService;

        public SearchController() {
            theService = new HAVSearchService();
            theUserRetrievalService = new UserRetrievalService<User>(new EntityHAVUserRetrievalRepository());
            theIssueService = new HAVIssueService(new ModelStateWrapper(this.ModelState));
        }

        public SearchController(IHAVSearchService aService, IUserRetrievalService<User> aUserRetrivalService, 
                                IHAVIssueService anIssueService) {
            theService = aService;
            theUserRetrievalService = aUserRetrivalService;
            theIssueService = anIssueService;
        }

        public ActionResult getUserAjaxResult(string q) {
            return Content(theService.UserSearch(q));
        }

        public ActionResult getIssueAjaxResult(string q) {
            return Content(theService.IssueSearch(q));
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index() {
            return View();
        }

        public ActionResult DoSearch(string SearchQuery, string searchType) {
            if(searchType.Equals("User")) {
                IEnumerable<User> myUsers = theUserRetrievalService.GetUsersByNameSearch(SearchQuery);

                if(myUsers.Count<User>() == 1) {
                    return RedirectToAction("Show", "Profile", new { shortName = myUsers.ElementAt(0).ShortUrl });
                } else if(myUsers.Count<User>() > 1) {
                    return View(USER_RESULTS, myUsers);
                } else {
                    TempData["Message"] = MessageHelper.NormalMessage(NO_MATCH);
                    return View(USER_RESULTS, myUsers);
                }
            } else {
                IEnumerable<Issue> myIssues = theIssueService.GetIssueByTitleSearch(SearchQuery);

                if (myIssues.Count<Issue>() == 1) {
                    return RedirectToAction("Details", "Issue", new { title = IssueTitleHelper.ConvertForUrl(myIssues.ElementAt(0).Title) });
                } else if (myIssues.Count<Issue>() > 1) {
                    return View(ISSUE_RESULTS, myIssues);
                } else {
                    TempData["Message"] = MessageHelper.NormalMessage(NO_MATCH);
                    return View(ISSUE_RESULTS, myIssues);
                }
            }       
        }
    }
}
