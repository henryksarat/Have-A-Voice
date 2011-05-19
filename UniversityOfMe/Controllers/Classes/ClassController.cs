using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Users.Services;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services;
using UniversityOfMe.Services.Classes;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Classes {
    public class ClassController : UOFMeBaseController {
        private const string GET_CLASS_ERROR = "Error getting the class list for the university. Please try again.";
        private const string NO_CLASSES = "There are no classes for the university created.";
        private const string CLASS_DOESNT_EXIST = "That class doesn't seem to exist. Go ahead and create it so people can post to it.";
        private const string VIEW_ALL_MEMBERS_ERROR = "Unable to retrieve the class enrollment list. Please try again.";

        private const string CREATE_CLASS_ERROR = "Unable to load the create class page. Please try again.";
        private const string CLASS_DETAILS_ERROR = "Unable to load load the class details. Please try again.";

        IClassService theClassService;
        IUniversityService theUniversityService;
        IValidationDictionary theValidationDictionary;

        public ClassController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())));
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theClassService = new ClassService(theValidationDictionary);
            theUniversityService = new UniversityService(theValidationDictionary);
        }

        public ActionResult List(string universityId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                User myUser = GetUserInformatonModel().Details;

                if (!UniversityHelper.IsFromUniversity(myUser, universityId)) {
                    return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
                }

                IEnumerable<Class> myClasses = new List<Class>();
            
                myClasses = theClassService.GetClassesForUniversity(universityId);

                if (myClasses.Count<Class>() == 0) {
                    ViewData["Message"] = NO_CLASSES;
                }

                LoggedInListModel<Class> myLoggedIn = new LoggedInListModel<Class>(myUser);
                myLoggedIn.Set(myClasses);

                return View("List", myLoggedIn);
            } catch(Exception myException) {
                LogError(myException, GET_CLASS_ERROR);
                return SendToErrorPage(GET_CLASS_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Create(string universityId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                User myUser = GetUserInformatonModel().Details;

                if (!UniversityHelper.IsFromUniversity(myUser, universityId)) {
                    return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
                }

                IDictionary<string, string> myAcademicTerms = DictionaryHelper.DictionaryWithSelect();

            
                myAcademicTerms = theUniversityService.CreateAcademicTermsDictionaryEntry();

                CreateClassModel myClassModel = new CreateClassModel() {
                    AcademicTerms = new SelectList(myAcademicTerms, "Value", "Key"),
                    Years = new SelectList(UOMConstants.ACADEMIC_YEARS, DateTime.UtcNow.Year)
                };

                LoggedInWrapperModel<CreateClassModel> myLoggedIn = new LoggedInWrapperModel<CreateClassModel>(myUser);
                myLoggedIn.Set(myClassModel);

                return View("Create", myLoggedIn);
            } catch (Exception myExpcetion) {
                LogError(myExpcetion, CREATE_CLASS_ERROR);
                return SendToErrorPage(CREATE_CLASS_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(CreateClassModel aClass) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                Class myResult = theClassService.CreateClass(GetUserInformatonModel(), aClass);

                if (myResult != null) {
                    string[] myUrlPieces = new string[]{ myResult.ClassCode, myResult.AcademicTermId, myResult.Year.ToString() };
                    return RedirectToAction("Details", new { id = URLHelper.ToUrlFriendly(string.Join(" ", myUrlPieces)) });
                }
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] = ErrorKeys.ERROR_MESSAGE;
                theValidationDictionary.ForceModleStateExport();
            }

            return RedirectToAction("Create");
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult DetailsWithClassId(string universityId, int classId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, universityId)) {
                return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
            }

            try {
                Class myClass = theClassService.GetClass(GetUserInformatonModel(), classId);
                LoggedInWrapperModel<Class> myLoggedInModel = new LoggedInWrapperModel<Class>(GetUserInformatonModel().Details);
                myLoggedInModel.Set(myClass);

                return View("Details", myLoggedInModel);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                return SendToResultPage(ErrorKeys.ERROR_MESSAGE);
            }
        }
        
        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Details(string universityId, string id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                bool myExists = theClassService.IsClassExists(id);

                if (!myExists) {
                    TempData["Message"] = CLASS_DOESNT_EXIST;
                    return RedirectToAction("Create", "Class");
                }

                Class myClass = theClassService.GetClass(GetUserInformatonModel(), id);
                LoggedInWrapperModel<Class> myLoggedInModel = new LoggedInWrapperModel<Class>(GetUserInformatonModel().Details);
                myLoggedInModel.Set(myClass);

                return View("Details", myLoggedInModel);
            } catch (Exception myException) {
                LogError(myException, CLASS_DETAILS_ERROR);
                return SendToResultPage(CLASS_DETAILS_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult ViewAllMembers(string universityId, int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            User myUser = GetUserInformatonModel().Details;

            if (!UniversityHelper.IsFromUniversity(myUser, universityId)) {
                return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
            }

            try {
                IEnumerable<ClassEnrollment> myClassEnrollments = theClassService.GetEnrolledInClass(id);
                LoggedInListModel<ClassEnrollment> myLogedInModel = new LoggedInListModel<ClassEnrollment>(myUser);
                myLogedInModel.Set(myClassEnrollments);

                return View("ViewAllMembers", myLogedInModel);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] = VIEW_ALL_MEMBERS_ERROR;
                return RedirectToAction("Details", new { id = id });
            }
        }
    }
}
