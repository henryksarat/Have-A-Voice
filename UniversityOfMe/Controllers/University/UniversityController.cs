using System;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Users.Services;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Clubs {
 
    public class UniversityController : UOFMeBaseController {
        private const string UNIVERSITY_PROFILE_ERROR = "An error occurred while loading you university feed page. Please logout and login again shortly.";

        private IValidationDictionary theValidation;
        private IUniversityService theUniversityService;

        public UniversityController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())));
            theValidation = new ModelStateWrapper(this.ModelState);
            theUniversityService = new UniversityService(theValidation);
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Main(string universityId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, universityId)) {
                return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
            }

            try {
                UniversityView myUniveristyView = theUniversityService.GetUniversityProfile(GetUserInformatonModel(), universityId);
                return View("Main", myUniveristyView);
            } catch (Exception myException) {
                LogError(myException, UNIVERSITY_PROFILE_ERROR);
                return SendToErrorPage(UNIVERSITY_PROFILE_ERROR);
            }
        }
    }
}
