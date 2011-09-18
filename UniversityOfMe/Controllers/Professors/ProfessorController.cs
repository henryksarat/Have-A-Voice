﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.BaseWebsite.Models;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Exceptions;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Photo.Exceptions;
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
        IUniversityService theUniversityService;

        public ProfessorController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theProfessorService = new ProfessorService(theValidationDictionary);
            theUniversityService = new UniversityService(theValidationDictionary);
        }

        public ActionResult List(string universityId) {
            return RedirectToAction("Professor", "Search", new { searchString = string.Empty, page = 1 });
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
                return RedirectToAction("Details", new { universityId = universityId, id = URLHelper.ToUrlFriendly(firstName + " " + lastName) });
            }

            return RedirectToAction("Create");
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Details(string universityId, string id) {
            try {
                UserInformationModel<User> myUserInfo = GetUserInformatonModel();
                User myUser = myUserInfo == null ? null : myUserInfo.Details;

                bool myExists = theProfessorService.IsProfessorExists(universityId, URLHelper.FromUrlFriendlyToNormalString(id));

                if (!myExists) {
                    TempData["Message"] += MessageHelper.WarningMessage(PROFESSOR_DOESNT_EXIST);
                    return RedirectToAction("Create", "Professor");
                }

                Professor myProfessor = theProfessorService.GetProfessor(universityId, id);

                if (myProfessor.ProfessorReviews.Count() == 0) {
                    ViewData["Message"] = MessageHelper.NormalMessage(NO_PROFESSOR_REVIEWS);
                }

                IDictionary<string, string> myAcademicTerms = DictionaryHelper.DictionaryWithSelect();
                myAcademicTerms = theUniversityService.CreateAcademicTermsDictionaryEntry();

                LoggedInWrapperModel<CreateProfessorReviewModel> myLoggedIn = new LoggedInWrapperModel<CreateProfessorReviewModel>(myUser);
                CreateProfessorReviewModel myProfessorReview = new CreateProfessorReviewModel() {
                    Professor = myProfessor,
                    ProfessorName = myProfessor.FirstName + " " + myProfessor.LastName,
                    AcademicTerms = new SelectList(myAcademicTerms, "Value", "Key"),
                    Years = new SelectList(UOMConstants.ACADEMIC_YEARS, DateTime.UtcNow.Year),
                    University = myProfessor.University,
                    UniversityId = myProfessor.UniversityId
                };

                myLoggedIn.Set(myProfessorReview);

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
                    TempData["Message"] += MessageHelper.SuccessMessage(PROFESSOR_PICTURE_SUGGESTION_SUCCESS);
                }
            } catch(PhotoException myException) {
                LogError(myException, PROFESSOR_SUGGESTED_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(PROFESSOR_SUGGESTED_ERROR);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] += MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            }

            return RedirectToAction("Details", new { id = URLHelper.ToUrlFriendly(firstName + " " + lastName) });
        }
    }
}
