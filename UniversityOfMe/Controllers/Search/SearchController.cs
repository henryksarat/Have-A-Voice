using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Users.Services;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View.Search;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.Search;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Site {
    public class SearchController : UOFMeBaseController {
        private ISearchService theSearchService;

        public SearchController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theSearchService = new SearchService();
        }

        public ActionResult DoSearch(SearchType searchType, string searchString) {
            return JavaScript("window.top.location.href ='" + Url.Action("All") + "?searchString="+ searchString+"';");
        }

        public ActionResult All(string searchString) {
            IEnumerable<ISearchResult> mySearchResults = theSearchService.GetSearchResults(searchString);
            return View("All", mySearchResults);
        }
    }
}
