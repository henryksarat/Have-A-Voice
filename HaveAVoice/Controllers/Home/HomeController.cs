using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Models.View;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Services;
using HaveAVoice.Repositories;
using HaveAVoice.Validation;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models;

namespace HaveAVoice.Controllers.Home {
    public class HomeController : HAVBaseController {
        private static string PAGE_LOAD_ERROR = "Unable to load the page. Please try again.";
        private static string UNABLE_TO_ADD_FILTER = "Unable to add the filter, please try again.";
        private IHAVHomeService theService;

        public HomeController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
            theService = new HAVHomeService(new ModelStateWrapper(this.ModelState));
        }

        public HomeController(IHAVHomeService aService, IHAVBaseService baseService)
            : base(baseService) {
            theService = aService;
        }

        public ActionResult NotLoggedIn() {
            NotLoggedInModel myModel = new NotLoggedInModel();

            try {
                myModel = theService.NotLoggedIn();
            } catch (Exception e) {
                LogError(e, PAGE_LOAD_ERROR);
                ViewData["Message"] = PAGE_LOAD_ERROR;
            }

            return View("NotLoggedIn", myModel);
        }

        public ActionResult LoggedIn() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            LoggedInModel myModel = new LoggedInModel();
            User myUser = GetUserInformaton();
            try {
                myModel = theService.LoggedIn(myUser);
            } catch (Exception e) {
                LogError(e, PAGE_LOAD_ERROR);
                ViewData["Message"] = PAGE_LOAD_ERROR;
            }

            return View("LoggedIn", myModel);
        }

        public ActionResult AddCityStateFilter(string city, string state) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            LoggedInModel myModel = new LoggedInModel();
            User myUser = GetUserInformaton();
            try {
                myModel = theService.LoggedIn(myUser);
                if (theService.AddCityStateFilter(myUser, city, state)) {
                    ViewData.ModelState.Remove("State");
                    ViewData.ModelState.Remove("City");
                }
            } catch (Exception e) {
                LogError(e, "Error adding the City/State filter.");
                return SendToErrorPage(UNABLE_TO_ADD_FILTER);
            }

            return View("LoggedIn", myModel);
        }

        public ActionResult AddZipCodeFilter(string zipCode) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            LoggedInModel myModel = new LoggedInModel();
            User myUser = GetUserInformaton();
            try {
                myModel = theService.LoggedIn(myUser);
                if (theService.AddZipCodeFilter(myUser, zipCode)) {
                    ViewData.ModelState.Remove("ZipCode");
                }
            } catch (Exception e) {
                LogError(e, "Error adding the Zip Code filter.");
                return SendToErrorPage(UNABLE_TO_ADD_FILTER);
            }

            return View("LoggedIn", myModel);
        }

        protected override ActionResult SendToResultPage(string title, string details) {
            throw new NotImplementedException();
        }
    }
}
