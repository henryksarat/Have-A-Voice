﻿using System;
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
using HaveAVoice.Helpers;

namespace HaveAVoice.Controllers.Users {
    public class ProfileController : HAVBaseController {
        private const string EMPTY_FEED = "You have nothing in your feed. Make some friends to start seeing activity here!";
        private const string EMPTY_PROFILE = "The user has no activity.";
        private const string MY_EMPTY_ISSUE_ACTIVITY = "You have not participated in any issues. Go out and start posting!";
        private const string EMPTY_ISSUE_ACTIVITY = "The user has yet to create any issues and reply to any exisiting issues.";

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

        [RequiredRouteValueAttribute.RequireRouteValues(new[] { "shortName" })]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Show(string shortName) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myViewingUser = GetUserInformaton();

            try {
                UserProfileModel myProfile = theService.Profile(3, myViewingUser);
                LoggedInWrapperModel<UserProfileModel> myModel = new LoggedInWrapperModel<UserProfileModel>(myProfile.User, SiteSection.Profile);
                myModel.Model = myProfile;

                if (myModel.Model.IsEmpty()) {
                    ViewData["Message"] = MessageHelper.NormalMessage(EMPTY_PROFILE);
                }

                return View("Show", myModel);
            } catch (Exception e) {
                LogError(e, USER_PAGE_ERROR);
                return SendToErrorPage(USER_PAGE_ERROR_POLITE);
            }
        }

        [RequiredRouteValueAttribute.RequireRouteValues(new[] { "id" })]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Show(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myViewingUser = GetUserInformaton();
            
            try {
                UserProfileModel myProfile = theService.Profile(id, myViewingUser);
                LoggedInWrapperModel<UserProfileModel> myModel = new LoggedInWrapperModel<UserProfileModel>(myProfile.User, SiteSection.Profile);
                myModel.Model = myProfile;

                if (myModel.Model.IsEmpty()) {
                    ViewData["Message"] = MessageHelper.NormalMessage(EMPTY_PROFILE);
                }

                return View("Show", myModel);
            } catch (Exception e) {
                LogError(e, USER_PAGE_ERROR);
                return SendToErrorPage(USER_PAGE_ERROR_POLITE);
            }
        }

        [RequiredRouteValueAttribute.RequireRouteValues(new string[] { })]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Show() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel myUser = GetUserInformatonModel();
            LoggedInWrapperModel<UserProfileModel> myModel = new LoggedInWrapperModel<UserProfileModel>(myUser.Details, SiteSection.MyProfile);

            try {
                if (HAVPermissionHelper.AllowedToPerformAction(myUser, HAVPermission.Authority_Feed)) {
                    myModel.Model = theService.AuthorityProfile(myUser.Details);
                } else {
                    myModel.Model = theService.MyProfile(myUser.Details);
                }

                if (myModel.Model.IsEmpty()) {
                    ViewData["Message"] = MessageHelper.NormalMessage(EMPTY_FEED);
                }
            } catch (Exception e) {
                LogError(e, USER_PAGE_ERROR);
                ViewData["Message"] = USER_PAGE_ERROR_POLITE;
            }

                return View("Show", myModel);
        }

        [RequiredRouteValueAttribute.RequireRouteValues(new[] { "id" })]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult IssueActivity(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            
            try {
                UserProfileModel myProfile = theService.UserIssueActivity(id, myUser);
                LoggedInWrapperModel<UserProfileModel> myModel = new LoggedInWrapperModel<UserProfileModel>(myProfile.User, SiteSection.IssueActivity);
                myModel.Model = myProfile;

                if (myModel.Model.IsEmpty()) {
                    ViewData["Message"] = MessageHelper.NormalMessage(EMPTY_ISSUE_ACTIVITY);
                }

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
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            LoggedInWrapperModel<UserProfileModel> myModel = new LoggedInWrapperModel<UserProfileModel>(myUser, SiteSection.MyIssueActivity);

            try {
                myModel.Model = theService.UserIssueActivity(myUser.Id, myUser);

                if (myModel.Model.IsEmpty()) {
                    ViewData["Message"] = MessageHelper.NormalMessage(MY_EMPTY_ISSUE_ACTIVITY);
                }
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
