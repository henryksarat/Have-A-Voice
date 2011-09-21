using System;
using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers.Users;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Users.Services;
using Social.Validation;
using UniversityOfMe.Controllers.Helpers;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.SocialModels;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Repositories.UserRepos;
using UniversityOfMe.Services.Users;
using UniversityOfMe.UserInformation;
using Social.Generic.Exceptions;
using UniversityOfMe.Services.Email;

namespace UniversityOfMe.Controllers.Users {
    public class UserController : AbstractUserController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline> {
        private const string CREATE_ACCOUNT_TITLE = "User account created!";
        private const string EDIT_SUCCESS = "Your account has been edited successfully!";
        private const string CREATE_AUTHORITY_ACCOUNT_SUCCESS = "The authority account has been created! You may now proceed to login.";
        private const string CREATE_ACCOUNT_ERROR_MESSAGE = "An error has occurred. Please try again.";
        private const string CREATE_ACCOUNT_ERROR = "Unable to create a user account.";

        private IValidationDictionary theValidationDictionary;
        private IUofMeUserService theUserService;

        public UserController()
            : base(new BaseService<User>(new EntityBaseRepository()), 
                    UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()),
                    InstanceHelper.CreateAuthencationService(), 
                    new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()),
                    new EntityUserRepository(),
                    new EmailService()) {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theUserService = new UofMeUserService(theValidationDictionary);
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
            return View("Create", new CreateUserModel() {
                RegisteredUserCount = theUserService.GetNumberOfRegisteredUsers(),
                Genders = new SelectList(Constants.GENDERS, Constants.SELECT)
            });
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Activation(string message) {
            return View("Activation", new StringModel(message));
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Activation(string email, string message) {
            bool myResult = theUserService.SendConfirmationEmail(email, UOMConstants.BASE_URL, UOMConstants.ACTIVATION_SUBJECT, UOMConstants.ACTIVATION_BODY);
            if (myResult) {
                return SendToResultPage("A activation email has been sent to " + email);
            }

            return RedirectToAction("Activation", "User", new { message = message });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(CreateUserModel aUserToCreate, bool agreement) {
            ActionResult myActionResult = base.Create(aUserToCreate, true, agreement,
                                                      UOMConstants.BASE_URL, UOMConstants.ACTIVATION_SUBJECT, UOMConstants.ACTIVATION_BODY,
                                                      new RegistrationStrategy());

            if (myActionResult == null) {
                AddGenders(aUserToCreate, aUserToCreate.State);
                myActionResult = View("Create", aUserToCreate);
            }

            return myActionResult;
        }
        
        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Edit() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();

            EditUserModel myModel = null;

            try {
                LoggedInWrapperModel<EditUserModel> myLoggedInModel = new LoggedInWrapperModel<EditUserModel>(myUser);
                myModel = theUserService.GetUserForEdit(myUser);
                myLoggedInModel.Set(myModel);
                return View("Edit", myLoggedInModel);
            } catch (Exception e) {
                LogError(e, "Unable to get the model to edit the user");
                return SendToErrorPage("Unable to edit your settings. Please try again.");
            }
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Edit(EditUserModel userToEdit) {
            try {

                string myPassword = userToEdit.NewPassword;
                if (myPassword.Trim() == string.Empty) {
                    myPassword = userToEdit.OriginalPassword;
                } else {
                    myPassword = HashHelper.DoHash(myPassword);
                }

                if (theUserService.EditUser(userToEdit, myPassword)) {
                    if (string.IsNullOrEmpty(userToEdit.NewEmail)) {
                        TempData["Message"] += MessageHelper.SuccessMessage(EDIT_SUCCESS);
                    } else {
                        TempData["Message"] += MessageHelper.SuccessMessage(EDIT_SUCCESS + " To finalize the email change please click the link sent to the new email you entered to confirm you are teh owner.");
                    }

                    ForceUserInformationRefresh();
                    return RedirectToAction("Edit");
                }
            } catch(CustomException myException) {
                LogError(myException, myException.Message);
                TempData["Message"] += MessageHelper.ErrorMessage(myException.Message);
            } catch (Exception exception) {
                LogError(exception, "Error editing the user.");
                TempData["Message"] += MessageHelper.ErrorMessage("An error has occurred please try your submission again later.");
                theValidationDictionary.ForceModleStateExport();
            }

            return RedirectToAction("Edit");
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

        private void AddGenders(CreateUserModel aUserModel, string aState) {
            aUserModel.Genders = new SelectList(Constants.GENDERS, aUserModel.Gender);
        }

        protected override ActionResult RedirectToProfile() {
            return RedirectToAction(UOMConstants.UNVIERSITY_MAIN_VIEW, UOMConstants.UNVIERSITY_MAIN_CONTROLLER, new { universityId = UniversityHelper.GetMainUniversityId(GetUserInformaton()) });
        }

        protected override string WarningMessage(string aMessage) {
            return MessageHelper.WarningMessage(aMessage);
        }
    }
}