using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers.Core;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Users.Services;
using UniversityOfMe.Controllers.Helpers;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.SocialModels;
using UniversityOfMe.Repositories;
using UniversityOfMe.UserInformation;
using UniversityOfMe.Services.Users;
using Social.Validation;
using System;
using Social.Generic.ActionFilters;

namespace UniversityOfMe.Controllers.Core {
    public class AuthenticationController : AbstractAuthenticationController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline> {
        private const string ACCOUNT_ACTIVATED_BODY = "You may now login and access have a voice!";
        private const string CHANGE_EMAIL_ERROR = "An error occurred while trying to change your email. Please try again.";
        private const string CHANGE_EMAIL_SUCCESS = "Your email has been changed! You can now login with it.";

        IUofMeUserService theUserService;
        IValidationDictionary theValidationDictionary;

        public AuthenticationController()
            : base(new BaseService<User>(new EntityBaseRepository()),
                   UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()),
                   InstanceHelper.CreateAuthencationService(),
                   new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())) {

            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theUserService = new UofMeUserService(theValidationDictionary);
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult ChangeEmail(string id) {
            LogOut();
            return View("ChangeEmail", new StringModel(id));
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult ChangeEmail(string oldEmail, string newEmailHash) {
            try {
                bool myResult = theUserService.ChangeEmail(oldEmail, newEmailHash);

                if (myResult) {
                    TempData["Message"] += MessageHelper.SuccessMessage(CHANGE_EMAIL_SUCCESS);
                    return RedirectToLogin();
                }
            } catch (Exception myException) {
                LogError(myException, CHANGE_EMAIL_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(CHANGE_EMAIL_ERROR);
            }

            return RedirectToAction("ChangeEmail", new { id = newEmailHash });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Login() {
            return base.Login();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        new public ActionResult Login(string loginEmail, string loginPassword, bool rememberMe) {
            return base.Login(loginEmail, loginPassword, rememberMe);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ActivateAccount(string id) {
            return base.ActivateAccount(id, ACCOUNT_ACTIVATED_BODY);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult LogOut() {
            return base.LogOut();
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

        protected override string WarningMessage(string aMessage) {
            return MessageHelper.WarningMessage(aMessage);
        }

        protected override ActionResult RedirectToProfile() {
            return RedirectToAction("IntermediateRedirectToProfile");
        }

        public ActionResult IntermediateRedirectToProfile() {
            return RedirectToAction(UOMConstants.UNVIERSITY_MAIN_VIEW, UOMConstants.UNVIERSITY_MAIN_CONTROLLER, new { universityId = UniversityHelper.GetMainUniversityId(GetUserInformaton()) });
        }
    }
}
