﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Social.Authentication;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Users.Services;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services;
using UniversityOfMe.Services.Classes;
using UniversityOfMe.UserInformation;
using Social.Generic.Models;

namespace UniversityOfMe.Controllers.Classes {
    public class ClassController : UOFMeBaseController {
        private const string GET_CLASS_ERROR = "Error getting the class list for the university. Please try again.";
        private const string NO_CLASSES = "There are no classes for the university created.";
        private const string CLASS_DOESNT_EXIST = "That class doesn't seem to exist. Go ahead and create it so people can post to it.";
        private const string VIEW_ALL_MEMBERS_ERROR = "Unable to retrieve the class enrollment list. Please try again.";

        private const string CREATE_CLASS_ERROR = "Unable to load the create class page. Please try again.";
        private const string CLASS_DETAILS_ERROR = "Unable to load load the class details. Please try again.";

        IClassService theClassService;
        IUniversityService theUniversityService;
        IValidationDictionary theValidationDictionary;

        public ClassController() {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theClassService = new ClassService(theValidationDictionary);
            theUniversityService = new UniversityService(theValidationDictionary);
        }

        public ActionResult List(string universityId) {
            return RedirectToAction("Class", "Search", new { searchString = string.Empty, page = 1 });
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

                CreateClassModel myClassModel = new CreateClassModel() {

                };

                LoggedInWrapperModel<CreateClassModel> myLoggedIn = new LoggedInWrapperModel<CreateClassModel>(myUser);
                myLoggedIn.Set(myClassModel);

                return View("Create", myLoggedIn);
            } catch (Exception myExpcetion) {
                LogError(myExpcetion, CREATE_CLASS_ERROR);
                return SendToErrorPage(CREATE_CLASS_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(CreateClassModel aClass) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                Class myResult = theClassService.CreateClass(GetUserInformatonModel(), aClass);
                if (myResult != null) {
                    TempData["Message"] = SuccessMessage("Class added! You can now tag books to it!");
                }
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                TempData["Message"] += ErrorKeys.ERROR_MESSAGE;
                theValidationDictionary.ForceModleStateExport();
            }

            return RedirectToAction("Create");
        }
        
        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult DetailsWithClassId(string universityId, int classId, ClassViewType classViewType) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            if (!UniversityHelper.IsFromUniversity(GetUserInformatonModel().Details, universityId)) {
                return SendToResultPage(UOMConstants.NOT_APART_OF_UNIVERSITY);
            }

            try {
                ClassDetailsModel myClass = theClassService.GetClass(GetUserInformatonModel(), classId, classViewType);
                LoggedInWrapperModel<ClassDetailsModel> myLoggedInModel = new LoggedInWrapperModel<ClassDetailsModel>(GetUserInformatonModel().Details);
                myLoggedInModel.Set(myClass);

                ViewData["ClassViewType"] = classViewType;
                return View("Details", myLoggedInModel);
            } catch (Exception myException) {
                LogError(myException, ErrorKeys.ERROR_MESSAGE);
                return SendToResultPage(ErrorKeys.ERROR_MESSAGE);
            }
        }
    }
}
