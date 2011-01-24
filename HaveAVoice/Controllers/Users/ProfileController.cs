using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Controllers.Admin;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Validation;
using HaveAVoice.Repositories;
using HaveAVoice.Services;
using HaveAVoice.Models;
using System.Text;
using HaveAVoice.Models.View;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Controllers.ActionFilters;
using HaveAVoice.Exceptions;

namespace HaveAVoice.Controllers.Users {
    public class ProfileController : HAVBaseController {
        private static string USER_PAGE_ERROR = "Unable to view the user page.";
        private static string USER_PAGE_ERROR_POLITE = USER_PAGE_ERROR + PLEASE_TRY_AGAIN;
        private static string PLEASE_TRY_AGAIN = " Please try again.";
        private const string USER_ACTIVITY_ERROR = "Unable to get the user's issue activity. Please try again.";

        private IHAVProfileService theService;

        public ProfileController()
            : base(new HAVBaseService(new HAVBaseRepository())) {
            theService = new HAVProfileService(new ModelStateWrapper(this.ModelState));
        }

        public ProfileController(IHAVProfileService aService, IHAVBaseService aBaseService)
            : base(aBaseService) {
            theService = aService;
        }

        [RequiredRouteValueAttribute.RequireRouteValues(new[] { "id" })]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Show(int id) {
            User myViewingUser = GetUserInformaton();
            
            try {
                UserProfileModel myProfile = theService.Profile(id, myViewingUser);
                LoggedInWrapperModel<UserProfileModel> myModel = new LoggedInWrapperModel<UserProfileModel>(myProfile.User, SiteSection.Profile);
                myModel.Model = myProfile;
                return View("Show", myModel);
            } catch (Exception e) {
                LogError(e, USER_PAGE_ERROR);
                return SendToErrorPage(USER_PAGE_ERROR_POLITE);
            }
        }

        [RequiredRouteValueAttribute.RequireRouteValues(new string[] { })]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Show() {
            User myUser = GetUserInformaton();
            LoggedInWrapperModel<UserProfileModel> myModel = new LoggedInWrapperModel<UserProfileModel>(myUser, SiteSection.MyProfile);

            try {
                myModel.Model = theService.MyProfile(myUser);
            } catch (Exception e) {
                LogError(e, USER_PAGE_ERROR);
                ViewData["Message"] = USER_PAGE_ERROR_POLITE;
            }

                return View("Show", myModel);
        }

        [RequiredRouteValueAttribute.RequireRouteValues(new[] { "id" })]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult IssueActivity(int id) {
            User myUser = GetUserInformaton();
            
            try {
                UserProfileModel myProfile = theService.UserIssueActivity(id, myUser);
                LoggedInWrapperModel<UserProfileModel> myModel = new LoggedInWrapperModel<UserProfileModel>(myProfile.User, SiteSection.IssueActivity);
                myModel.Model = myProfile;
                return View("IssueActivity", myModel);
            } catch (CustomException e) {
                return SendToErrorPage(e.Message);
            } catch (Exception e) {
                LogError(e, USER_ACTIVITY_ERROR);
                return SendToErrorPage(USER_ACTIVITY_ERROR);
            }
        }

        [RequiredRouteValueAttribute.RequireRouteValues(new string[] { })]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult IssueActivity() {
            User myUser = GetUserInformaton();
            LoggedInWrapperModel<UserProfileModel> myModel = new LoggedInWrapperModel<UserProfileModel>(myUser, SiteSection.IssueActivity);

            try {
                myModel.Model = theService.UserIssueActivity(myUser.Id, myUser);
            } catch (CustomException e) {
                return SendToErrorPage(e.Message);
            } catch (Exception e) {
                LogError(e, USER_ACTIVITY_ERROR);
                ViewData["Message"] = USER_ACTIVITY_ERROR;
            }

            return View("IssueActivity", myModel);
        }
    }
}
