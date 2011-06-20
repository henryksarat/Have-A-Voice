using System;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Generic.Models;
using Social.User.Services;
using Social.Users.Services;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Repositories.UserRepos;
using UniversityOfMe.Services.UserFeatures;
using UniversityOfMe.UserInformation;
using Social.Generic.Constants;

namespace UniversityOfMe.Controllers.Profile {
    public class ProfileController : UOFMeBaseController {
        private const string USER_PAGE_ERROR = "Unable to view the user page. Please try again.";
        private const string INVALID_SHORT_URL = "No user is assigned with that web address. Verify the spelling and try again.";

        private const string PROFILE_VIEW = "Show";

        private const string FEATURE_UPDATED = "Your preferences have been updated!";

        private IUserRetrievalService<User> theUserRetrievalService;
        private IFeatureService theFeatureService;
        
        public ProfileController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theUserRetrievalService = new UserRetrievalService<User>(new EntityUserRetrievalRepository());
            theFeatureService = new FeatureService();
        }

        [RequiredRouteValueAttribute.RequireRouteValues(new[] { "shortName" })]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Show(string shortName) {
            try {
                UserInformationModel<User> myViewingUser = GetUserInformatonModel();
                User myProfile = theUserRetrievalService.GetUserByShortUrl(shortName);
                if (myProfile == null) {
                    return SendToErrorPage(INVALID_SHORT_URL);
                }

                return View(PROFILE_VIEW, myProfile);
            } catch (Exception e) {
                LogError(e, USER_PAGE_ERROR);
                return SendToErrorPage(USER_PAGE_ERROR);
            }
        }

        [RequiredRouteValueAttribute.RequireRouteValues(new[] { "id" })]
        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Show(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myViewingUser = GetUserInformatonModel();
                
                User myUser = theUserRetrievalService.GetUser(id);

                return View(PROFILE_VIEW, myUser);
            } catch (Exception e) {
                LogError(e, USER_PAGE_ERROR);
                return SendToErrorPage(USER_PAGE_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult DisableFeature(Features feature) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                theFeatureService.DisableFeature(GetUserInformatonModel().Details, feature);
                ForceUserInformationRefresh();
                TempData["Message"] = MessageHelper.SuccessMessage(FEATURE_UPDATED);
            } catch (Exception e) {
                LogError(e, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] = MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            }

            return RedirectToAction("Main", "University", new { universityId = UniversityHelper.GetMainUniversity(GetUserInformatonModel().Details).Id });
        }
    }
}
