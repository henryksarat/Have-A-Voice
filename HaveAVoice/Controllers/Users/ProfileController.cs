using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Controllers.Admin;
using HaveAVoice.Models.Services.UserFeatures;
using HaveAVoice.Models.Validation;
using HaveAVoice.Models.Repositories;
using HaveAVoice.Models.Services;
using HaveAVoice.Models;
using System.Text;
using HaveAVoice.Models.View;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Controllers.ActionFilters;

namespace HaveAVoice.Controllers.Users
{
    public class ProfileController : HAVBaseController {
        private static string USER_PAGE_ERROR = "Unable to view the user page.";
        private static string USER_PAGE_ERROR_POLITE = USER_PAGE_ERROR + PLEASE_TRY_AGAIN;
        private static string PLEASE_TRY_AGAIN = " Please try again.";
        private static string FANS_ERROR = "Unable to get the fans list";
        private static string FANS_OF_ERROR = "Unable to get the people who are fans of this user.";

        private IHAVProfileService theService;

        public ProfileController()
            : base(new HAVBaseService(new HAVBaseRepository())) {
            theService = new HAVProfileService(new ModelStateWrapper(this.ModelState));
        }

        public ProfileController(IHAVProfileService aService, IHAVBaseService aBaseService)
            : base(aBaseService) {
            theService = aService;
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Show(int id) {
            User myViewingUser = GetUserInformaton();
            try {
                ProfileModel myProfile = theService.Profile(id, myViewingUser).Build();
                return View("Show", myProfile);
            } catch (Exception e) {
                LogError(e, USER_PAGE_ERROR);
                return SendToErrorPage(USER_PAGE_ERROR_POLITE);
            }
        }

        public ActionResult Fans(int id) {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }
            IEnumerable<Fan> myFans = new List<Fan>();
            try {
                myFans = theService.FindFansForUser(id);
            } catch (Exception e) {
                string myError = ErrorHelper.ErrorString("{0}. [userId={1}]", FANS_ERROR, id);
                LogError(e, myError);
                ViewData["Message"] = FANS_ERROR + PLEASE_TRY_AGAIN;
            }

            return View("Fans", myFans);
        }

        public ActionResult FansOf(int id) {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }
            IEnumerable<Fan> myFans = new List<Fan>();
            try {
                myFans = theService.FindFansOfUser(id);
            } catch (Exception e) {
                string myError = ErrorHelper.ErrorString("{0}. [userId={1}]", FANS_OF_ERROR, id);
                LogError(e, myError);
                ViewData["Message"] = FANS_OF_ERROR + PLEASE_TRY_AGAIN;
            }

            return View("FansOf", myFans);
        }


        public override ActionResult SendToResultPage(string aTitle, string aDetails) {
            return SendToResultPage(SiteSectionsEnum.Profile, aTitle, aDetails);
        }
    }
}
