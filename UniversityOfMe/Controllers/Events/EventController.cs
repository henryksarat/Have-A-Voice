using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Models;
using Social.Users.Services;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.Events;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Events {
    public class EventController : UOFMeBaseController {
        private const string EVENT_CREATED = "The event has been created successfully!";
        private const string EVENT_EDITED = "The event has been edited successfully!";
        private const string GET_EVENT_ERROR = "Unable to get the event. Please try again.";
        private const string GET_EVENTS_ERROR = "Unable to get the events. Please try again.";
        private const string NO_EVENTS = "There are currently no events. Please create an event university wide.";
        private const string DELETE_ERROR = "Error deleting the event. Please try again.";
        private const string NO_EVENT = "No event exists with that id.";

        IEventService theEventService;
        IValidationDictionary theValidationDictionary;

        public EventController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theEventService = new EventService(theValidationDictionary);
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

                IDictionary<string, string> myPrivacyOptions = theEventService.CreateAllPrivacyOptionsEntry();
                EventViewModel myEventViewModel = new EventViewModel() {
                    EventPrivacyOptions = new SelectList(myPrivacyOptions, "Value", "Key")
                };

                LoggedInWrapperModel<EventViewModel> myLoggedIn = new LoggedInWrapperModel<EventViewModel>(myUser);
                myLoggedIn.Set(myEventViewModel);

                return View("Create", myLoggedIn);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                return SendToErrorPage(ErrorKeys.ERROR_MESSAGE);
            }
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(EventViewModel anEvent) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, anEvent.UniversityId)) {
                return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
            }

            UserInformationModel<User> myUserInformation = GetUserInformatonModel();

            try {
                Event myResult = theEventService.CreateEvent(myUserInformation, anEvent);

                if (myResult != null) {
                    TempData["Message"] = EVENT_CREATED;
                    return RedirectToAction("Details", new { id = myResult.Id });
                }
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                ViewData["Message"] = ErrorKeys.ERROR_MESSAGE;
                theValidationDictionary.ForceModleStateExport();
            }

            return RedirectToAction("Create");
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Details(string universityId, int id) {
            try {
                User myUser = GetUserInformatonModel().Details;

                if (!UniversityHelper.IsFromUniversity(myUser, universityId)) {
                    return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
                }

                Event myEvent = theEventService.GetEvent(universityId, id);

                if (myEvent == null) {
                    TempData["Message"] = NO_EVENT;
                    return RedirectToAction("List");
                }

                LoggedInWrapperModel<Event> myLoggedIn = new LoggedInWrapperModel<Event>(myUser);
                myLoggedIn.Set(myEvent);

                return View("Details", myLoggedIn);
            } catch (Exception myException) {
                LogError(myException, GET_EVENT_ERROR);
                return SendToResultPage(GET_EVENT_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string universityId, int id) {
            try {
                if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, universityId)) {
                    return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
                }

                theEventService.DeleteEvent(GetUserInformatonModel(), universityId, id);
                TempData["Message"] = DELETE_ERROR;

                return RedirectToAction("List");
            } catch (Exception myException) {
                LogError(myException, DELETE_ERROR);
                TempData["Message"] = DELETE_ERROR;
                return RedirectToAction("Details", new { id = id });
            }
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Edit(string universityId, int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                User myUser = GetUserInformatonModel().Details;

                if (!UniversityHelper.IsFromUniversity(myUser, universityId)) {
                    return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
                }

                Event myEvent = theEventService.GetEvent(universityId, id);
                IDictionary<string, string> myPrivacyOptions = theEventService.CreateAllPrivacyOptionsEntry();

                EventViewModel myEventView = new EventViewModel(myEvent) {
                    EventPrivacyOptions = new SelectList(myPrivacyOptions, "Value", "Key")
                };

                LoggedInWrapperModel<EventViewModel> myLoggedIn = new LoggedInWrapperModel<EventViewModel>(myUser);
                myLoggedIn.Set(myEventView);

                return View("Edit", myLoggedIn);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                return SendToErrorPage(ErrorKeys.ERROR_MESSAGE);
            }
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Edit(EventViewModel anEvent) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, anEvent.UniversityId)) {
                return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
            }

            try {
                UserInformationModel<User> myUserInformation = GetUserInformatonModel();
                bool myResult = theEventService.EditEvent(myUserInformation, anEvent);

                if (myResult) {
                    TempData["Message"] = EVENT_EDITED;
                    return RedirectToAction("Details", new { id = anEvent.Id });
                }
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                ViewData["Message"] = ErrorKeys.ERROR_MESSAGE;
                theValidationDictionary.ForceModleStateExport();
            }

            return RedirectToAction("Edit", new { id = anEvent.Id });
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

                IEnumerable<Event> myEvents = new List<Event>();
            
                myEvents = theEventService.GetEventsForUniversity(myUser, universityId);

                if (myEvents.Count<Event>() == 0) {
                    TempData["Message"] = NO_EVENTS;
                }

                LoggedInListModel<Event> myLoggedIn = new LoggedInListModel<Event>(myUser);
                myLoggedIn.Set(myEvents);

                return View("List", myLoggedIn);
            } catch (Exception myException) {
                LogError(myException, GET_EVENTS_ERROR);
                return SendToErrorPage(GET_EVENTS_ERROR);
            }
        }
    }
}
