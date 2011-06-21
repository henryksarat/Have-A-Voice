using System.Web.Mvc;

namespace UniversityOfMe.Controllers.Site {
    public class HomeController : UOFMeBaseController {
        public ActionResult Main() {
            return RedirectToAction("Main", "Site");
        }
    }
}
