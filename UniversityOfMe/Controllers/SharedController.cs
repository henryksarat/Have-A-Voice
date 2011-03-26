using System.Web.Mvc;
using Social.Generic.Models;

namespace UniversityOfMe.Controllers.Shared {
    public class SharedController : BaseSocialController {
        public ActionResult Error() {
            StringModel myError = (StringModel)Session["ErrorMessage"];
            Session.Remove("ErrorMessage");
            return View("Error", myError);
        }

        public ActionResult Result() {
            StringModel myMessage = (StringModel)Session["Message"];
            Session.Remove("Message");
            return View("Result", myMessage);
        }
    }
}
