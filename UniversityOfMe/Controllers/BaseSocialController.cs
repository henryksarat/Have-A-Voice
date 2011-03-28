using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication;
using Social.Authentication.Services;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.User.Services;
using Social.Users.Services;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Repositories.AdminRepos;
using UniversityOfMe.Repositories.AuthenticationRepos;
using UniversityOfMe.Repositories.UserRepos;

namespace UniversityOfMe.Controllers  {
    public abstract class BaseSocialController : Controller {
        private const string AUTHENTICATION_DURING_COOKIE_ERROR = "An error occurred while trying to authentication when grabbing the login info from a cookie.";
        private const string AFTER_AUTHENTICATION_ERROR = "An error occurred after authentication after a cookie.";
        private const string READ_ME_ERROR = "An error occurred while reading the read me credentials.";

        public IUserInformation<User, WhoIsOnline> theUserInformation;

        private IBaseService<User> theErrorService;
        private IAuthenticationService<User, Role, Permission, UserRole, PrivacySetting, RolePermission> theAuthService;
        private IWhoIsOnlineService<User, WhoIsOnline> theWhoIsOnlineService;

        public BaseSocialController(IBaseService<User> baseService) :
            this(baseService,
                new AuthenticationService<User, Role, Permission, UserRole, PrivacySetting, RolePermission>(
                    new UserRetrievalService<User>(new EntityUserRetrievalRepository()),
                    new UserPrivacySettingsService<User, PrivacySetting>(new EntityUserPrivacySettingsRepository()),
                    new EntityAuthenticationRepository(),
                    new EntityUserRepository(),
                    new EntityRoleRepository()),
                new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())) { }

        public BaseSocialController(IBaseService<User> baseService,
                                    IAuthenticationService<User, Role, Permission, UserRole, PrivacySetting, RolePermission> anAuthService, 
                                    IWhoIsOnlineService<User, WhoIsOnline> aWhoIsOnlineService) {
            theErrorService = baseService;
            theAuthService = anAuthService;
            theWhoIsOnlineService = aWhoIsOnlineService;
        }

        protected override void Initialize(RequestContext rc) {
            base.Initialize(rc);
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())));
        }

        protected User GetUserInformaton() {
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            return myUserInformation != null ? myUserInformation.Details : null;
        }

        protected AbstractUserModel<User> GetSocialUserInformation() {
            return SocialUserModel.Create(GetUserInformaton());
        }

        protected UserInformationModel<User> GetUserInformatonModel() {
            return UserInformationFactory.GetUserInformation();
        }

        protected void RefreshUserInformation() {
            UserInformationModel<User> myUserInformationModel = GetUserInformatonModel();
            try {
                myUserInformationModel =
                    theAuthService.RefreshUserInformationModel(myUserInformationModel.Details.Email, myUserInformationModel.Details.Password, new ProfilePictureStrategy());
            } catch (Exception myException) {
                LogError(myException, String.Format("Big problem! Was unable to refresg the user information model for userid={0}", myUserInformationModel.Details.Id));
            }
            Session["UserInformation"] = myUserInformationModel;
        }

        protected bool IsLoggedIn() {
            if (!UserInformationFactory.IsLoggedIn()) {

                User myUser = null;

                try {
                    myUser = theAuthService.ReadRememberMeCredentials();
                } catch (Exception myException) {
                    LogError(myException, READ_ME_ERROR);
                }

                if (myUser != null) {
                    UserInformationModel<User> userModel = null;
                    AbstractUserModel<User> mySocialUserModel = SocialUserModel.Create(myUser);
                    try {
                        userModel = theAuthService.CreateUserInformationModel(mySocialUserModel, new ProfilePictureStrategy());
                    } catch (Exception e) {
                        LogError(e, AUTHENTICATION_DURING_COOKIE_ERROR);
                    }

                    if (userModel != null) {
                        try {
                            theWhoIsOnlineService.AddToWhoIsOnline(userModel.Details, HttpContext.Request.UserHostAddress);

                            CreateUserInformationSession(userModel);
                            theAuthService.CreateRememberMeCredentials(SocialUserModel.Create(userModel.Details));
                        } catch (Exception e) {
                            LogError(e, AFTER_AUTHENTICATION_ERROR);
                        }
                    }
                }
            }

            return UserInformationFactory.IsLoggedIn();
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
            AbstractUserModel<User> mySocialUserInfo = GetSocialUserInformation();
            theErrorService.LogError(mySocialUserInfo, anException, aDetails);
        }

        protected ActionResult RedirectToLogin() {
            TempData["Message"] = "You must be logged in to do that.";
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

        private void AddErrorToSession(string anError) {
            Session["ErrorMessage"] = new StringModel(anError);
        }

        private void CreateUserInformationSession(UserInformationModel<User> aUserModel) {
            Session["UserInformation"] = aUserModel;
        }
    }
}
