using System.Web.Mvc;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Models;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Services.Users;

namespace UniversityOfMe.Controllers.Clubs {
    public class SiteController : UOFMeBaseController {
        private IUofMeUserService theUserService;

        public SiteController() {
            theUserService = new UofMeUserService(null);
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Main(string universityId) {           
            if (IsLoggedIn()) {
                return RedirectToProfile();
            }
           
            return View("Main", new CreateUserModel() {
                RegisteredUserCount = theUserService.GetNumberOfRegisteredUsers(),
                Genders = new SelectList(Constants.GENDERS, Constants.SELECT)
            });
        }

        public ActionResult About() {
            return View("About");
        }

        public ActionResult Terms() {
            return View("Terms");
        }

        public ActionResult Privacy() {
            return View("Privacy");
        }
    }
}
