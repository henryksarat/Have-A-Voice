using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using HaveAVoice.Models;
using HaveAVoice.Models.Validation;
using HaveAVoice.Models.View;
using HaveAVoice.Services;
using HaveAVoice.Repositories;
using HaveAVoice.Helpers;
using HaveAVoice.Services.AdminFeatures;

namespace HaveAVoice.Controllers.Errors
{
    public class ErrorController : HAVBaseController
    {
        private IHAVErrorService theService;

        public ErrorController()
            : base(new HAVBaseService(new HAVBaseRepository())) {
            theService = new HAVErrorService(new ModelStateWrapper(this.ModelState));
        }

        public ErrorController(IHAVErrorService service, IHAVBaseService baseService) 
            : base(baseService) {
            theService = service;
        }

        public ActionResult Index() {
            return View(theService.GetAllErrors().ToList<ErrorLog>());
        }

        public override ActionResult SendToResultPage(string title, string details) {
            return SendToResultPage(SiteSectionsEnum.None, title, details);
        }
    }
}
