using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Exceptions;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.ActionMethods;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Services;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Validation;
using HaveAVoice.Controllers.ActionFilters;
using HaveAVoice.Controllers.Helpers;

namespace HaveAVoice.Controllers.Users {
    public class UserController : HAVBaseController {
        private const string EDIT_SUCCESS = "Your account has been edited successfully!";
        private const string CREATE_ACCOUNT_TITLE = "User account created!";
        private const string CREATE_ACCOUNT_SUCCESS = "An email has been sent to the email you provided. Follow the instructions to activate your account so you can login and start using the site.";
        private const string CREATE_AUTHORITY_ACCOUNT_SUCCESS = "The authority account has been created! You may now proceed to login.";
        private const string EMAIL_ERROR = "Couldn't send activation e-mail so the User has been activated.";
        private const string CREATE_ACCOUNT_ERROR_MESSAGE = "An error has occurred. Please try again.";
        private const string CREATE_ACCOUNT_ERROR = "Unable to create a user account.";

        
        private IHAVUserService theService;
        private IValidationDictionary theValidationDictionary;

        public UserController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theService = new HAVUserService(theValidationDictionary);
        }

        public UserController(IHAVUserService service, IHAVBaseService baseService)
            : base(baseService) {
            theService = service;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index() {
            if (Session["UserInformation"] == null)
                return View("Login");
            else
                return RedirectToAction("Index", "Message");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create() {
            if (IsLoggedIn()) {
                return RedirectToProfile();
            }
            return View("Create", new CreateUserModelBuilder() { 
                States = new SelectList(HAVConstants.STATES, "Select"),
                Genders = new SelectList(HAVConstants.GENDERS, "Select")
            });
        }

        [CaptchaValidator]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(CreateUserModelBuilder aUserToCreate, bool captchaValid) {
            if (IsLoggedIn()) {
                return RedirectToProfile();
            }
            try {
                bool myResult = theService.CreateUser(aUserToCreate.Build(), captchaValid, aUserToCreate.Agreement, HttpContext.Request.UserHostAddress);
                if (myResult) {
                    return SendToResultPage(CREATE_ACCOUNT_TITLE, CREATE_ACCOUNT_SUCCESS);
                }
            } catch (EmailException e) {
                LogError(e, EMAIL_ERROR);
                return SendToResultPage("User account created! You can login and start posting!");
            } catch (Exception e) {
                ViewData["Message"] = MessageHelper.ErrorMessage(CREATE_ACCOUNT_ERROR_MESSAGE);
                LogError(e, CREATE_ACCOUNT_ERROR);
            }

            AddStatesAndGenders(aUserToCreate, aUserToCreate.State);

            return View("Create", aUserToCreate);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CreateAuthority(string email, string token, string authorityType) {
            if (IsLoggedIn()) {
                return RedirectToProfile();
            }
            return View("CreateAuthority", new CreateAuthorityUserModelBuilder() {
                Email = email,
                Token = token,
                AuthorityType = authorityType,
                States = new SelectList(HAVConstants.STATES, "Select"),
                Genders = new SelectList(HAVConstants.GENDERS, "Select")
            });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateAuthority(CreateAuthorityUserModelBuilder aBuilder) {
            if (IsLoggedIn()) {
                return RedirectToProfile();
            }
            try {
                bool myResult = theService.CreateUserAuthority(aBuilder.Build(), aBuilder.Token, aBuilder.AuthorityType, aBuilder.Agreement, HttpContext.Request.UserHostAddress);
                if (myResult) {
                    return SendToResultPage(CREATE_ACCOUNT_TITLE, CREATE_AUTHORITY_ACCOUNT_SUCCESS);
                }
            } catch (Exception e) {
                ViewData["Message"] = MessageHelper.ErrorMessage(CREATE_ACCOUNT_ERROR_MESSAGE);
                LogError(e, CREATE_ACCOUNT_ERROR);
            }

            AddStatesAndGenders(aBuilder, aBuilder.RepresentingState);
            return View("CreateAuthority", aBuilder);
        }

        private void AddStatesAndGenders(CreateUserModel aUserModel, string aState) {
            aUserModel.Genders = new SelectList(HAVConstants.GENDERS, aUserModel.SelectedGender.ToString());
            aUserModel.States = new SelectList(HAVConstants.STATES, aState);
        }

        public ActionResult Edit() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            } 
            User myUser = GetUserInformaton();

            EditUserModel myModel = null;

            try {
                myModel = theService.GetUserForEdit(myUser);
            } catch (Exception e) {
                LogError(e, "Unable to get the model to edit the user");
                SendToErrorPage("Unable to edit your settings. Please try again.");
            }

            return View("Edit", myModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(EditUserModel userToEdit) {
            try {
                if (theService.EditUser(userToEdit)) {
                    return SendToResultPage(EDIT_SUCCESS);
                }
            } catch (Exception exception) {
                LogError(exception, "Error editing the user.");
                ViewData["Message"] = MessageHelper.ErrorMessage("An error has occurred please try your submission again later.");
            }

            return View("Edit", userToEdit);
        }

        public ActionResult FriendFeed() {
            return null;
        }
    }
}