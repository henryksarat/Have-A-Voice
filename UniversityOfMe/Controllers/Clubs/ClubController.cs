using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Users.Services;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Helpers.Constants;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services;
using UniversityOfMe.Services.Clubs;
using UniversityOfMe.Services.Professors;
using UniversityOfMe.UserInformation;
using Social.Admin.Exceptions;

namespace UniversityOfMe.Controllers.Clubs {

    public class ClubController : UOFMeBaseController {
        private const string CLUB_CREATED = "Club created successfully!";
        private const string CLUB_EDITED = "Club edited successfully!";
        private const string CLUB_DEACTIVATED = "The club has been deactivated! You can always activate it again by going on the club page and activating it.";
        private const string CLUB_ACTIVATED = "The club has been activated again!";

        private const string CLUB_LIST_ERROR = "Error getting club list. Please try again.";
        private const string CLUB_TYPE_ERROR = "Error getting club types. Please try again.";
        private const string CLUB_GET_FOR_EDIT_ERROR = "Error getting the club for an edit. Please try again.";
        private const string GET_CLUB_ERROR = "An error has occurred while retrieving the club. Please try again.";
        private const string CLUB_DEACTIVATED_ERROR = "An error has occurred while deactivating the club.";
        private const string CLUB_ACTIVATED_ERROR = "An error has occurred while activating the club again.";

        IValidationDictionary theValidationDictionary;
        IClubService theClubService;
        IUniversityService theUniversityService;

        public ClubController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theClubService = new ClubService(theValidationDictionary);
            theUniversityService = new UniversityService(theValidationDictionary);
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Activate(int clubId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                bool myResult = theClubService.ActivateClub(GetUserInformatonModel(), clubId);

                if (myResult) {
                    TempData["Message"] = MessageHelper.SuccessMessage(CLUB_ACTIVATED);
                }
            } catch (Exception myException) {
                LogError(myException, CLUB_DEACTIVATED_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(CLUB_ACTIVATED_ERROR);
            }

            return RedirectToAction("Details", new { id = clubId });
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

                LoggedInWrapperModel<ClubViewModel> myLoggedIn = new LoggedInWrapperModel<ClubViewModel>(myUser);
                myClubTypes = theClubService.CreateAllClubTypesDictionaryEntry();

                ClubViewModel myClubModel = new ClubViewModel() {
                    ClubTypes = new SelectList(myClubTypes, "Value", "Key"),
                    Title = ClubConstants.DEFAULT_CLUB_LEADER_TITLE
                };

                myLoggedIn.Set(myClubModel);

                return View("Create", myLoggedIn);
            } catch (Exception myExpcetion) {
                LogError(myExpcetion, CLUB_TYPE_ERROR);
                return SendToErrorPage(CLUB_TYPE_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(ClubViewModel club) {
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
                    TempData["Message"] = MessageHelper.SuccessMessage(CLUB_CREATED);
                    return RedirectToAction("List");
                }
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                ViewData["Message"] = MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            }

            return RedirectToAction("Create");
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Deactivate(int clubId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                bool myResult = theClubService.DeactivateClub(GetUserInformatonModel(), clubId);

                if (myResult) {
                    TempData["Message"] = MessageHelper.SuccessMessage(CLUB_DEACTIVATED);
                }
            } catch (Exception myException) {
                LogError(myException, CLUB_DEACTIVATED_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(CLUB_DEACTIVATED_ERROR);
            }

            return RedirectToAction("List", "Club");
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Details(int id) {
            try {
                UserInformationModel<User> myUser = GetUserInformatonModel();

                Club myClub = theClubService.GetClub(myUser, id);

                if (!UniversityHelper.IsFromUniversity(myUser.Details, myClub.UniversityId)) {
                    return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
                }

                LoggedInWrapperModel<Club> myLoggedIn = new LoggedInWrapperModel<Club>(myUser.Details);
                myLoggedIn.Set(myClub);

                return View("Details", myLoggedIn);
            } catch (Exception myException) {
                LogError(myException, GET_CLUB_ERROR);
                return SendToResultPage(GET_CLUB_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Edit(string universityId, int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {

                UserInformationModel<User> myUser = GetUserInformatonModel();

                IDictionary<string, string> myClubTypes = DictionaryHelper.DictionaryWithSelect();

                LoggedInWrapperModel<ClubViewModel> myLoggedIn = new LoggedInWrapperModel<ClubViewModel>(myUser.Details);
                myClubTypes = theClubService.CreateAllClubTypesDictionaryEntry();

                Club myClub = theClubService.GetClubForEdit(myUser, id);

                ClubViewModel myClubModel = new ClubViewModel(myClub) {
                    ClubTypes = new SelectList(myClubTypes, "Value", "Key", myClub.ClubType)
                };

                myLoggedIn.Set(myClubModel);

                return View("Edit", myLoggedIn);
            } catch(PermissionDenied myException) {
                TempData["Message"] = MessageHelper.WarningMessage(myException.Message);
            } catch (Exception myExpcetion) {
                LogError(myExpcetion, CLUB_GET_FOR_EDIT_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(CLUB_GET_FOR_EDIT_ERROR);
            }

            return RedirectToAction("Details", new { id = id });
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Edit(ClubViewModel club) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            UserInformationModel<User> myUserInformation = GetUserInformatonModel();

            try {
                bool myResult = theClubService.EditClub(myUserInformation, club);

                if (myResult) {
                    TempData["Message"] = MessageHelper.SuccessMessage(CLUB_EDITED);
                    return RedirectToAction("Details", new { id = club.ClubId });
                }
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] = MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            }

            return RedirectToAction("Edit", new { id = club.ClubId });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult List(string universityId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUser = GetUserInformatonModel();

                if (!UniversityHelper.IsFromUniversity(myUser.Details, universityId)) {
                    return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
                }

                IEnumerable<Club> myClubs = new List<Club>();

                LoggedInListModel<Club> myLoggedIn = new LoggedInListModel<Club>(myUser.Details);
                myClubs = theClubService.GetClubs(myUser, universityId);
                myLoggedIn.Set(myClubs);
                return View("List", myLoggedIn);
            } catch (Exception myException) {
                LogError(myException, CLUB_LIST_ERROR);
                return SendToErrorPage(CLUB_LIST_ERROR);
            }
        }
    }
}
