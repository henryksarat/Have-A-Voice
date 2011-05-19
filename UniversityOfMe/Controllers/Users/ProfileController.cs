﻿using System;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.Models;
using Social.User.Services;
using Social.Users.Services;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Repositories.UserRepos;
using UniversityOfMe.UserInformation;
using Social.Generic.ActionFilters;

namespace UniversityOfMe.Controllers.Profile {
    public class ProfileController : UOFMeBaseController {
        private const string USER_PAGE_ERROR = "Unable to view the user page. Please try again.";
        private const string INVALID_SHORT_URL = "No user is assigned with that web address. Verify the spelling and try again.";

        private const string PROFILE_VIEW = "Show";

        private IUserRetrievalService<User> theUserRetrievalService;
        
        public ProfileController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())));
            theUserRetrievalService = new UserRetrievalService<User>(new EntityUserRetrievalRepository());
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

                LoggedInWrapperModel<User> myModel = new LoggedInWrapperModel<User>(myViewingUser.Details);
                myModel.Set(myProfile);

                return View(PROFILE_VIEW, myModel);
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
                LoggedInWrapperModel<User> myModel = new LoggedInWrapperModel<User>(myViewingUser.Details);
                myModel.Set(myUser);

                return View(PROFILE_VIEW, myModel);
            } catch (Exception e) {
                LogError(e, USER_PAGE_ERROR);
                return SendToErrorPage(USER_PAGE_ERROR);
            }
        }
    }
}