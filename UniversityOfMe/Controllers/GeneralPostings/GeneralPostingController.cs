using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Models;
using Social.Users.Services;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.GeneralPostings;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.GeneralPostings {
    public class GeneralPostingController : UOFMeBaseController {
        private const string NO_GENERAL_POSTING = "There are no general postings yet.";
        
        IGeneralPostingService theGeneralPostingService;
        IValidationDictionary theValidationDictionary;

        public GeneralPostingController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())));
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theGeneralPostingService = new GeneralPostingService(theValidationDictionary);
        }

        public ActionResult List(string universityId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, universityId)) {
                return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
            }

            IEnumerable<GeneralPosting> myGeneralPostings = new List<GeneralPosting>();

            try {
                myGeneralPostings = theGeneralPostingService.GetGeneralPostingsForUniversity(universityId);

                if (myGeneralPostings.Count<GeneralPosting>() == 0) {
                    ViewData["Message"] = NO_GENERAL_POSTING;
                }
            } catch(Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                ViewData["Message"] = ErrorKeys.ERROR_MESSAGE;
            }

            return View("List", myGeneralPostings);
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Create(string universityId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, universityId)) {
                return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
            }

            return View("Create");
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(string universityId, string title, string body) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                GeneralPosting myResult = theGeneralPostingService.CreateGeneralPosting(GetUserInformatonModel(), universityId, title, body);

                if (myResult != null) {
                    return RedirectToAction("Details", new { id = myResult.Id });
                }
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] = ErrorKeys.ERROR_MESSAGE;
                theValidationDictionary.ForceModleStateExport();
            }

            return RedirectToAction("Create");
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Details(string universityId, int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                GeneralPosting myGeneralPosting = theGeneralPostingService.GetGeneralPosting(id);
                
                return View("Details", myGeneralPosting);
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
