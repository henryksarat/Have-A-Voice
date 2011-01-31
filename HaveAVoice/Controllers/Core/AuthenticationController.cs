using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Validation;
using HaveAVoice.Services;
using HaveAVoice.Repositories;
using HaveAVoice.Models.View;
using HaveAVoice.Exceptions;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Services.Helpers;

namespace HaveAVoice.Controllers.Core {
    public class AuthenticationController : HAVBaseController {
        private const string ACCOUNT_ACTIVATED_TITLE = "Account activated!";
        private const string ACCOUNT_ACTIVATED_BODY = "You may now login and access have a voice!";

        private static string AUTHENTICAITON_ERROR = "Error authenticating. Please try again.";
        private static string INCORRECT_LOGIN = "Incorrect username and password combination.";
        private static string INVALID_ACTIVATION_CODE = "Invalid activation code.";
        private static string SPECIFIC_ROLE_ERROR = "Unable to activate the account because of a role issue.";
        private static string OUR_ERROR = "Couldn't activate the account because of something on our end. Please try again later.";
        private static string ACTIVATION_ERROR = "Error while activating your account. Please try again.";

        private IHAVAuthenticationService theAuthService;
        private IHAVWhoIsOnlineService theWhoIsOnlineService;
        private IValidationDictionary theValidationDictionary;

        public AuthenticationController() :
            base(new HAVBaseService(new HAVBaseRepository())) {
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theAuthService = new HAVAuthenticationService();
            theWhoIsOnlineService = new HAVWhoIsOnlineService();
        }

        public AuthenticationController(IHAVBaseService baseService, IHAVAuthenticationService anAuthService, IHAVWhoIsOnlineService aWhoIsOnlineService)
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

            UserInformationModel userModel = null;
            try {
                userModel = theAuthService.AuthenticateUser(email, password);
            } catch (Exception e) {
                LogError(e, AUTHENTICAITON_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(AUTHENTICAITON_ERROR);
                return View("Login");
            }

            if (userModel != null) {
                theWhoIsOnlineService.AddToWhoIsOnline(userModel.Details, HttpContext.Request.UserHostAddress);

                CreateUserInformationSession(userModel);
                if (rememberMe) {
                    theAuthService.CreateRememberMeCredentials(userModel.Details);
                }
            } else {
                ViewData["Message"] = MessageHelper.NormalMessage(INCORRECT_LOGIN);
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
                return SendToResultPage(ACCOUNT_ACTIVATED_TITLE, ACCOUNT_ACTIVATED_BODY);
            } catch (NullUserException) {
                myError = INVALID_ACTIVATION_CODE;
            } catch (NullRoleException e) {
                LogError(e, SPECIFIC_ROLE_ERROR);
                myError = OUR_ERROR;
            } catch (Exception e) {
                LogError(e, ACTIVATION_ERROR);
                myError = ACTIVATION_ERROR;
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
            return RedirectToAction("Login");
        }

        private void CreateUserInformationSession(UserInformationModel aUserModel) {
            Session["UserInformation"] = aUserModel;
        }
    }
}
