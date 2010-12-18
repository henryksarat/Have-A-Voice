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
                LogError(e, "Unable to authenticate the User.");
                ViewData["Message"] = "Error authenticating. Please try again.";
                return View("Login");
            }

            if (userModel != null) {
                theService.AddToWhoIsOnline(userModel.Details, HttpContext.Request.UserHostAddress);

                CreateUserInformationSession(userModel);
                if (rememberMe) {
                    theService.CreateRememberMeCredentials(userModel.Details);
                }
            } else {
                ViewData["Message"] = "Incorrect aUsername and aPassword combination.";
                return View("Login");
            }

            return RedirectToAction("LoggedIn", "Home");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ActivateAccount(string id) {
            try {
                UserInformationModel userModel = theService.ActivateNewUser(id);
                CreateUserInformationSession(userModel);
                return RedirectToAction("LoggedIn", "Home", null);
            } catch (NullUserException) {
                ViewData["Message"] = "That activation code doesn't exist for a user.";
            } catch (NullRoleException e) {
                LogError(e, "Unable to activate the account because of a role issue.");
                ViewData["Message"] = "Couldn't activate the account because of something on our end. Please try again later.";
            } catch (Exception e) {
                LogError(e, "Couldn't activate the account for some reason.");
                ViewData["Message"] = "Error while activating your account, please try again.";
            }
            return View("ActivateAccount");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LogOut() {
            theService.RemoveFromWhoIsOnline(GetUserInformaton(), HttpContext.Request.UserHostAddress);
            Session.Clear();
            return RedirectToAction("Login");
        }

        private void CreateUserInformationSession(UserInformationModel userModel) {
            Session["UserInformation"] = userModel;
        }

        public override ActionResult SendToResultPage(string aTitle, string aDetails) {
            throw new NotImplementedException();
        }
    }
}
