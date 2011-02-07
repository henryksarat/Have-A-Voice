﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Services;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Repositories;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers;
using HaveAVoice.Controllers.ActionFilters;

namespace HaveAVoice.Controllers.Users {
    public class FanController : HAVBaseController {
        private const string ALREADY_FAN = "You are already a fan of this user.";
        private const string NOT_FAN = "You are not a fan of the user so you can't de-fan them.";
        private const string FAN_SUCCESS = "You have become a fan!";
        private const string FAN_REMOVE_SUCCESS = "You are no longer the users fan.";

        private const string FAN_ERROR = "An error occurred while trying to become a fan of the user. Please try again.";
        private const string FAN_REMOVE_ERROR = "An error occurred while trying to remove you as a fan from the user. Please try again.";
        private const string FAN_LIST_ERROR = "AN error occurred while trying to get the list of everyone that is a fan of you. Please try again.";

        private const string LIST_VIEW = "List";

        private IHAVFanService theFanService;

        public FanController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
                theFanService = new HAVFanService();
        }

        public FanController(IHAVBaseService aBaseService, IHAVFanService aFanService)
            : base(aBaseService) {
                theFanService = aFanService;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Add(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();

            try {
                if (!theFanService.IsFan(myUser, id)) {
                    theFanService.Add(myUser, id);
                    RefreshUserInformation();
                    TempData["Message"] = FAN_SUCCESS;
                } else {
                    TempData["Message"] = ALREADY_FAN;
                }
            } catch (Exception e) {
                LogError(e, FAN_ERROR);
                TempData["Message"] = FAN_ERROR;
            }

            return RedirectToProfile(id);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Remove(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();

            try {
                if (theFanService.IsFan(myUser, id)) {
                    theFanService.Remove(myUser, id);
                    TempData["Message"] = FAN_REMOVE_SUCCESS;
                } else {
                    TempData["Message"] = NOT_FAN;
                }
            } catch (Exception e) {
                LogError(e, FAN_ERROR);
                TempData["Message"] = FAN_REMOVE_ERROR;
            }

            return RedirectToProfile(id);
        }

        [RequiredRouteValueAttribute.RequireRouteValues(new string[] { })]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult List() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            LoggedInListModel<Fan> myLoggedInModel = new LoggedInListModel<Fan>(myUser, SiteSection.Friend);
            try {
                myLoggedInModel.Models = theFanService.GetAllFansForUser(myUser);
            } catch (Exception e) {
                LogError(e, FAN_LIST_ERROR);
                return SendToErrorPage(FAN_LIST_ERROR);
            }

            return View(LIST_VIEW, myLoggedInModel);
        }
    }
}
