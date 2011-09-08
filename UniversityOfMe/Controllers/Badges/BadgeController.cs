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
using UniversityOfMe.Services.Users;

namespace UniversityOfMe.Controllers.Badges {
    public class BadgeController : UOFMeBaseController {
        private const string BADGES_RETRIEVAL_ERROR = "An error occurred while getting your badges.";
        private const string HIDE_BADGE_ERROR = "Unable to hide the badge. Please try again later.";

        private IUofMeUserRetrievalService theUserRetrievalService;
        private IBadgeService theBadgeService;

        public BadgeController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theBadgeService = new BadgeService();
            theUserRetrievalService = new UofMeUserRetrievalService();
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult List() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUserInformation = GetUserInformatonModel();
                IEnumerable<Badge> myBadges = theBadgeService.GetBadgesForUser(myUserInformation.Details);
                LoggedInWrapperModel<SomethingListWithUser<Badge>> myLoggedInModel = new LoggedInWrapperModel<SomethingListWithUser<Badge>>(myUserInformation.Details);
                SomethingListWithUser<Badge> myListedBadges = new SomethingListWithUser<Badge>() {
                    ListedItems = myBadges,
                    TargetUser = myUserInformation.Details
                };
                myLoggedInModel.Set(myListedBadges);
                return View("List", myLoggedInModel);
            } catch (Exception myException) {
                LogError(myException, BADGES_RETRIEVAL_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(BADGES_RETRIEVAL_ERROR);
            }

            return RedirectToProfile();
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult ListBadgesForUser(int userId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUserInformation = GetUserInformatonModel();
                User myUser = theUserRetrievalService.GetUser(userId);
                IEnumerable<Badge> myBadges = theBadgeService.GetBadgesForUser(myUser);
                LoggedInWrapperModel<SomethingListWithUser<Badge>> myLoggedInModel = new LoggedInWrapperModel<SomethingListWithUser<Badge>>(myUserInformation.Details);
                SomethingListWithUser<Badge> myListedBadges = new SomethingListWithUser<Badge>() {
                    ListedItems = myBadges,
                    TargetUser = myUserInformation.Details
                };
                myLoggedInModel.Set(myListedBadges);
                return View("List", myLoggedInModel);
            } catch (Exception myException) {
                LogError(myException, BADGES_RETRIEVAL_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(BADGES_RETRIEVAL_ERROR);
            }

            return RedirectToProfile();
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Hide(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUserInformation = GetUserInformatonModel();
                theBadgeService.MarkBadgeAsSeen(myUserInformation, id);
            } catch (Exception myException) {
                LogError(myException, HIDE_BADGE_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(HIDE_BADGE_ERROR);
            }

            return RedirectToProfile();
        }
    }
}
