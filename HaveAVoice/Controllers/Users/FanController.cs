using System;
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
        private const string FAN_SUCCESS = "You have become a fan!";

        private const string FAN_ERROR = "An error occurred while trying to become a fan of the user. Please try again.";
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
                    return SendToResultPage(FAN_SUCCESS);
                } else {
                    return SendToErrorPage(ALREADY_FAN);
                }
            } catch (Exception e) {
                LogError(e, FAN_ERROR);
                return SendToErrorPage(FAN_ERROR);
            }
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
