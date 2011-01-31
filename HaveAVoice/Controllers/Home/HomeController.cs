using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Models.View;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Services;
using HaveAVoice.Repositories;
using HaveAVoice.Validation;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Services.Helpers;

namespace HaveAVoice.Controllers.Home {
    public class HomeController : HAVBaseController {
        private const string FILTERED_SAVED = "Filter saved an executed.";

        private const string PAGE_LOAD_ERROR = "Unable to load the page. Please try again.";
        private const string UNABLE_TO_ADD_FILTER = "Unable to add the filter, please try again.";

        private const string VIEW_DATA_MESSAGE = "Message";
        private const string NOT_LOGGED_IN = "NotLoggedIn";

        private static string INCORRECT_LOGIN = "Incorrect username and password combination.";
        private static string AUTHENTICAITON_ERROR = "Error authenticating. Please try again.";

        private IHAVAuthenticationService theAuthService;
        private IHAVWhoIsOnlineService theWhoIsOnlineService;
        private IHAVHomeService theService;

        public HomeController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
            theService = new HAVHomeService(new ModelStateWrapper(this.ModelState));
            theAuthService = new HAVAuthenticationService();
            theWhoIsOnlineService = new HAVWhoIsOnlineService();
        }

        public HomeController(IHAVBaseService aBaseService, IHAVAuthenticationService anAuthService, IHAVHomeService aService, IHAVWhoIsOnlineService aWhoIsOnlineService)
            : base(aBaseService) {
            theAuthService = anAuthService;
            theService = aService;
            theWhoIsOnlineService = aWhoIsOnlineService;
        }

        public ActionResult Index() {
            return View("Index");
        }

        public ActionResult NotLoggedIn() {
            if (IsLoggedIn()) {
                return RedirectToProfile();
            }

            User myUser = theAuthService.ReadRememberMeCredentials();
            if (myUser != null) {
                UserInformationModel userModel = null;
                try {
                    userModel = theAuthService.AuthenticateUserWithHashedPassword(myUser.Email, myUser.Password);
                } catch (Exception e) {
                    LogError(e, AUTHENTICAITON_ERROR);
                    TempData["Message"] = MessageHelper.ErrorMessage(AUTHENTICAITON_ERROR);
                    return View("Authentication", "Login");
                }

                if (userModel != null) {
                    theWhoIsOnlineService.AddToWhoIsOnline(userModel.Details, HttpContext.Request.UserHostAddress);

                    CreateUserInformationSession(userModel);
                    theAuthService.CreateRememberMeCredentials(userModel.Details);
                } else {
                    TempData["Message"] = MessageHelper.NormalMessage(INCORRECT_LOGIN);
                    return View("Authentication", "Login");
                }

                return RedirectToProfile();
                
            }

            NotLoggedInModel myModel = new NotLoggedInModel();

            try {
                myModel = theService.NotLoggedIn();
            } catch (Exception e) {
                LogError(e, PAGE_LOAD_ERROR);
                ViewData[VIEW_DATA_MESSAGE] = MessageHelper.ErrorMessage(PAGE_LOAD_ERROR);
            }

            return View(NOT_LOGGED_IN, myModel);
        }

        private void CreateUserInformationSession(UserInformationModel aUserModel) {
            Session["UserInformation"] = aUserModel;
        }
    }
}
