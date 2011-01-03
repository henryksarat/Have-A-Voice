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

namespace HaveAVoice.Controllers.Users
{
    public class FanController : HAVBaseController {
        private const string APPROVED = "Fan approved!";
        private const string DECLINED = "Fan declined!";

        private const string FAN_ERROR = "Unable to become a fan. Please try again.";
        private static string FANS_ERROR = "Unable to get the fans list. Please try again.";
        private static string FANS_OF_ERROR = "Unable to get the people who are fans of this user. Please try again.";

        private const string ERROR_MESSAGE_VIEWDATA = "Message";
        private const string FANS_VIEW = "Fans";
        private const string LIST_VIEW = "List";
        private const string FANS_OF_VIEW = "FansOf";
        private const string PENDING_FANS_VIEW = "Pending";
      

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
        public ActionResult Create(int id) {
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

            return RedirectToAction("Show", "Profile", new { id = id });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult List() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            IEnumerable<Fan> myFans = new List<Fan>();
            try {
                myFans = theFanService.FindFansForUser(GetUserInformaton().Id);
            } catch (Exception e) {
                LogError(e, FANS_ERROR);
                ViewData[ERROR_MESSAGE_VIEWDATA] = FANS_ERROR;
            }

            return View(LIST_VIEW, myFans);
        }

        [AcceptVerbs(HttpVerbs.Get)]
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

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Pending() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            IEnumerable<Fan> myPendingFans = new List<Fan>();
            try {
                myPendingFans = theFanService.FindPendingFansForUser(GetUserInformaton().Id);
            } catch (Exception e) {
                LogError(e, HAVConstants.ERROR);
                ViewData[ERROR_MESSAGE_VIEWDATA] = HAVConstants.ERROR;
            }

            return View(PENDING_FANS_VIEW, myPendingFans);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Approve(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                theFanService.ApproveFan(id);
                TempData[ERROR_MESSAGE_VIEWDATA] = APPROVED;
            } catch (Exception e) {
                LogError(e, HAVConstants.ERROR);
                TempData[ERROR_MESSAGE_VIEWDATA] = HAVConstants.ERROR;
            }

            return RedirectToAction("Pending");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Decline(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                theFanService.DeclineFan(id);
                TempData[ERROR_MESSAGE_VIEWDATA] = DECLINED;
            } catch (Exception e) {
                LogError(e, HAVConstants.ERROR);
                TempData[ERROR_MESSAGE_VIEWDATA] = HAVConstants.ERROR;
            }

            return RedirectToAction("Pending");
        }
    }
}
