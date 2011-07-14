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
using UniversityOfMe.Helpers;
using Social.Generic.Models;
using System.Collections.Generic;
using UniversityOfMe.Models.View;

namespace UniversityOfMe.Controllers.Classes {
    public class ClassEnrollmentController : UOFMeBaseController {
        private const string ENROLLED_SUCCESS = "You have added yourself to the class enrollment list.";
        private const string ENROLLED_REMOVE_SUCCESS = "You have removed yourself from the class enrollment list.";
        private const string CLASS_ENROLLED_GET_ERROR = "Unable to get the class list. Please try again.";

        IClassService theClassService;
        IValidationDictionary theValidationDictionary;

        public ClassEnrollmentController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theClassService = new ClassService(theValidationDictionary);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create(int classId, ClassViewType classViewType) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                theClassService.AddToClassEnrollment(GetUserInformatonModel(), classId);

                TempData["Message"] += MessageHelper.SuccessMessage(ENROLLED_SUCCESS);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] += MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            }

            return RedirectToAction("DetailsWithClassId", "Class", new { classId = classId, classViewType = classViewType });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int classId, ClassViewType classViewType) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                theClassService.RemoveFromClassEnrollment(GetUserInformatonModel(), classId);

                TempData["Message"] += MessageHelper.SuccessMessage(ENROLLED_REMOVE_SUCCESS);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] += MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            }

            return RedirectToAction("DetailsWithClassId", "Class", new { classId = classId, classViewType = classViewType });
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult List(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                UserInformationModel<User> myUserInformation = GetUserInformatonModel();
                IEnumerable<ClassEnrollment> myClubMembers = theClassService.GetEnrolledInClass(id);
                LoggedInListModel<ClassEnrollment> myLoggedInModel = new LoggedInListModel<ClassEnrollment>(myUserInformation.Details);
                myLoggedInModel.Set(myClubMembers);
                return View("List", myLoggedInModel);
            } catch (Exception myException) {
                LogError(myException, CLASS_ENROLLED_GET_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(CLASS_ENROLLED_GET_ERROR);
                return RedirectToAction("Details", "Class", new { id = id });
            }
        }
    }
}
