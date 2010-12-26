using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Validation;
using HaveAVoice.Services;
using HaveAVoice.Repositories;
using HaveAVoice.Helpers;
using HaveAVoice.Services.UserFeatures;

namespace HaveAVoice.Controllers.Issues
{
    public class SearchController : HAVBaseController
    {
        private IHAVSearchService theService;

        public SearchController() : base(new HAVBaseService(new HAVBaseRepository())) {
            theService = new HAVSearchService(new ModelStateWrapper(this.ModelState));
        }

        public SearchController(IHAVSearchService aService, IHAVBaseService baseService)
            : base(baseService) {
            theService = aService;
        }

        public ActionResult getAjaxResult(string q) {
            return Content(theService.SearchResult(q));
        }

        public ActionResult Index() {
            return View();
        }
    }
}
