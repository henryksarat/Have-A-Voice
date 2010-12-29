
using System;
using System.Web.Mvc;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Helpers;
using HaveAVoice.Validation;
using HaveAVoice.Repositories;
using HaveAVoice.Services;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Controllers.Helpers;
using System.Collections.Generic;

namespace HaveAVoice.Controllers.Users
{
    public class CalendarController : HAVBaseController {
        private static string ADD_EVENT_SUCCESS = "Event added.";
        private static string DELETE_EVENT_SUCCESS = "Event deleted successfully!";

        private static string LOAD_EVENTS_ERROR = "Error loading the calendar. Please try again.";
        private static string ADD_EVENT_ERROR = "Error adding event. Please try again.";
        private static string DELETE_EVENT_ERROR = "Error deleting event. Please try again.";

        private static string LIST_VIEW = "List";
        private static string SHOW_VIEW = "Show";


        private IHAVCalendarService theEventService;

        public CalendarController()
            : base(new HAVBaseService(new HAVBaseRepository())) {
             ModelStateWrapper myModelWrapper = new ModelStateWrapper(this.ModelState);
             theEventService = new HAVCalendarService(myModelWrapper);
        }

        public CalendarController(IHAVCalendarService aService, IHAVBaseService aBaseService)
            : base(aBaseService) {
            theEventService = aService;
        }

        public ActionResult List() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            return GetEvents(myUser, myUser.Id, LIST_VIEW);
        }

        public ActionResult Show(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            return GetEvents(myUser, id, SHOW_VIEW);
        }

        public ActionResult GetEvents(User aViewingUser, int aUserId, string aViewName) {
            IEnumerable<Event> myList = new List<Event>();
            try {

                myList = theEventService.GetEventsForUser(aViewingUser, aUserId);
            } catch (Exception e) {
                LogError(e, LOAD_EVENTS_ERROR);
                ViewData["Message"] = LOAD_EVENTS_ERROR;
            }

            return View(aViewName, myList);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddEvent(DateTime date, string information) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            int myUserId = GetUserInformaton().Id;
            try {
                if (theEventService.AddEvent(myUserId, date, information)) {
                    ViewData.ModelState.Remove("Information");
                    ViewData["Message"] = ADD_EVENT_SUCCESS;
                }
            } catch (Exception e) {
                string myError = ErrorHelper.ErrorString("{0} [UserId={1};]", ADD_EVENT_ERROR, myUserId);
                LogError(e, myError);
                ViewData["Message"] = ADD_EVENT_ERROR;
            }

            return RedirectToAction("Show");
        }

        public ActionResult DeleteEvent(int eventId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                theEventService.DeleteEvent(GetUserInformatonModel(), eventId);
                ViewData["Message"] = DELETE_EVENT_SUCCESS;

            } catch (Exception e) {
                string myError = ErrorHelper.ErrorString("{0} [EventId={1};]", DELETE_EVENT_ERROR, eventId);
                LogError(e, myError);
                ViewData["Message"] = ADD_EVENT_ERROR;
            }

            return RedirectToAction("Show");
        }


    }
}
