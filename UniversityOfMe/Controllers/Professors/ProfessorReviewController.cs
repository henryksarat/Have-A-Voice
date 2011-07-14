﻿using System.Web;
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
using UniversityOfMe.Services.Professors;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Professors {

    public class ProfessorReviewController : UOFMeBaseController {
        private const string PROFESSOR_REVIEW = "The review for the professor has been posted!";
        private const string PROFESSOR_DOESNT_EXIST = "Couldn't find that professor. Please create that professor profile.";

        private IValidationDictionary theValidationDictionary;
        private IProfessorReviewService theProfessorReviewService;
        private IProfessorService theProfessorService;
        private IUniversityService theUniversityService;

        public ProfessorReviewController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theProfessorReviewService = new ProfessorReviewService(theValidationDictionary);
            theProfessorService = new ProfessorService(theValidationDictionary);
            theUniversityService = new UniversityService(theValidationDictionary);
        }


        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(CreateProfessorReviewModel professorReview) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            bool myResult = theProfessorReviewService.CreateProfessorReview(GetUserInformatonModel(), professorReview.ToModel());

            if (myResult) {
                TempData["Message"] += MessageHelper.SuccessMessage(PROFESSOR_REVIEW);
            }

            return RedirectToAction("Details", "Professor", new { id = URLHelper.ToUrlFriendly(professorReview.ProfessorName) });
        }
    }
}
