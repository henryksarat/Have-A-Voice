using System.Web.Mvc;
using Social.Generic.Models;

namespace UniversityOfMe.Controllers.Shared {
    public class SharedController : UOFMeBaseController {
        public ActionResult Error() {
            StringModel myError = (StringModel)Session["ErrorMessage"];
            Session.Remove("ErrorMessage");
            return View("Error", myError);
        }

        public ActionResult Result() {
            MessageModel myMessage = (MessageModel)Session["Message"];
            Session.Remove("Message");
            return View("Result", myMessage);
        }
    }
}
