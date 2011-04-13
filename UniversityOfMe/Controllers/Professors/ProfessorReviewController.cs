using System;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Services.Issues;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
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

    public class ProfessorReviewController : UOFMeBaseController {
        private const string PROFESSOR_REVIEW = "The review for the professor has been posted!";
        private const string PROFESSOR_DOESNT_EXIST = "Couldn't find that professor. Please create that professor profile.";

        IProfessorReviewService theProfessorReviewService;
        IProfessorService theProfessorService;
        IUniversityService theUniversityService;

        public ProfessorReviewController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())));
            IValidationDictionary myModelState = new ModelStateWrapper(this.ModelState);
            theProfessorReviewService = new ProfessorReviewService(myModelState);
            theProfessorService = new ProfessorService(myModelState);
            theUniversityService = new UniversityService();
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Create(int id) {
            Professor myProfessor = theProfessorService.GetProfessor(id);

            if (myProfessor == null) {
                TempData["Message"] = PROFESSOR_DOESNT_EXIST;
                return RedirectToAction("Create", "Professor");
            }
            CreateProfessorReviewModel myViewModel = new CreateProfessorReviewModel() {
                ProfessorId = myProfessor.Id,
                ProfessorName = myProfessor.FirstName + " " + myProfessor.LastName,
                AcademicTerms = new SelectList(theUniversityService.CreateAcademicTermsDictionaryEntry(), "Value", "Key"),
                AcademicTermId = Constants.SELECT,
                Years = new SelectList(UOMConstants.ACADEMIC_YEARS, DateTime.UtcNow.Year)
            };

            return View("Create", myViewModel);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(CreateProfessorReviewModel professorReview) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            bool myResult = theProfessorReviewService.CreateProfessorReview(GetUserInformatonModel(), professorReview.ToModel());

            if (myResult) {
                return SendToResultPage(PROFESSOR_REVIEW);
            }

            return RedirectToAction("Create");
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
