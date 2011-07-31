using System;
using System.Web.Mvc;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.UserFeatures;
using Social.Users.Services;
using Social.Validation;
using Social.Generic.Constants;

namespace HaveAVoice.Controllers.Home {
    public class HomeController : HAVBaseController {
        private const string FILTERED_SAVED = "Filter saved an executed.";

        private const string PAGE_LOAD_ERROR = "Unable to load the page. Please try again.";
        private const string UNABLE_TO_ADD_FILTER = "Unable to add the filter, please try again.";

        private const string VIEW_DATA_MESSAGE = "Message";
        private const string MAIN = "Main";

        private IHAVAuthenticationService theAuthService;
        private IWhoIsOnlineService<User, WhoIsOnline> theWhoIsOnlineService;
        private IHAVHomeService theService;

        public HomeController() {
            theService = new HAVHomeService(new ModelStateWrapper(this.ModelState));
            theAuthService = new HAVAuthenticationService();
            theWhoIsOnlineService = new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository());
        }

        public HomeController(IHAVAuthenticationService anAuthService, IHAVHomeService aService, IWhoIsOnlineService<User, WhoIsOnline> aWhoIsOnlineService) {
            theAuthService = anAuthService;
            theService = aService;
            theWhoIsOnlineService = aWhoIsOnlineService;
        }

        public ActionResult Index() {
            return View("Index");
        }

        public ActionResult Main() {
            if (IsLoggedIn()) {
                return RedirectToProfile();
            }
            return View("Main", new CreateUserModelBuilder() {
                States = new SelectList(UnitedStates.STATES, Constants.SELECT),
                Genders = new SelectList(Constants.GENDERS, Constants.SELECT)
            });
        }
    }
}
