using System;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Generic.Models;
using Social.Users.Services;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.Clubs;
using UniversityOfMe.Services.Professors;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Clubs {
    public class ClubBoardController : UOFMeBaseController {
        private const string POSTED_TO_BOARD = "Posted to the clubs board!";

        private const string POSTING_TO_BOARD_ERROR = "An error occurred while posting to the club board. Please try again.";

        IClubService theClubService;

        public ClubBoardController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            IValidationDictionary myModelState = new ModelStateWrapper(this.ModelState);
            theClubService = new ClubService(myModelState);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(string boardMessage, int clubId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            UserInformationModel<User> myUserInformation = GetUserInformatonModel();

            try {
                bool myResult = theClubService.PostToClubBoard(myUserInformation, clubId, boardMessage);
                if (myResult) {
                    TempData["Message"] = MessageHelper.SuccessMessage(POSTED_TO_BOARD);
                }
            } catch (Exception myException) {
                LogError(myException, POSTING_TO_BOARD_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(POSTING_TO_BOARD_ERROR);
            }

            return RedirectToAction("Details", "Club", new { id = clubId });
        }
    }
}
