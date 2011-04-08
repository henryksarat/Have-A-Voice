using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers;
using HaveAVoice.Services.Classes;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Validation;
using UniversityOfMe.Controllers.Helpers;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services;
using UniversityOfMe.Services.Classes;
using Generic.ActionFilters.ActionFilters;

namespace UniversityOfMe.Controllers.Classes {
    public class ClassController : BaseController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline> {
        private const string GET_CLASS_ERROR = "Error getting the class list for the university. Please try again.";
        private const string NO_CLASSES = "There are no classes for the university created.";
        private const string CLASS_DOESNT_EXIST = "That class doesn't seem to exist. Go ahead and create it so people can post to it.";

        IClassService theClassService;
        IUniversityService theUniversityService;
        IValidationDictionary theValidationDictionary;

        public ClassController()
            : base(new BaseService<User>(new EntityBaseRepository()),
                   UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())),
                   InstanceHelper.CreateAuthencationService(),
                   new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())) {
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theClassService = new ClassService(theValidationDictionary);
            theUniversityService = new UniversityService();
        }

        public ActionResult List(string universityId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, universityId)) {
                return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
            }

            IEnumerable<Class> myClasses = new List<Class>();

            try {
                myClasses = theClassService.GetClassesForUniversity(universityId);

                if (myClasses.Count<Class>() == 0) {
                    ViewData["Message"] = NO_CLASSES;
                }
            } catch(Exception myException) {
                LogError(myException, GET_CLASS_ERROR);
                ViewData["Message"] = GET_CLASS_ERROR;
            }

            return View("List", myClasses);
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Create(string universityId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, universityId)) {
                return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
            }

            IDictionary<string, string> myAcademicTerms = DictionaryHelper.DictionaryWithSelect();

            try {
                myAcademicTerms = theUniversityService.CreateAcademicTermsDictionaryEntry();
            } catch (Exception myExpcetion) {
                LogError(myExpcetion, UOMErrorKeys.ACADEMIC_TERMS_GET_ERROR);
                ViewData["Message"] = UOMErrorKeys.ACADEMIC_TERMS_GET_ERROR;
            }

            return View("Create", new CreateClassModel() {
                AcademicTerms = new SelectList(myAcademicTerms, "Value", "Key"),
                Years = new SelectList(UOMConstants.ACADEMIC_YEARS, DateTime.UtcNow.Year)
            });
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

                return View("Details", myClass);
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
                
                return View("Details", myClass);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                return SendToResultPage(ErrorKeys.ERROR_MESSAGE);
            }
        }

        protected override AbstractUserModel<User> GetSocialUserInformation() {
            return SocialUserModel.Create(GetUserInformaton());
        }

        protected override AbstractUserModel<User> CreateSocialUserModel(User aUser) {
            return SocialUserModel.Create(aUser);
        }

        protected override IProfilePictureStrategy<User> ProfilePictureStrategy() {
            return new ProfilePictureStrategy();
        }

        protected override string UserEmail() {
            return GetUserInformaton().Email;
        }

        protected override string UserPassword() {
            return GetUserInformaton().Password;
        }

        protected override int UserId() {
            return GetUserInformaton().Id;
        }

        protected override string ErrorMessage(string aMessage) {
            return aMessage;
        }

        protected override string NormalMessage(string aMessage) {
            return aMessage;
        }

        protected override string SuccessMessage(string aMessage) {
            return aMessage;
        }
    }
}
