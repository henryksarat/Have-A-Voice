using System;
using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers.Users;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.ActionMethods;
using HaveAVoice.Models;
using HaveAVoice.Models.SocialWrappers;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.Generic.Constants;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Validation;

namespace HaveAVoice.Controllers.Users {
    public class UserController : AbstractUserController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline> {
        private const string CREATE_ACCOUNT_TITLE = "User account created!";
        private const string EDIT_SUCCESS = "Your account has been edited successfully!";
        private const string CREATE_AUTHORITY_ACCOUNT_SUCCESS = "The authority account has been created! You may now proceed to login.";
        private const string CREATE_ACCOUNT_ERROR_MESSAGE = "An error has occurred. Please try again.";
        private const string CREATE_ACCOUNT_ERROR = "Unable to create a user account.";
        
        private IHAVUserService theService;
        private IValidationDictionary theValidationDictionary;

        public UserController()
            : base(new BaseService<User>(new HAVBaseRepository()), 
            UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository()))
            , new HAVAuthenticationService(), new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository()),
            new EntityHAVUserRepository()) {
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theService = new HAVUserService(theValidationDictionary);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Index() {
            return base.Index();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create() {
            if (IsLoggedIn()) {
                return RedirectToProfile();
            }
            return View("Create", new CreateUserModelBuilder() {
                States = new SelectList(UnitedStates.STATES, Constants.SELECT),
                Genders = new SelectList(Constants.GENDERS, Constants.SELECT)
            });
        }

        [CaptchaValidator]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(CreateUserModelBuilder aUserToCreate, bool captchaValid) {
            ActionResult myActionResult = base.Create(aUserToCreate.Build(), captchaValid, aUserToCreate.Agreement, 
                                                      HAVConstants.BASE_URL, HAVConstants.ACTIVATION_SUBJECT, HAVConstants.ACTIVATION_BODY,
                                                      new RegistrationStrategy());

            if (myActionResult == null) {
                AddStatesAndGenders(aUserToCreate, aUserToCreate.State);
                myActionResult = View("Create", aUserToCreate);
            }

            return myActionResult;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CreateAuthority(string email, AuthorityVerificationModel model) {
            if (IsLoggedIn()) {
                return RedirectToProfile();
            }
            return View("CreateAuthority", new CreateAuthorityUserModelBuilder() {
                Email = email,
                Token = model.Token,
                AuthorityType = model.AuthorityType,
                UserPosition = model.AuthorityPosition,
                States = new SelectList(UnitedStates.STATES, Constants.SELECT),
                Genders = new SelectList(Constants.GENDERS, Constants.SELECT)
            });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateAuthority(CreateAuthorityUserModelBuilder aBuilder) {
            if (IsLoggedIn()) {
                return RedirectToProfile();
            }
            try {
                bool myResult = theService.CreateUserAuthority(aBuilder.Build(), aBuilder.Token, aBuilder.AuthorityType, 
                                                               aBuilder.Agreement, HttpContext.Request.UserHostAddress);
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
                    TempData["Message"] = MessageHelper.SuccessMessage(EDIT_SUCCESS);
                    RefreshUserInformation();
                    return RedirectToAction("Edit");
                }
            } catch (Exception exception) {
                LogError(exception, "Error editing the user.");
                ViewData["Message"] = MessageHelper.ErrorMessage("An error has occurred please try your submission again later.");
            }

            return View("Edit", userToEdit);
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
            return MessageHelper.ErrorMessage(aMessage);
        }

        protected override string NormalMessage(string aMessage) {
            return MessageHelper.NormalMessage(aMessage);
        }

        protected override string SuccessMessage(string aMessage) {
            return MessageHelper.SuccessMessage(aMessage);
        }

        private void AddStatesAndGenders(CreateUserModel aUserModel, string aState) {
            aUserModel.Genders = new SelectList(Constants.GENDERS, aUserModel.Gender);
            aUserModel.States = new SelectList(UnitedStates.STATES, aState);
        }
    }
}