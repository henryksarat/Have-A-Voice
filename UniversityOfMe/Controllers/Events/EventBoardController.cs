using System;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Services.Events;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Generic.Models;
using Social.Validation;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.Events;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Events {
    public class EventBoardController : UOFMeBaseController {
        private const string POSTED_TO_BOARD = "Posted to the events board!";

        private const string POSTING_TO_BOARD_ERROR = "An error occurred while posting to the event board. Please try again.";

        IEventService theEventService;

        public EventBoardController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())));
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
                    TempData["Message"] = POSTED_TO_BOARD;
                }
            } catch (Exception myException) {
                LogError(myException, POSTING_TO_BOARD_ERROR);
                TempData["Message"] = POSTING_TO_BOARD_ERROR;
            }

            return RedirectToAction("Details", "Event", new { id = eventId });
        }
    }
}
