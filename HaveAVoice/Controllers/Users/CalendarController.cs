
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
using HaveAVoice.Controllers.ActionFilters;
using HaveAVoice.Exceptions;
using System.Linq;

namespace HaveAVoice.Controllers.Users {
    public class CalendarController : HAVBaseController {
        private static string ADD_EVENT_SUCCESS = "Event added.";
        private static string DELETE_EVENT_SUCCESS = "Event deleted successfully!";
        private const string NO_EVENTS = "There are no upcoming events.";

        private const string USER_RETRIEVAL_ERROR = "Error retrieving the user.";
        private static string LOAD_EVENTS_ERROR = "Error loading the calendar. Please try again.";
        private static string ADD_EVENT_ERROR = "Error adding event. Please try again.";
        private static string DELETE_EVENT_ERROR = "Error deleting event. Please try again.";

        private static string LIST_VIEW = "List";

        private IHAVCalendarService theEventService;
        private IHAVUserRetrievalService theUserRetrievalService;

        public CalendarController()
            : base(new HAVBaseService(new HAVBaseRepository())) {
             ModelStateWrapper myModelWrapper = new ModelStateWrapper(this.ModelState);
             theEventService = new HAVCalendarService(myModelWrapper);
             theUserRetrievalService = new HAVUserRetrievalService();
        }

        public CalendarController(IHAVCalendarService aService, IHAVUserRetrievalService aUserRetrievalService, IHAVBaseService aBaseService)
            : base(aBaseService) {
            theEventService = aService;
            theUserRetrievalService = aUserRetrievalService;
        }

        [RequiredRouteValueAttribute.RequireRouteValues(new string[] { })]
        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult List() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            return GetEvents(myUser, myUser.Id);
        }

        [RequiredRouteValueAttribute.RequireRouteValues(new[] { "id" })]
        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult List(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            return GetEvents(myUser, id);
        }

        public ActionResult GetEvents(User aViewingUser, int aUserIdOfCalendar) {
            User myUserOfCalendar;
            try {
                myUserOfCalendar = theUserRetrievalService.GetUser(aUserIdOfCalendar);
            } catch (Exception e) {
                LogError(e, USER_RETRIEVAL_ERROR);
                return SendToErrorPage(LOAD_EVENTS_ERROR);
            }

            LoggedInListModel<Event> myLoggedInModel = new LoggedInListModel<Event>(myUserOfCalendar, aViewingUser, SiteSection.Calendar);
            try {
                myLoggedInModel.Models = theEventService.GetEventsForUser(aViewingUser, aUserIdOfCalendar);
                if (myLoggedInModel.Models.Count<Event>() == 0) {
                    ViewData["Message"] = MessageHelper.NormalMessage(NO_EVENTS);
                }
            } catch(NotFriendException e) {
                SendToErrorPage(HAVConstants.NOT_FRIEND);
            } catch (Exception e) {
                LogError(e, LOAD_EVENTS_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(LOAD_EVENTS_ERROR);
            }

            return View(LIST_VIEW, myLoggedInModel);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult AddEvent(DateTime date, string information) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            int myUserId = GetUserInformaton().Id;
            try {
                if (theEventService.AddEvent(myUserId, date, information)) {
                    TempData["Message"] = MessageHelper.SuccessMessage(ADD_EVENT_SUCCESS);
                }
            } catch (Exception e) {
                LogError(e, ADD_EVENT_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(ADD_EVENT_ERROR);
            }

            return RedirectToAction(LIST_VIEW);
        }

        public ActionResult DeleteEvent(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                theEventService.DeleteEvent(GetUserInformatonModel(), id);
                ViewData["Message"] = MessageHelper.SuccessMessage(DELETE_EVENT_SUCCESS);

            } catch (Exception e) {
                LogError(e, DELETE_EVENT_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(DELETE_EVENT_ERROR);
            }

            return RedirectToAction("List");
        }
    }
}
