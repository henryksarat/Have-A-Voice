using System;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Users.Services;
using Social.Validation;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.Classes;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Classes {
    public class ClassEnrollmentController : UOFMeBaseController {
        private const string ENROLLED_SUCCESS = "You have added yourself to the class enrollment list.";
        private const string ENROLLED_REMOVE_SUCCESS = "You have removed yourself from the class enrollment list.";

        IClassService theClassService;
        IValidationDictionary theValidationDictionary;

        public ClassEnrollmentController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())));
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theClassService = new ClassService(theValidationDictionary);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(int classId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                theClassService.AddToClassEnrollment(GetUserInformatonModel(), classId);

                TempData["Message"] = ENROLLED_SUCCESS;
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] = ErrorKeys.ERROR_MESSAGE;
            }

            return RedirectToAction("DetailsWithClassId", "Class", new { classId = classId });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int classId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                theClassService.RemoveFromClassEnrollment(GetUserInformatonModel(), classId);

                TempData["Message"] = ENROLLED_REMOVE_SUCCESS;
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] = ErrorKeys.ERROR_MESSAGE;
            }

            return RedirectToAction("DetailsWithClassId", "Class", new { classId = classId });
        }
    }
}
