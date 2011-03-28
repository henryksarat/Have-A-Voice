using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using HaveAVoice.Models.View;
using HaveAVoice.Services;
using HaveAVoice.Repositories;
using Social.Generic.Services;
using HaveAVoice.Models;

namespace HaveAVoice.Controllers.Shared
{
    public class SharedController : HAVBaseController
    {
        public SharedController()
            : base(new BaseService<User>(new HAVBaseRepository())) { }

        public ActionResult PageNotFound() {
            ErrorModel error = new ErrorModel();
            error.ErrorMessage = "That page was not found.";
            return View("Error", error);
        }

        public ActionResult Error() {
            ErrorModel error = (ErrorModel)Session["ErrorMessage"];
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
