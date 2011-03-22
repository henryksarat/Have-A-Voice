﻿using System;
using System.Web.Mvc;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Services;
using System.Web.Routing;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Models.SocialWrappers;
using Social.User.Models;
using Social.Generic.Models;

namespace HaveAVoice.Controllers  {
    public abstract class HAVBaseController : Controller {
        private const string AUTHENTICATION_DURING_COOKIE_ERROR = "An error occurred while trying to authentication when grabbing the login info from a cookie.";
        private const string AFTER_AUTHENTICATION_ERROR = "An error occurred after authentication after a cookie.";
        private const string READ_ME_ERROR = "An error occurred while reading the read me credentials.";

        public IUserInformation theUserInformation;

        private IHAVBaseService theErrorService;
        private IHAVAuthenticationService theAuthService;
        private IHAVWhoIsOnlineService theWhoIsOnlineService;

        public HAVBaseController(IHAVBaseService baseService) : 
            this(baseService, new HAVAuthenticationService(), new HAVWhoIsOnlineService()) { }

        public HAVBaseController(IHAVBaseService baseService, IHAVAuthenticationService anAuthService, IHAVWhoIsOnlineService aWhoIsOnlineService) {
            theErrorService = baseService;
            theAuthService = anAuthService;
            theWhoIsOnlineService = aWhoIsOnlineService;
        }

        protected override void Initialize(RequestContext rc) {
            base.Initialize(rc);
            HAVUserInformationFactory.SetInstance(UserInformation.Instance());
            theErrorService.ResetConnection();
        }

        protected User GetUserInformaton() {
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            return myUserInformation != null ? myUserInformation.Details : null;
        }

        protected AbstractUserModel<User> GetSocialUserInformation() {
            return SocialUserModel.Create(GetUserInformaton());
        }

        protected UserInformationModel<User> GetUserInformatonModel() {
            return HAVUserInformationFactory.GetUserInformation();
        }

        protected void RefreshUserInformation() {
            UserInformationModel<User> myUserInformationModel = GetUserInformatonModel();
            try {
                myUserInformationModel = theAuthService.RefreshUserInformationModel(GetUserInformatonModel());
            } catch (Exception myException) {
                LogError(myException, String.Format("Big problem! Was unable to refresg the user information model for userid={0}", myUserInformationModel.Details.Id));
            }
            Session["UserInformation"] = myUserInformationModel;
        }

        protected bool IsLoggedIn() {
            if (!HAVUserInformationFactory.IsLoggedIn()) {

                User myUser = null;

                try {
                    myUser = theAuthService.ReadRememberMeCredentials();
                } catch (Exception myException) {
                    LogError(myException, READ_ME_ERROR);
                }

                if (myUser != null) {
                    UserInformationModel<User> userModel = null;
                    try {
                        userModel = theAuthService.CreateUserInformationModel(myUser);
                    } catch (Exception e) {
                        LogError(e, AUTHENTICATION_DURING_COOKIE_ERROR);
                    }

                    if (userModel != null) {
                        try {
                            theWhoIsOnlineService.AddToWhoIsOnline(userModel.Details, HttpContext.Request.UserHostAddress);

                            CreateUserInformationSession(userModel);
                            theAuthService.CreateRememberMeCredentials(userModel.Details);
                        } catch (Exception e) {
                            LogError(e, AFTER_AUTHENTICATION_ERROR);
                        }
                    }
                }
            }

            return HAVUserInformationFactory.IsLoggedIn();
        }

        protected ActionResult SendToErrorPage(string error) {
            AddErrorToSession(error);
            return RedirectToAction("Error", "Shared");
        }

        protected ActionResult SendToResultPage(string title, string details) {
            AddMessageToSession(title, details);
            return RedirectToAction("Result", "Shared");
        }

        protected ActionResult SendToResultPage(string details) {
            return SendToResultPage(null, details);
        }

        protected void LogError(Exception anException, string aDetails) {
            theErrorService.LogError(anException, aDetails);
        }

        protected ActionResult RedirectToLogin() {
            TempData["Message"] = MessageHelper.NormalMessage("You must be logged in to do that.");
            return RedirectToAction("Login", "Authentication");
        }

        protected ActionResult RedirectToProfile() {
            return RedirectToAction("Show", "Profile");
        }

        protected ActionResult RedirectToProfile(int anId) {
            return RedirectToAction("Show", "Profile", new { id = anId });
        }

        protected ActionResult RedirectToHomePage() {
            return RedirectToAction("NotLoggedIn", "Home");
        }

        private void AddMessageToSession(string title, string details) {
            MessageModel messageModel = new MessageModel();
            messageModel.Title = title;
            messageModel.Details = details;
            Session["Message"] = messageModel;
        }

        private void AddErrorToSession(string error) {
            ErrorModel errorModel = new ErrorModel();
            errorModel.ErrorMessage = error;
            Session["ErrorMessage"] = errorModel;
        }

        private void CreateUserInformationSession(UserInformationModel<User> aUserModel) {
            Session["UserInformation"] = aUserModel;
        }
    }
}
