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

namespace HaveAVoice.Controllers.Users
{
    public class FanController : HAVBaseController {
        private const string FAN_ERROR = "Unable to become a fan. Please try again.";
        private static string FANS_ERROR = "Unable to get the fans list. Please try again.";
        private static string FANS_OF_ERROR = "Unable to get the people who are fans of this user. Please try again.";

        private const string ERROR_MESSAGE_VIEWDATA = "Message";
        private const string FANS_VIEW = "Fans";
        private const string FANS_OF_VIEW = "FansOf";
      

        private IHAVFanService theFanService;

        public FanController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
                theFanService = new HAVFanService();
        }

        public FanController(IHAVBaseService aBaseService, IHAVFanService aFanService)
            : base(aBaseService) {
                theFanService = aFanService;
        }

        public ActionResult BecomeAFan(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();

            try {
                theFanService.AddFan(myUser, id);
            } catch (Exception e) {
                LogError(e, FAN_ERROR);
                return SendToErrorPage(FAN_ERROR);
            }

            return RedirectToAction("Show", "Profile", id);
        }

        public ActionResult Fans(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            IEnumerable<Fan> myFans = new List<Fan>();
            try {
                myFans = theFanService.FindFansForUser(id);
            } catch (Exception e) {
                LogError(e, FANS_ERROR);
                ViewData[ERROR_MESSAGE_VIEWDATA] = FANS_ERROR;
            }

            return View(FANS_VIEW, myFans);
        }

        public ActionResult FansOf(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            IEnumerable<Fan> myFans = new List<Fan>();
            try {
                myFans = theFanService.FindFansOfUser(id);
            } catch (Exception e) {
                LogError(e, FANS_OF_ERROR);
                ViewData[ERROR_MESSAGE_VIEWDATA] = FANS_OF_ERROR;
            }

            return View(FANS_OF_VIEW, myFans);
        }

        protected override ActionResult SendToResultPage(string aTitle, string aDetails) {
            return SendToResultPage(SiteSectionsEnum.User, aTitle, aDetails);
        }
    }
}
