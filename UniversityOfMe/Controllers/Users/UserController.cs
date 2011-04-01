using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers.Users;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.Generic.Constants;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Validation;
using UniversityOfMe.Controllers.Helpers;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Repositories.UserRepos;

namespace UniversityOfMe.Controllers.Users {
    public class UserController : AbstractUserController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline> {
        private const string CREATE_ACCOUNT_TITLE = "User account created!";
        private const string EDIT_SUCCESS = "Your account has been edited successfully!";
        private const string CREATE_AUTHORITY_ACCOUNT_SUCCESS = "The authority account has been created! You may now proceed to login.";
        private const string CREATE_ACCOUNT_ERROR_MESSAGE = "An error has occurred. Please try again.";
        private const string CREATE_ACCOUNT_ERROR = "Unable to create a user account.";

        public UserController()
            : base(new BaseService<User>(new EntityBaseRepository()), 
            UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())),
            InstanceHelper.CreateAuthencationService(), 
            new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()),
            new EntityUserRepository()) { }

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
                Genders = new SelectList(Constants.GENDERS, Constants.SELECT)
            });
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

        private void AddGenders(CreateUserModel aUserModel, string aState) {
            aUserModel.Genders = new SelectList(Constants.GENDERS, aUserModel.Gender);
        }
    }
}