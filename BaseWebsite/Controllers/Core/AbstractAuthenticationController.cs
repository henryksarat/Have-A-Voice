using System;
using System.Web;
using System.Web.Mvc;
using Social.Admin.Exceptions;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.Authentication.Services;
using Social.Generic.Constants;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.User.Exceptions;
using Social.Users.Services;
using Social.Validation;

namespace BaseWebsite.Controllers.Core {
    //T = User
    //U = Role
    //V = Permission
    //W = UserRole
    //X = PrivacySetting
    //Y = RolePermission
    //Z = WhoIsOnline
    public abstract class AbstractAuthenticationController<T, U, V, W, X, Y, Z> : BaseController<T, U, V, W, X, Y, Z> {
        private IAuthenticationService<T, U, V, W, X, Y> theAuthService;
        private IWhoIsOnlineService<T, Z> theWhoIsOnlineService;
        private IValidationDictionary theValidationDictionary;

        public AbstractAuthenticationController(IBaseService<T> aBaseService, 
                                                IUserInformation<T, Z> aUserInformation, 
                                                IAuthenticationService<T, U, V, W, X, Y> anAuthService, 
                                                IWhoIsOnlineService<T, Z> aWhoIsOnlineService)
            : base(aBaseService, aUserInformation, anAuthService, aWhoIsOnlineService) {
            theAuthService = anAuthService;
            theWhoIsOnlineService = aWhoIsOnlineService;
        }

        protected ActionResult Login() {
            if (IsLoggedIn()) {
                return RedirectToProfile();
            }
            return View("Login");
        }

        protected ActionResult Login(string email, string password, bool rememberMe) {
            if (IsLoggedIn()) {
                return RedirectToProfile();
            }

            UserInformationModel<T> userModel = null;
            try {
                userModel = theAuthService.AuthenticateUser(email, password, ProfilePictureStrategy());
            } catch (Exception e) {
                LogError(e, AuthenticationKeys.AUTHENTICAITON_ERROR);
                ViewData["Message"] = ErrorMessage(AuthenticationKeys.AUTHENTICAITON_ERROR);
                return View("Login");
            }

            if (userModel != null) {
                theWhoIsOnlineService.AddToWhoIsOnline(userModel.Details, HttpContext.Request.UserHostAddress);

                CreateUserInformationSession(userModel);
                if (rememberMe) {
                    theAuthService.CreateRememberMeCredentials(GetSocialUserInformation(userModel.Details));
                }
            } else {
                ViewData["Message"] = NormalMessage(AuthenticationKeys.INCORRECT_LOGIN);
                return View("Login");
            }

            return RedirectToProfile();
        }

        protected ActionResult ActivateAccount(string id, string anAccountActivationBody) {
            if (IsLoggedIn()) {
                return RedirectToProfile();
            }
            string myError;
            try {
                theAuthService.ActivateNewUser(id);
                return SendToResultPage(AuthenticationKeys.ACCOUNT_ACTIVATED_TITLE, anAccountActivationBody);
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

        protected ActionResult LogOut() {
            if (!IsLoggedIn()) {
                return RedirectToHomePage();
            }
            theWhoIsOnlineService.RemoveFromWhoIsOnline(GetUserInformaton(), HttpContext.Request.UserHostAddress);
            Session.Clear();
            CookieHelper.ClearCookies();
            TempData["Message"] = SuccessMessage(AuthenticationKeys.LOGGED_OUT);
            return RedirectToAction("Login");
        }

        private void CreateUserInformationSession(UserInformationModel<T> aUserModel) {
            Session["UserInformation"] = aUserModel;
        }
    }
}
