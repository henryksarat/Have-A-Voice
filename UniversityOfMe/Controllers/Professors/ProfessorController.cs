using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Exceptions;
using Social.Generic.Models;
using Social.Users.Services;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.Professors;
using UniversityOfMe.UserInformation;
using Social.Photo.Exceptions;
using Social.BaseWebsite.Models;
using UniversityOfMe.Models.View;

namespace UniversityOfMe.Controllers.Professors {

    public class ProfessorController : UOFMeBaseController {
        private const string PROFESSOR_CREATED = "Professor created successfully! You can now proceed to review them.";
        private const string PROFESSOR_PICTURE_SUGGESTION_SUCCESS = "Thanks for suggesting a picture for the professor! We will review it and if we think it should replace the current one, we will do so.";
        private const string NO_PROFESSOR_REVIEWS = "There are no reviews for this professor. Be the first to review this professor!";
        private const string PROFESSOR_DOESNT_EXIST = "I'm sorry but that professor doesn't exist in the database. Please create the professor and then be the first to review them!";

        private const string PROFESSOR_SUGGESTED_ERROR = "An error occurred while trying to upload the suggested picture. Please try again.";
        private const string PROFESSOR_LIST_ERROR = "Unable to load the list of professors. Please try again.";
        private const string CREATE_PROFESSOR_ERROR = "Unable to load the create professor page. Please try again.";

        IValidationDictionary theValidationDictionary;
        IProfessorService theProfessorService;

        public ProfessorController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())));
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theProfessorService = new ProfessorService(theValidationDictionary);
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

                ILoggedInListModel<Professor> myLoggedInModel = new LoggedInListModel<Professor>(myUser);
                myLoggedInModel.Set(theProfessorService.GetProfessorsForUniversity(universityId));

                return View("List", myLoggedInModel);
            } catch (Exception myException) {
                LogError(myException, PROFESSOR_LIST_ERROR);
                return SendToErrorPage(PROFESSOR_LIST_ERROR);
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

                LoggedInWrapperModel<Professor> myLoggedIn = new LoggedInWrapperModel<Professor>(myUser);

                return View("Create", myLoggedIn);
            } catch (Exception myException) {
                LogError(myException, CREATE_PROFESSOR_ERROR);
                return SendToErrorPage(CREATE_PROFESSOR_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(string universityId, string firstName, string lastName, HttpPostedFileBase professorImage) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, universityId)) {
                return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
            }
            bool myResult = theProfessorService.CreateProfessor(GetUserInformatonModel(), universityId, firstName, lastName, professorImage);

            if (myResult) {
                return SendToResultPage(PROFESSOR_CREATED);
            }

            return RedirectToAction("Create");
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Details(string universityId, string id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                User myUser = GetUserInformatonModel().Details;

                if (!UniversityHelper.IsFromUniversity(myUser, universityId)) {
                    return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
                }
                bool myExists = theProfessorService.IsProfessorExists(universityId, id);

                if (!myExists) {
                    TempData["Message"] = PROFESSOR_DOESNT_EXIST;
                    return RedirectToAction("Create", "Professor");
                }

                Professor myProfessor = theProfessorService.GetProfessor(GetUserInformatonModel(), universityId, id);

                if (myProfessor.ProfessorReviews.Count() == 0) {
                    ViewData["Message"] = NO_PROFESSOR_REVIEWS;
                }

                LoggedInWrapperModel<Professor> myLoggedIn = new LoggedInWrapperModel<Professor>(myUser);
                myLoggedIn.Set(myProfessor);

                return View("Details", myLoggedIn);
            } catch(CustomException myException) {
                LogError(myException, myException.Message + ", name=" + id);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
            }

            return SendToErrorPage(ErrorKeys.ERROR_MESSAGE);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult SuggestProfessorPicture(string universityId, int professorId, HttpPostedFileBase professorImage, string firstName, string lastName) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();

            if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, universityId)) {
                return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
            }

            try {
                bool myResult = theProfessorService.CreateProfessorImageSuggestion(myUserInformation, professorId, professorImage);

                if (myResult) {
                    TempData["Message"] = PROFESSOR_PICTURE_SUGGESTION_SUCCESS;
                }
            } catch(PhotoException myException) {
                LogError(myException, PROFESSOR_SUGGESTED_ERROR);
                TempData["Message"] = PROFESSOR_SUGGESTED_ERROR;
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] = ErrorKeys.ERROR_MESSAGE;
            }

            return RedirectToAction("Details", new { id = URLHelper.ToUrlFriendly(firstName + " " + lastName) });
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
