﻿using System;
using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.UserFeatures;
using Social.Friend.Exceptions;
using Social.Generic.ActionFilters;
using Social.User.Services;
using Social.Validation;
using Social.Generic.Models;

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

        private IValidationDictionary theValidationDictionary;
        private IHAVCalendarService theEventService;
        private IUserRetrievalService<User> theUserRetrievalService;

        public CalendarController() {
             theValidationDictionary = new ModelStateWrapper(this.ModelState);
             theEventService = new HAVCalendarService(theValidationDictionary);
             theUserRetrievalService = new UserRetrievalService<User>(new EntityHAVUserRetrievalRepository());
        }

        public CalendarController(IHAVCalendarService aService, IUserRetrievalService<User> aUserRetrievalService) {
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

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult AddEvent(EventViewModel anEvent) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            
            try {
                UserInformationModel<User> myUser = GetUserInformatonModel();
                Event myEvent = theEventService.AddEvent(myUser, anEvent);
                if (myEvent != null) {
                    TempData["Message"] += MessageHelper.SuccessMessage(ADD_EVENT_SUCCESS);
                }
            } catch (Exception e) {
                LogError(e, ADD_EVENT_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(ADD_EVENT_ERROR);
                theValidationDictionary.ForceModleStateExport();
            }

            return RedirectToAction(LIST_VIEW);
        }

        public ActionResult DeleteEvent(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                theEventService.DeleteEvent(GetUserInformatonModel(), id);
                TempData["Message"] += MessageHelper.SuccessMessage(DELETE_EVENT_SUCCESS);

            } catch (Exception e) {
                LogError(e, DELETE_EVENT_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(DELETE_EVENT_ERROR);
            }

            return RedirectToAction("List");
        }

        private ActionResult GetEvents(User aViewingUser, int aUserIdOfCalendar) {
            User myUserOfCalendar;
            try {
                myUserOfCalendar = theUserRetrievalService.GetUser(aUserIdOfCalendar);
            } catch (Exception e) {
                LogError(e, USER_RETRIEVAL_ERROR);
                return SendToErrorPage(LOAD_EVENTS_ERROR);
            }

            LoggedInWrapperModel<EventViewModel> myLoggedInModel = new LoggedInWrapperModel<EventViewModel>(myUserOfCalendar, aViewingUser, SiteSection.Calendar);
            EventViewModel myModel = new EventViewModel();
            try {
                myModel.Results = theEventService.GetEventsForUser(aViewingUser, aUserIdOfCalendar);
                if (myModel.Results.Count<Event>() == 0) {
                    TempData["Message"] += MessageHelper.NormalMessage(NO_EVENTS);
                }

                myLoggedInModel.Set(myModel);
            } catch (NotFriendException e) {
                SendToErrorPage(HAVConstants.NOT_FRIEND);
            } catch (Exception e) {
                LogError(e, LOAD_EVENTS_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(LOAD_EVENTS_ERROR);
            }

            return View(LIST_VIEW, myLoggedInModel);
        }
    }
}
