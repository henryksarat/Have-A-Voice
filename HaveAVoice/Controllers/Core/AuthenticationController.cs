using System;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Exceptions;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Models.SocialWrappers;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.Helpers;
using HaveAVoice.Services.UserFeatures;
using Social.Admin.Exceptions;
using Social.Authentication.Services;
using Social.Generic.Constants;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Users.Services;
using Social.Validation;

namespace HaveAVoice.Controllers.Core {
    public class AuthenticationController : HAVBaseController {
        private const string ACCOUNT_ACTIVATED_BODY = "You may now login and access have a voice!";

        private IAuthenticationService<User, Role, Permission, UserRole, PrivacySetting, RolePermission> theAuthService;
        private IWhoIsOnlineService<User, WhoIsOnline> theWhoIsOnlineService;
        private IValidationDictionary theValidationDictionary;

        public AuthenticationController() :
            base(new BaseService<User>(new HAVBaseRepository())) {
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theAuthService = new HAVAuthenticationService();
            theWhoIsOnlineService = new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository());
        }

        public AuthenticationController(IBaseService<User> baseService, IHAVAuthenticationService anAuthService, IWhoIsOnlineService<User, WhoIsOnline> aWhoIsOnlineService)
            : base(baseService) {
            theAuthService = anAuthService;
            theWhoIsOnlineService = aWhoIsOnlineService;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Login() {
            if (IsLoggedIn()) {
                return RedirectToProfile();
            }
            return View("Login");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(string email, string password, bool rememberMe) {
            if (IsLoggedIn()) {
                return RedirectToProfile();
            }

            UserInformationModel<User> userModel = null;
            try {
                userModel = theAuthService.AuthenticateUser(email, password, new ProfilePictureStrategy());
            } catch (Exception e) {
                LogError(e, AuthenticationKeys.AUTHENTICAITON_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(AuthenticationKeys.AUTHENTICAITON_ERROR);
                return View("Login");
            }

            if (userModel != null) {
                theWhoIsOnlineService.AddToWhoIsOnline(userModel.Details, HttpContext.Request.UserHostAddress);

                CreateUserInformationSession(userModel);
                if (rememberMe) {
                    theAuthService.CreateRememberMeCredentials(SocialUserModel.Create(userModel.Details));
                }
            } else {
                ViewData["Message"] = MessageHelper.NormalMessage(AuthenticationKeys.INCORRECT_LOGIN);
                return View("Login");
            }

            return RedirectToProfile();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ActivateAccount(string id) {
            if (IsLoggedIn()) {
                return RedirectToProfile();
            }
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
            TempData["Message"] = MessageHelper.SuccessMessage(AuthenticationKeys.LOGGED_OUT);
            return RedirectToAction("Login");
        }

        private void CreateUserInformationSession(UserInformationModel<User> aUserModel) {
            Session["UserInformation"] = aUserModel;
        }
    }
}
