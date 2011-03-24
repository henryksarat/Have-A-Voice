using System;
using System.Web.Mvc;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services;
using HaveAVoice.Services.UserFeatures;
using Social.Users.Services;
using Social.Validation;

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

        public HomeController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
            theService = new HAVHomeService(new ModelStateWrapper(this.ModelState));
            theAuthService = new HAVAuthenticationService();
            theWhoIsOnlineService = new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository());
        }

        public HomeController(IHAVBaseService aBaseService, IHAVAuthenticationService anAuthService, IHAVHomeService aService, IWhoIsOnlineService<User, WhoIsOnline> aWhoIsOnlineService)
            : base(aBaseService) {
            theAuthService = anAuthService;
            theService = aService;
            theWhoIsOnlineService = aWhoIsOnlineService;
        }

        public ActionResult Index() {
            return View("Index");
        }

        [OutputCache(Duration = 10, VaryByParam = "none")]
        public ActionResult Main() {
            NotLoggedInModel myModel = new NotLoggedInModel();
            try {
                myModel = theService.NotLoggedIn();
            } catch (Exception e) {
                LogError(e, PAGE_LOAD_ERROR);
                ViewData[VIEW_DATA_MESSAGE] = MessageHelper.ErrorMessage(PAGE_LOAD_ERROR);
            }

            return View(MAIN, myModel);
        }
    }
}
