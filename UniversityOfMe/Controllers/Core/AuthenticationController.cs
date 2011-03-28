using System;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Services.UserFeatures;
using Social.Admin.Exceptions;
using Social.Authentication.Helpers;
using Social.Authentication.Services;
using Social.Generic.Constants;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.User.Exceptions;
using Social.User.Services;
using Social.Users.Services;
using Social.Validation;
using UniversityOfMe.Controllers;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Repositories;
using UniversityOfMe.Repositories.AdminRepos;
using UniversityOfMe.Repositories.AuthenticationRepos;
using UniversityOfMe.Repositories.UserRepos;

namespace HaveAVoice.Controllers.Core {
    public class AuthenticationController : BaseSocialController {
        public const string ACCOUNT_ACTIVATED_BODY = "You may now login and access University Of Me!";

        private IAuthenticationService<User, Role, Permission, UserRole, PrivacySetting, RolePermission> theAuthService;
        private IWhoIsOnlineService<User, WhoIsOnline> theWhoIsOnlineService;
        private IValidationDictionary theValidationDictionary;

        public AuthenticationController() :
            base(new BaseService<User>(new EntityBaseRepository())) {
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theAuthService = new AuthenticationService<User, Role, Permission, UserRole, PrivacySetting, RolePermission>(
                    new UserRetrievalService<User>(new EntityUserRetrievalRepository()),
                    new UserPrivacySettingsService<User, PrivacySetting>(new EntityUserPrivacySettingsRepository()),
                    new EntityAuthenticationRepository(),
                    new EntityUserRepository(),
                    new EntityRoleRepository());
            theWhoIsOnlineService = new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository());
        }

        public AuthenticationController(IBaseService<User> baseService,
                                        IAuthenticationService<User, Role, Permission, UserRole, PrivacySetting, RolePermission> anAuthService, 
                                        IWhoIsOnlineService<User, WhoIsOnline> aWhoIsOnlineService) : base(baseService) {
            theAuthService = anAuthService;
            theWhoIsOnlineService = aWhoIsOnlineService;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Login() {
            return View("Login");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(string email, string password, bool rememberMe) {
            UserInformationModel<User> userModel = null;
            try {
                userModel = theAuthService.AuthenticateUser(email, password, new ProfilePictureStrategy());
            } catch (Exception e) {
                LogError(e, AuthenticationKeys.AUTHENTICAITON_ERROR);
                ViewData["Message"] = AuthenticationKeys.AUTHENTICAITON_ERROR;
                return View("Login");
            }

            if (userModel != null) {
                theWhoIsOnlineService.AddToWhoIsOnline(userModel.Details, HttpContext.Request.UserHostAddress);

                CreateUserInformationSession(userModel);
                if (rememberMe) {
                    theAuthService.CreateRememberMeCredentials(SocialUserModel.Create(userModel.Details));
                }
            } else {
                ViewData["Message"] = AuthenticationKeys.INCORRECT_LOGIN;
                return View("Login");
            }

            return RedirectToAction("Login");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ActivateAccount(string id) {
            string myError;
            try {
                theAuthService.ActivateNewUser(id);
                return SendToResultPage(AuthenticationKeys.ACCOUNT_ACTIVATED_TITLE, ACCOUNT_ACTIVATED_BODY);
            } catch (NullUserException) {
                myError = AuthenticationKeys.INVALID_ACTIVATION_CODE;
            } catch (NullRoleException e) {
                LogError(e, AuthenticationKeys.SPECIFIC_ROLE_ERROR);
                myError = AuthenticationKeys.OUR_ERROR;
            } catch (Exception e) {
                LogError(e, AuthenticationKeys.ACTIVATION_ERROR);
                myError = AuthenticationKeys.ACTIVATION_ERROR;
            }
            return SendToErrorPage(myError);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LogOut() {
            if (!IsLoggedIn()) {
                return RedirectToHomePage();
            }
            theWhoIsOnlineService.RemoveFromWhoIsOnline(GetUserInformaton(), HttpContext.Request.UserHostAddress);
            Session.Clear();
            CookieHelper.ClearCookies();
            TempData["Message"] = AuthenticationKeys.LOGGED_OUT;
            return RedirectToAction("Login");
        }

        private void CreateUserInformationSession(UserInformationModel<User> aUserModel) {
            Session["UserInformation"] = aUserModel;
        }
    }
}
