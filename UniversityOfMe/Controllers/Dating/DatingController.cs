using System;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Users.Services;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.Dating;
using UniversityOfMe.UserInformation;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Controllers.Dating {
    public class DatingController : UOFMeBaseController {
        private const string DATING_RESPONSE_CREATED = "You response if you wanted to date the user was saved.";
        private const string DATING_MATCH_SEEN = "You marked the dating match as seen. It will not display any more.";


        IDatingService theDatingService;

        public DatingController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theDatingService = new DatingService();
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(int sourceUserId, bool response) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                theDatingService.AddDatingResult(sourceUserId, GetUserInformatonModel().Details.Id, response);
                TempData["Message"] = MessageHelper.SuccessMessage(DATING_RESPONSE_CREATED);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] = MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            }

            return RedirectToProfile();
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult MarkAsSeen(int datingLogId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                theDatingService.MarkDatingLogAsSeenBySourceUser(GetUserInformatonModel().Details, datingLogId);
                TempData["Message"] = MessageHelper.SuccessMessage(DATING_MATCH_SEEN);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] = MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            }

            return RedirectToProfile();
        }
    }
}
