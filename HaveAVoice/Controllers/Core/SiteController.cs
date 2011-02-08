using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaveAVoice.Controllers.Core
{
    public class SiteController : Controller {
        public ActionResult Privacy() {
            return View("Privacy");
        }

        public ActionResult AboutUs() {
            return View("AboutUs");
        }

        public ActionResult Terms() {
            return View("Terms");
        }
    }
}
