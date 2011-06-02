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
using System.Web.Security;

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

        protected IAuthenticationService<T, U, V, W, X, Y> GetAuthService() {
            return theAuthService;
        }

        protected ActionResult Login() {
            if (IsLoggedIn()) {
                
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
                    theAuthService.CreateRememberMeCredentials(CreateSocialUserModel(userModel.Details));
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
            UserInformationModel<T> myUserInformation = GetUserInformatonModel();
            if (myUserInformation != null) {
                theWhoIsOnlineService.RemoveFromWhoIsOnline(myUserInformation.Details, HttpContext.Request.UserHostAddress);
            }

            FormsAuthentication.SignOut();
            Session.Abandon();

            // clear authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            TempData["Message"] = SuccessMessage(AuthenticationKeys.LOGGED_OUT);

            FormsAuthentication.RedirectToLoginPage();

            return RedirectToAction("Login");
        }

        private void CreateUserInformationSession(UserInformationModel<T> aUserModel) {
            FormsAuthentication.SetAuthCookie(aUserModel.UserId.ToString(), false);
        }
    }
}
