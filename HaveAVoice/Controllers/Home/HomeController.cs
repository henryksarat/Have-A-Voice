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

namespace HaveAVoice.Controllers.Home {
    public class HomeController : HAVBaseController {
        private const string FILTERED_SAVED = "Filter saved an executed.";

        private const string PAGE_LOAD_ERROR = "Unable to load the page. Please try again.";
        private const string UNABLE_TO_ADD_FILTER = "Unable to add the filter, please try again.";

        private const string VIEW_DATA_MESSAGE = "Message";
        private const string NOT_LOGGED_IN = "NotLoggedIn";

        private IHAVHomeService theService;

        public HomeController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
            theService = new HAVHomeService(new ModelStateWrapper(this.ModelState));
        }

        public HomeController(IHAVBaseService aBaseService, IHAVHomeService aService)
            : base(aBaseService) {
            theService = aService;
        }

        public ActionResult Index() {
            return View("Index");
        }

        public ActionResult NotLoggedIn() {
            if (IsLoggedIn()) {
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
    }
}
