﻿using System;
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
using Social.Admin.Exceptions;

namespace UniversityOfMe.Controllers.Events {
    public class EventController : UOFMeBaseController {
        private const string EVENT_CREATED = "The event has been created successfully!";
        private const string EVENT_EDITED = "The event has been edited successfully!";
        private const string GET_EVENT_ERROR = "Unable to get the event. Please try again.";
        private const string GET_EVENTS_ERROR = "Unable to get the events. Please try again.";
        private const string NO_EVENTS = "There are currently no events. Please create an event university wide.";
        private const string DELETE_ERROR = "Error deleting the event. Please try again.";
        private const string DELETE = "The event has been deleted.";
        private const string NO_EVENT = "No event exists with that id.";
        private const string ATTENDING = "You've been added to the guest list.";
        private const string UNATTENDING = "You've been removed from the the guest list.";
        private const string UNATTENDING_ERROR = "An error occurred while adding you to the guest list. Please try again.";
        private const string ATTENDING_ERROR = "An error occurred while removing you from the guest list. Please try again.";

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
                    TempData["Message"] += MessageHelper.SuccessMessage(EVENT_CREATED);
                    return RedirectToAction("Details", new { id = myResult.Id });
                }
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                ViewData["Message"] = MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
                theValidationDictionary.ForceModleStateExport();
            }

            return RedirectToAction("Create");
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Details(string universityId, int id) {
            try {
                UserInformationModel<User> myUser = GetUserInformatonModel();

                if (!UniversityHelper.IsFromUniversity(myUser.Details, universityId)) {
                    return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
                }

                Event myEvent = theEventService.GetEvent(myUser, universityId, id);

                if (myEvent == null) {
                    TempData["Message"] += MessageHelper.WarningMessage(NO_EVENT);
                    return RedirectToAction("List");
                }

                LoggedInWrapperModel<Event> myLoggedIn = new LoggedInWrapperModel<Event>(myUser.Details);
                myLoggedIn.Set(myEvent);

                return View("Details", myLoggedIn);
            } catch (Exception myException) {
                LogError(myException, GET_EVENT_ERROR);
                return SendToResultPage(GET_EVENT_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(string universityId, int id) {
            try {
                if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, universityId)) {
                    return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
                }

                theEventService.DeleteEvent(GetUserInformatonModel(), universityId, id);
                TempData["Message"] += MessageHelper.SuccessMessage(DELETE);

                return RedirectToAction("List");
            } catch(PermissionDenied) {
                TempData["Message"] += MessageHelper.WarningMessage(ErrorKeys.PERMISSION_DENIED);
            } catch (Exception myException) {
                LogError(myException, DELETE_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(DELETE_ERROR);
            }

            return RedirectToAction("Details", new { id = id });
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Edit(string universityId, int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                UserInformationModel<User> myUser = GetUserInformatonModel();

                if (!UniversityHelper.IsFromUniversity(myUser.Details, universityId)) {
                    return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
                }

                Event myEvent = theEventService.GetEventForEdit(myUser, universityId, id);
                IDictionary<string, string> myPrivacyOptions = theEventService.CreateAllPrivacyOptionsEntry();

                EventViewModel myEventView = new EventViewModel(myEvent) {
                    EventPrivacyOptions = new SelectList(myPrivacyOptions, "Value", "Key", myEvent.EntireSchool)
                };

                LoggedInWrapperModel<EventViewModel> myLoggedIn = new LoggedInWrapperModel<EventViewModel>(myUser.Details);
                myLoggedIn.Set(myEventView);

                return View("Edit", myLoggedIn);
            } catch (PermissionDenied) {
                TempData["Message"] += MessageHelper.WarningMessage(ErrorKeys.PERMISSION_DENIED);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] += MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            }

            return RedirectToAction("Details", new { id = id });
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
                    TempData["Message"] += MessageHelper.SuccessMessage(EVENT_EDITED);
                    return RedirectToAction("Details", new { id = anEvent.Id });
                }
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                ViewData["Message"] = MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
                theValidationDictionary.ForceModleStateExport();
            }

            return RedirectToAction("Edit", new { id = anEvent.Id });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult List(string universityId) {
            return RedirectToAction("Event", "Search", new { searchString = string.Empty, page = 1 });
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Attend(string universityId, int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                User myUser = GetUserInformatonModel().Details;

                if (!UniversityHelper.IsFromUniversity(myUser, universityId)) {
                    return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
                }

                theEventService.Attend(GetUserInformatonModel(), id);

                TempData["Message"] += MessageHelper.SuccessMessage(ATTENDING);

            } catch (Exception myException) {
                LogError(myException, ATTENDING_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(ATTENDING_ERROR);
            }

            return RedirectToAction("Details", new { id = id });
        }


        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Unattend(string universityId, int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                User myUser = GetUserInformatonModel().Details;

                if (!UniversityHelper.IsFromUniversity(myUser, universityId)) {
                    return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
                }

                theEventService.Unattend(GetUserInformatonModel(), id);

                TempData["Message"] += MessageHelper.SuccessMessage(UNATTENDING);

            } catch (Exception myException) {
                LogError(myException, UNATTENDING_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(UNATTENDING_ERROR);
            }

            return RedirectToAction("Details", new { id = id });
        }
    }
}
