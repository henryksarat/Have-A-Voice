using System;
using System.Web.Mvc;
using System.Web.Routing;
using Social.Generic.Models;

namespace HaveAVoice.Controllers  {
    /*
    public abstract class BaseSocialController : Controller {
        protected ActionResult SendToErrorPage(string error) {
            AddErrorToSession(error);
            return RedirectToAction("Error", "Shared");
        }

        protected ActionResult SendToResultPage(string aDetails) {
            AddMessageToSession(aDetails);
            return RedirectToAction("Result", "Shared");
        }

        private void AddMessageToSession(string aDetails) {
            Session["Message"] = new StringModel(aDetails);
        }

        private void AddErrorToSession(string anError) {
            Session["ErrorMessage"] = new StringModel(anError);
        }
    }
     * */
}
