using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using HaveAVoice.Models.View;
using HaveAVoice.Services;
using HaveAVoice.Models.Repositories;

namespace HaveAVoice.Controllers.Shared
{
    public class SharedController : HAVBaseController
    {
        public SharedController()
            : base(new HAVBaseService(new HAVBaseRepository())) { }

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



        public override ActionResult SendToResultPage(string title, string details) {
            throw new NotImplementedException();
        }
    }
}
