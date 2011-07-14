using System;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Generic.Models;
using Social.Users.Services;
using Social.Validation;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.Events;
using UniversityOfMe.UserInformation;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Controllers.Events {
    public class EventBoardController : UOFMeBaseController {
        private const string POSTED_TO_BOARD = "Posted to the events board!";

        private const string POSTING_TO_BOARD_ERROR = "An error occurred while posting to the event board. Please try again.";

        IEventService theEventService;

        public EventBoardController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            IValidationDictionary myModelState = new ModelStateWrapper(this.ModelState);
            theEventService = new EventService(myModelState);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(string boardMessage, int eventId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            UserInformationModel<User> myUserInformation = GetUserInformatonModel();

            try {
                bool myResult = theEventService.PostToEventBoard(myUserInformation, eventId, boardMessage);
                if (myResult) {
                    TempData["Message"] += MessageHelper.SuccessMessage(POSTED_TO_BOARD);
                }
            } catch (Exception myException) {
                LogError(myException, POSTING_TO_BOARD_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(POSTING_TO_BOARD_ERROR);
            }

            return RedirectToAction("Details", "Event", new { id = eventId });
        }
    }
}
