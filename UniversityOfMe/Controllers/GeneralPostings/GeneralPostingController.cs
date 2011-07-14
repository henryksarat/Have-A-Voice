using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Models;
using Social.Users.Services;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.SocialModels;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.GeneralPostings;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.GeneralPostings {
    public class GeneralPostingController : UOFMeBaseController {
        private const string NO_GENERAL_POSTING = "There are no general postings yet.";
        private const string SUBSCRIBED = "You have been subscribed to the general posting! You will now receive notifications when there are new replies.";
        private const string UNSUBSCRIBED = "You have been unsubscribed from the general posting and won't receive anymore notifications.";
        private const string UNSUBSCRIBED_ERROR = "An error occurred while unsubscribing you. Please try again.";
        private const string SUBSCRIBED_ERROR = "An error occurred while subscribing you. Please try again.";
        
        IGeneralPostingService theGeneralPostingService;
        IValidationDictionary theValidationDictionary;

        public GeneralPostingController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theGeneralPostingService = new GeneralPostingService(theValidationDictionary);
        }

        public ActionResult List(string universityId) {
            return RedirectToAction("GeneralPosting", "Search", new { searchString = string.Empty, page = 1 });
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Create(string universityId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, universityId)) {
                return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
            }

            try {
                LoggedInWrapperModel<GeneralPosting> myLoggedInModel = new LoggedInWrapperModel<GeneralPosting>(GetUserInformatonModel().Details);
                return View("Create", myLoggedInModel);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                return SendToErrorPage(ErrorKeys.ERROR_MESSAGE);
            }
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(string universityId, string title, string body) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                GeneralPosting myResult = theGeneralPostingService.CreateGeneralPosting(GetUserInformatonModel(), universityId, title, body);

                if (myResult != null) {
                    return RedirectToAction("Details", new { id = myResult.Id });
                }
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] += MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
                theValidationDictionary.ForceModleStateExport();
            }

            return RedirectToAction("Create");
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Details(string universityId, int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                UserInformationModel<User> myGeneralInformation = GetUserInformatonModel();
                LoggedInWrapperModel<GeneralPosting> myLoggedInModel = new LoggedInWrapperModel<GeneralPosting>(myGeneralInformation.Details);
                GeneralPosting myGeneralPosting = theGeneralPostingService.GetGeneralPosting(myGeneralInformation, id);
                myLoggedInModel.Set(myGeneralPosting);

                return View("Details", myLoggedInModel);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                return SendToResultPage(ErrorKeys.ERROR_MESSAGE);
            }
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Unsubscribe(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                theGeneralPostingService.Unsubscribe(GetUserInformatonModel(), id);

                TempData["Message"] += MessageHelper.SuccessMessage(UNSUBSCRIBED);

            } catch (Exception myException) {
                LogError(myException, UNSUBSCRIBED_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(UNSUBSCRIBED_ERROR);
            }

            return RedirectToAction("Details", new { id = id });
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Subscribe(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                theGeneralPostingService.Subscribe(GetUserInformatonModel(), id);

                TempData["Message"] += MessageHelper.SuccessMessage(SUBSCRIBED);

            } catch (Exception myException) {
                LogError(myException, UNSUBSCRIBED_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(SUBSCRIBED_ERROR);
            }

            return RedirectToAction("Details", new { id = id });
        }
    }
}
