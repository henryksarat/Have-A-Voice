using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Models.View;
using HaveAVoice.Services;
using HaveAVoice.Repositories;
using HaveAVoice.Helpers;
using Social.Generic.Helpers;
using Social.Admin.Helpers;
using HaveAVoice.Models;
using Social.Generic.Services;

namespace HaveAVoice.Controllers.Admin {
    public class AdminController : AdminBaseController {
        private static string PAGE_NOT_FOUND = "You do not have access.";

        public AdminController() : 
            base(new BaseService<User>(new HAVBaseRepository())) {
        }

        public AdminController(IBaseService<User> aBaseService)
            : base(aBaseService) {
        }

        public ActionResult Index() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            if (!PermissionHelper<User>.AllowedToPerformAction(GetUserInformatonModel(), SocialPermission.View_Admin)) {
                return SendToErrorPage(PAGE_NOT_FOUND);
            }
            return View("Index");
        }
    }
}
