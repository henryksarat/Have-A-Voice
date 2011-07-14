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

namespace UniversityOfMe.Controllers.Site {
    public class FeatureController : UOFMeBaseController {
        private const string FEATURES_UPDATED = "Your site feature settings have been updated successfully!";

        private const string FEATURE_RETRIEVAL_ERROR = "An error occurred while retreiving the site features. Please try again.";
        private const string FEATURES_UPDATED_ERROR = "An error occurred while updating your site feature settings. Please try again.";

        private IFeatureService theFeatureService;

        public FeatureController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theFeatureService = new FeatureService();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUserInformation = GetUserInformatonModel();
                IEnumerable<Pair<Feature, bool>> myFeatures = theFeatureService.GetFeatureSettingsForUser(myUserInformation);
                LoggedInListModel<Pair<Feature, bool>> myLoggedInModel = new LoggedInListModel<Pair<Feature, bool>>(myUserInformation.Details);
                myLoggedInModel.Set(myFeatures);
                return View("Edit", myLoggedInModel);
            } catch (Exception myException) {
                LogError(myException, FEATURE_RETRIEVAL_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(FEATURE_RETRIEVAL_ERROR);
            }

            return RedirectToProfile();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        new public ActionResult Edit(UpdateFeaturesModel aSettings) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUserInfo = GetUserInformatonModel();
                theFeatureService.UpdateFeatureSettings(myUserInfo, aSettings);
                TempData["Message"] += MessageHelper.SuccessMessage(FEATURES_UPDATED);
                ForceUserInformationRefresh();
            } catch (Exception myException) {
                LogError(myException, FEATURE_RETRIEVAL_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(FEATURES_UPDATED_ERROR);
            }

            return RedirectToAction("Edit");
        }
    }
}
