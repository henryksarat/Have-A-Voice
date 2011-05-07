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
    public class ClassReviewController : UOFMeBaseController {
        private const string REVIEW_POSTED = "The review was posted.";
        private const string REVIEW_ERROR = "Unable to post the review, please try again.";

        IClassService theClassService;
        IValidationDictionary theValidationDictionary;

        public ClassReviewController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())));
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theClassService = new ClassService(theValidationDictionary);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(int classId, string rating, string review, bool anonymous) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                bool myResult = theClassService.CreateClassReview(GetUserInformatonModel(), classId, rating, review, anonymous);

                if (myResult) {
                    TempData["Message"] = REVIEW_POSTED;
                }
            } catch (Exception myException) {
                LogError(myException, REVIEW_ERROR);
                TempData["Message"] = REVIEW_ERROR;
                theValidationDictionary.ForceModleStateExport();
            }

            return RedirectToAction("DetailsWithClassId", "Class", new { classId = classId });
        }
    }
}
