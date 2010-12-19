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

namespace HaveAVoice.Controllers.Core {
    public class AuthenticationController : HAVBaseController {
        private static string AUTHENTICAITON_ERROR = "Error authenticating. Please try again.";
        private static string INCORRECT_LOGIN = "Incorrect username and password combination.";
        private static string INVALID_ACTIVATION_CODE = "Invalid activation code.";
        private static string SPECIFIC_ROLE_ERROR = "Unable to activate the account because of a role issue.";
        private static string OUR_ERROR = "Couldn't activate the account because of something on our end. Please try again later.";
        private static string ACTIVATION_ERROR = "Error while activating your account. Please try again.";

        private IHAVUserService theService;
        private IValidationDictionary theValidationDictionary;

        public AuthenticationController() :
            base(new HAVBaseService(new HAVBaseRepository())) {
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theService = new HAVUserService(theValidationDictionary);
        }

        public AuthenticationController(IHAVUserService service, IHAVBaseService baseService)
            : base(baseService) {
            theService = service;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Login() {
            return View("Login");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(string email, string password, bool rememberMe) {
            UserInformationModel userModel = null;
            try {
                userModel = theService.AuthenticateUser(email, password);
            } catch (Exception e) {
                LogError(e, AUTHENTICAITON_ERROR);
                ViewData["Message"] = AUTHENTICAITON_ERROR;
                return View("Login");
            }

            if (userModel != null) {
                theService.AddToWhoIsOnline(userModel.Details, HttpContext.Request.UserHostAddress);

                CreateUserInformationSession(userModel);
                if (rememberMe) {
                    theService.CreateRememberMeCredentials(userModel.Details);
                }
            } else {
                ViewData["Message"] = INCORRECT_LOGIN;
                return View("Login");
            }

            return RedirectToPostLogin();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ActivateAccount(string id) {
            string myError;
            try {
                UserInformationModel userModel = theService.ActivateNewUser(id);
                CreateUserInformationSession(userModel);
                return RedirectToPostLogin();
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
            theService.RemoveFromWhoIsOnline(GetUserInformaton(), HttpContext.Request.UserHostAddress);
            Session.Clear();
            return RedirectToAction("Login");
        }

        private ActionResult RedirectToPostLogin() {
            return RedirectToAction("LoggedIn", "Home");
        }

        private void CreateUserInformationSession(UserInformationModel aUserModel) {
            Session["UserInformation"] = aUserModel;
        }

        protected override ActionResult SendToResultPage(string aTitle, string aDetails) {
            throw new NotImplementedException();
        }
    }
}
