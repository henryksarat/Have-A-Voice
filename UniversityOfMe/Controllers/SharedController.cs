using System.Web.Mvc;
using Social.Generic.Models;
using Social.Generic.Services;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories;

namespace UniversityOfMe.Controllers.Shared {
    public class SharedController : BaseSocialController {
        public SharedController()
            : base(new BaseService<User>(new EntityBaseRepository())) { }

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
