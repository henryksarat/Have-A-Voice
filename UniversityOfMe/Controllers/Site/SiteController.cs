using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Photo.Exceptions;
using Social.Users.Services;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.TextBooks;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Clubs {
    public class SiteController : UOFMeBaseController {
        public SiteController() { }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Main(string universityId) {
            if (IsLoggedIn()) {
                return RedirectToProfile();
            }
            return View("Main", new CreateUserModel() {
                Genders = new SelectList(Constants.GENDERS, Constants.SELECT)
            });
        }
    }
}
