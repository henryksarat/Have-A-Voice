using System.Web.Mvc;
using UniversityOfMe.Models.View;
using Social.Generic.Constants;

namespace UniversityOfMe.Controllers.Site {
    public class HomeController : UOFMeBaseController {
        public ActionResult Main() {
            return RedirectToAction("Main", "Site");
        }
    }
}
