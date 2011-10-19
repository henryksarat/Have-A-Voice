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
using UniversityOfMe.Helpers.Search;
using System;
using Social.Generic.ActionFilters;
using Social.Generic.Models;

namespace UniversityOfMe.Controllers.Site {
    public class SearchController : UOFMeBaseController {
        private ISearchService theSearchService;

        public SearchController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theSearchService = new SearchService();
        }

        public ActionResult DoSearch(SearchFilter searchType, string searchString, int page) {
            return JavaScript("window.top.location.href ='" + Url.Action(searchType.ToString()) + "?searchString=" + searchString + "&page=" + page + "';");
        }

        public ActionResult DoAdvancedSearch(SearchFilter searchType, string searchString, int page, SearchBy searchBy, OrderBy orderBy, string universityId) {
            return JavaScript("window.top.location.href ='" + Url.Action(searchType.ToString() + "Advanced") + "?searchString=" + searchString + "&page=" + page + "&searchBy=" + searchBy.ToString() + "&orderBy=" + orderBy.ToString() + "&universityId=" + universityId + "';");
        }

        public ActionResult All(string universityId, string searchString, int page) {
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            searchString = MassageSearchString(searchString);
            SearchResultsModel mySearchResults = theSearchService.GetAllSearchResults(myUserInformation, searchString, page);
            return View("Results", mySearchResults);
        }

        public ActionResult AllAdvanced(string searchString, int page, SearchBy searchBy, OrderBy orderBy, string universityId) {
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            searchString = MassageSearchString(searchString);
            SearchResultsModel mySearchResults = theSearchService.GetAllSearchResults(myUserInformation, searchString, page, universityId);
            return View("Results", mySearchResults);
        }

        public ActionResult Class(string searchString, int page) {
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            searchString = MassageSearchString(searchString);
            SearchResultsModel mySearchResults = theSearchService.GetClassSearchResults(myUserInformation, searchString, page, SearchBy.ClassCode, OrderBy.ClassCode);
            return View("Results", mySearchResults);
        }

        public ActionResult ClassAdvanced(string searchString, int page, SearchBy searchBy, OrderBy orderBy, string universityId) {
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            searchString = MassageSearchString(searchString);
            SearchResultsModel mySearchResults = theSearchService.GetClassSearchResults(myUserInformation, searchString, page, searchBy, orderBy, universityId);
            return View("Results", mySearchResults);
        }

        public ActionResult User(string searchString, int page) {
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            searchString = MassageSearchString(searchString);
            SearchResultsModel mySearchResults = theSearchService.GetUserSearchResults(myUserInformation, searchString, page, SearchBy.Name, OrderBy.Name);
            return View("Results", mySearchResults);
        }

        public ActionResult UserAdvanced(string searchString, int page, SearchBy searchBy, OrderBy orderBy, string universityId) {
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            searchString = MassageSearchString(searchString);
            SearchResultsModel mySearchResults = theSearchService.GetUserSearchResults(myUserInformation, searchString, page, searchBy, orderBy, universityId);
            return View("Results", mySearchResults);
        }

        public ActionResult Textbook(string searchString, int page) {
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            searchString = MassageSearchString(searchString);
            SearchResultsModel mySearchResults = theSearchService.GetTextBookSearchResults(myUserInformation, searchString, page, SearchBy.Title, OrderBy.Title);
            return View("Results", mySearchResults);
        }

        public ActionResult TextbookAdvanced(string searchString, int page, SearchBy searchBy, OrderBy orderBy, string universityId) {
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            searchString = MassageSearchString(searchString);
            SearchResultsModel mySearchResults = theSearchService.GetTextBookSearchResults(myUserInformation, searchString, page, searchBy, orderBy, universityId);
            return View("Results", mySearchResults);
        }

        public ActionResult Marketplace(string searchString, int page, string itemType) {
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            searchString = MassageSearchString(searchString);
            SearchResultsModel mySearchResults = theSearchService.MarketplaceSearchResults(myUserInformation, searchString, page, SearchBy.Title, OrderBy.Newest, itemType);
            return View("Results", mySearchResults);
        }

        public ActionResult MarketplaceAdvanced(string searchString, int page, string itemType, string universityId) {
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            searchString = MassageSearchString(searchString);
            SearchResultsModel mySearchResults = theSearchService.MarketplaceSearchResults(myUserInformation, searchString, page, SearchBy.Title, OrderBy.Newest, itemType, universityId);
            return View("Results", mySearchResults);
        }

        private string MassageSearchString(string aSearchString) {
            return string.IsNullOrEmpty(aSearchString) ? string.Empty : aSearchString;
        }
    }
}
