using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Models.View;
using HaveAVoice.Services;
using HaveAVoice.Repositories;
using HaveAVoice.Helpers;

namespace HaveAVoice.Controllers.Admin {
    public class AdminController : AdminBaseController {
        private static string PAGE_NOT_FOUND = "You do not have access.";

        public AdminController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
        }

        public AdminController(IHAVBaseService aBaseService)
            : base(aBaseService) {
        }

        public ActionResult Index() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            if (!HAVPermissionHelper.AllowedToPerformAction(GetUserInformatonModel(), HAVPermission.View_Admin)) {
                return SendToErrorPage(PAGE_NOT_FOUND);
            }
            return View("Index");
        }

        protected override ActionResult SendToResultPage(string title, string details) {
            return SendToResultPage(SiteSectionsEnum.Admin, title, details);
        }
    }
}
