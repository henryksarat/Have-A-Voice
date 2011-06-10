﻿using System;
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
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.GeneralPostings;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.GeneralPostings {
    public class GeneralPostingController : UOFMeBaseController {
        private const string NO_GENERAL_POSTING = "There are no general postings yet.";
        
        IGeneralPostingService theGeneralPostingService;
        IValidationDictionary theValidationDictionary;

        public GeneralPostingController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
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

            try {
                LoggedInListModel<GeneralPosting> myLoggedInListModel = new LoggedInListModel<GeneralPosting>(GetUserInformatonModel().Details);
                IEnumerable<GeneralPosting> myGeneralPostings = theGeneralPostingService.GetGeneralPostingsForUniversity(universityId);

                myLoggedInListModel.Set(myGeneralPostings);

                if (myGeneralPostings.Count<GeneralPosting>() == 0) {
                    ViewData["Message"] = MessageHelper.NormalMessage(NO_GENERAL_POSTING);
                }

                return View("List", myLoggedInListModel);
            } catch(Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                return SendToErrorPage(ErrorKeys.ERROR_MESSAGE);
            }
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Create(string universityId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, universityId)) {
                return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
            }

            try {
                LoggedInWrapperModel<GeneralPosting> myLoggedInModel = new LoggedInWrapperModel<GeneralPosting>(GetUserInformatonModel().Details);
                return View("Create", myLoggedInModel);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                return SendToErrorPage(ErrorKeys.ERROR_MESSAGE);
            }
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
                TempData["Message"] = MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
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
                LoggedInWrapperModel<GeneralPosting> myLoggedInModel = new LoggedInWrapperModel<GeneralPosting>(GetUserInformatonModel().Details);
                GeneralPosting myGeneralPosting = theGeneralPostingService.GetGeneralPosting(id);
                myLoggedInModel.Set(myGeneralPosting);

                return View("Details", myLoggedInModel);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                return SendToResultPage(ErrorKeys.ERROR_MESSAGE);
            }
        }
    }
}
