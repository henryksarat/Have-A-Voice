using System.Web.Mvc;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using Social.Generic.Models;
using Social.Generic.Services;

namespace HaveAVoice.Controllers.Shared
{
    public class SharedController : HAVBaseController
    {
        public SharedController() { }

        public ActionResult PageNotFound() {
            ErrorModel error = new ErrorModel();
            error.ErrorMessage = "That page was not found.";
            return View("Error", error);
        }

        public ActionResult Error() {
            StringModel error = (StringModel)Session["ErrorMessage"];
            Session.Remove("ErrorMessage");
            return View("Error", error);
        }

        public ActionResult Result() {
            MessageModel message = (MessageModel)Session["Message"];
            Session.Remove("Message");
            return View("Result", message);
        }
    }
}
