﻿using System;
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
        private const string PAGE_LOAD_ERROR = "Unable to load the page. Please try again.";
        private const string UNABLE_TO_ADD_FILTER = "Unable to add the filter, please try again.";

        private const string NOT_LOGGED_IN = "NotLoggedIn";
        private const string FAN_FEED = "FanFeed";
        private const string OFFICIALS_FEED = "OfficialsFeed";
        private const string FILTERED_FEED = "FilteredFeed";

        private IHAVHomeService theService;

        public HomeController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
            theService = new HAVHomeService(new ModelStateWrapper(this.ModelState));
        }

        public HomeController(IHAVBaseService aBaseService, IHAVHomeService aService)
            : base(aBaseService) {
            theService = aService;
        }

        public ActionResult Index() {
            return View("Index");
        }

        public ActionResult NotLoggedIn() {
            NotLoggedInModel myModel = new NotLoggedInModel();

            try {
                myModel = theService.NotLoggedIn();
            } catch (Exception e) {
                LogError(e, PAGE_LOAD_ERROR);
                ViewData["Message"] = PAGE_LOAD_ERROR;
            }

            return View(NOT_LOGGED_IN, myModel);
        }

        public ActionResult FanFeed() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            
            LoggedInModel<FeedModel> myModel = new LoggedInModel<FeedModel>(new UserModel(myUser));
            try {
                myModel = theService.FanFeed(myUser);
            } catch (Exception e) {
                LogError(e, PAGE_LOAD_ERROR);
                ViewData["Message"] = PAGE_LOAD_ERROR;
            }

            return View(FAN_FEED, myModel);
        }

        public ActionResult OfficialsFeed() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            LoggedInModel<FeedModel> myModel = new LoggedInModel<FeedModel>(new UserModel(myUser));
            try {
                myModel = theService.OfficialsFeed(myUser);
            } catch (Exception e) {
                LogError(e, PAGE_LOAD_ERROR);
                ViewData["Message"] = PAGE_LOAD_ERROR;
            }

            return View(OFFICIALS_FEED, myModel);
        }

        public ActionResult FilteredFeed() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            LoggedInModel<FeedModel> myModel = new LoggedInModel<FeedModel>(new UserModel(myUser));
            try {
                myModel = theService.FilteredFeed(myUser);
            } catch (Exception e) {
                LogError(e, PAGE_LOAD_ERROR);
                ViewData["Message"] = PAGE_LOAD_ERROR;
            }

            return View(FILTERED_FEED, myModel);
        }

        public ActionResult AddCityStateFilter(string city, string state) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            LoggedInModel<FeedModel> myModel = new LoggedInModel<FeedModel>(new UserModel(myUser));
            try {
                myModel = theService.FanFeed(myUser);
                if (theService.AddCityStateFilter(myUser, city, state)) {
                    ViewData.ModelState.Remove("State");
                    ViewData.ModelState.Remove("City");
                }
            } catch (Exception e) {
                LogError(e, "Error adding the City/State filter.");
                return SendToErrorPage(UNABLE_TO_ADD_FILTER);
            }

            return View(FILTERED_FEED, myModel);
        }

        public ActionResult AddZipCodeFilter(string zipCode) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            User myUser = GetUserInformaton();
            LoggedInModel<FeedModel> myModel = new LoggedInModel<FeedModel>(new UserModel(myUser));
            try {
                myModel = theService.FanFeed(myUser);
                if (theService.AddZipCodeFilter(myUser, zipCode)) {
                    ViewData.ModelState.Remove("ZipCode");
                }
            } catch (Exception e) {
                LogError(e, "Error adding the Zip Code filter.");
                return SendToErrorPage(UNABLE_TO_ADD_FILTER);
            }

            return View(FILTERED_FEED, myModel);
        }
    }
}
