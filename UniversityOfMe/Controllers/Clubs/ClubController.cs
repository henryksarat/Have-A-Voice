using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Services.Clubs;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Users.Services;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services;
using UniversityOfMe.Services.Professors;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Clubs {

    public class ClubController : UOFMeBaseController {
        public const string CLUB_CREATED = "Club created successfully!";
        public const string CLUB_LIST_ERROR = "Error getting club list. Please try again.";
        public const string CLUB_TYPE_ERROR = "Error getting club types. Please try again.";
        public const string GET_CLUB_ERROR = "An error has occurred while retrieving the club. Please try again.";

        IValidationDictionary theValidationDictionary;
        IClubService theClubService;
        IUniversityService theUniversityService;

        public ClubController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())));
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theClubService = new ClubService(theValidationDictionary);
            theUniversityService = new UniversityService(theValidationDictionary);
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Create(string universityId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {

                User myUser = GetUserInformatonModel().Details;

                if (!UniversityHelper.IsFromUniversity(myUser, universityId)) {
                    return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
                }

                IDictionary<string, string> myClubTypes = DictionaryHelper.DictionaryWithSelect();

                LoggedInWrapperModel<CreateClubModel> myLoggedIn = new LoggedInWrapperModel<CreateClubModel>(myUser);
                myClubTypes = theClubService.CreateAllClubTypesDictionaryEntry();

                CreateClubModel myClubModel = new CreateClubModel() {
                    ClubTypes = new SelectList(myClubTypes, "Value", "Key")
                };

                myLoggedIn.Set(myClubModel);

                return View("Create", myLoggedIn);
            } catch (Exception myExpcetion) {
                LogError(myExpcetion, CLUB_TYPE_ERROR);
                return SendToErrorPage(CLUB_TYPE_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(CreateClubModel club) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, club.UniversityId)) {
                return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
            }

            UserInformationModel<User> myUserInformation = GetUserInformatonModel();


            try {
                bool myResult = theClubService.CreateClub(myUserInformation, club);

                if (myResult) {
                    TempData["Message"] = CLUB_CREATED;
                    return RedirectToAction("List");
                }
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                ViewData["Message"] = ErrorKeys.ERROR_MESSAGE;
            }

            return RedirectToAction("Create");
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Details(int id) {
            try {
                User myUser = GetUserInformatonModel().Details;

                Club myClub = theClubService.GetClub(id);

                if (!UniversityHelper.IsFromUniversity(myUser, myClub.UniversityId)) {
                    return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
                }

                LoggedInWrapperModel<Club> myLoggedIn = new LoggedInWrapperModel<Club>(myUser);
                myLoggedIn.Set(myClub);

                return View("Details", myLoggedIn);
            } catch (Exception myException) {
                LogError(myException, GET_CLUB_ERROR);
                return SendToResultPage(GET_CLUB_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult List(string universityId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {

                User myUser = GetUserInformatonModel().Details;

                if (!UniversityHelper.IsFromUniversity(myUser, universityId)) {
                    return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
                }

                IEnumerable<Club> myClubs = new List<Club>();

                LoggedInListModel<Club> myLoggedIn = new LoggedInListModel<Club>(myUser);
                myClubs = theClubService.GetClubs(universityId);
                myLoggedIn.Set(myClubs);
                return View("List", myLoggedIn);
            } catch (Exception myException) {
                LogError(myException, CLUB_LIST_ERROR);
                return SendToErrorPage(CLUB_LIST_ERROR);
            }
        }
    }
}
