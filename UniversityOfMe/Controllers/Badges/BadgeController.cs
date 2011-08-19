using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic;
using Social.Generic.ActionFilters;
using Social.Users.Services;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.UserFeatures;
using UniversityOfMe.UserInformation;
using Social.Generic.Models;
using UniversityOfMe.Models.View;
using System;
using UniversityOfMe.Services.Badges;

namespace UniversityOfMe.Controllers.Badges {
    public class BadgeController : UOFMeBaseController {
        private const string BADGES_RETRIEVAL_ERROR = "An error occurred while getting your badges.";

        private IBadgeService theBadgeService;

        public BadgeController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theBadgeService = new BadgeService();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult List() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUserInformation = GetUserInformatonModel();
                IEnumerable<Badge> myBadges = theBadgeService.GetBadgesForUser(myUserInformation.Details);
                LoggedInListModel<Badge> myLoggedInModel = new LoggedInListModel<Badge>(myUserInformation.Details);
                myLoggedInModel.Set(myBadges);
                return View("List", myLoggedInModel);
            } catch (Exception myException) {
                LogError(myException, BADGES_RETRIEVAL_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(BADGES_RETRIEVAL_ERROR);
            }

            return RedirectToProfile();
        }
    }
}
