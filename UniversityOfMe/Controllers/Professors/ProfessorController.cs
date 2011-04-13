using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Services.Issues;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Exceptions;
using Social.Generic.Models;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services;
using UniversityOfMe.Services.Professors;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Professors {

    public class ProfessorController : UOFMeBaseController {
        private const string PROFESSOR_CREATED = "Professor created successfully! You can now proceed to review them.";
        private const string NO_PROFESSOR_REVIEWS = "There are no reviews for this professor. Be the first to review this professor!";
        private const string PROFESSOR_DOESNT_EXIST = "I'm sorry but that professor doesn't exist in the database. Please create the professor and then be the first to review them!";

        IProfessorService theProfessorService;
        IProfessorReviewService theProfessorReviewService;
        IUniversityService theUniversityService;

        public ProfessorController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())));
            IValidationDictionary myModelState = new ModelStateWrapper(this.ModelState);
            theProfessorService = new ProfessorService(myModelState);
            theProfessorReviewService = new ProfessorReviewService(myModelState);
            theUniversityService = new UniversityService();
        }

        public ActionResult List(string universityId) {
            IEnumerable<Professor> myProfessors = theProfessorService.GetProfessorsForUniversity(universityId);
            return View("List", myProfessors);
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Create(string universityId) {
            CreateProfessorModel myViewModel = new CreateProfessorModel() {
                Universities = new SelectList(theUniversityService.CreateAllUniversitiesDictionaryEntry(), "Value", "Key")
            };
            return View("Create", myViewModel);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(CreateProfessorModel professor) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            bool myResult = theProfessorService.CreateProfessor(GetUserInformatonModel(), professor.ToModel());

            if (myResult) {
                return SendToResultPage(PROFESSOR_CREATED);
            }

            return RedirectToAction("Create");
        }

        public ActionResult Details(string universityId, string id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                bool myExists = theProfessorService.IsProfessorExists(universityId, id);

                if (!myExists) {
                    TempData["Message"] = PROFESSOR_DOESNT_EXIST;
                    return RedirectToAction("Create", "Professor");
                }

                IEnumerable<ProfessorReview> myProfessorReviews = theProfessorReviewService.GetProfessorReviews(GetUserInformatonModel(), universityId, id);

                if (myProfessorReviews.Count() == 0) {
                    ViewData["Message"] = NO_PROFESSOR_REVIEWS;
                }

                return View("Details", myProfessorReviews);
            } catch(CustomException myException) {
                LogError(myException, myException.Message + ", name=" + id);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
            }

            return SendToResultPage(ErrorKeys.ERROR_MESSAGE);
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
