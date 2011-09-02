using System.Web.Mvc;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Models;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;

namespace UniversityOfMe.Controllers.Clubs {
    public class SiteController : UOFMeBaseController {
        public SiteController() {
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Main(string universityId) {           
            if (IsLoggedIn()) {
                return RedirectToProfile();
            }

           
            return View("Main", new CreateUserModel() {
                Genders = new SelectList(Constants.GENDERS, Constants.SELECT)
            });
        }

        public ActionResult About() {
            if (IsLoggedIn()) {

            }

            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            
            return View("About");
        }
    }
}
