using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers.Users;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.BaseWebsite.Models;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.User.Models;
using Social.User.Services;
using Social.Users.Services;
using Social.Validation;
using UniversityOfMe.Controllers.Helpers;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Repositories.UserRepos;
using UniversityOfMe.Services.Users;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Users {
    public class UserPrivacySettingController : AbstractUserPrivacySettingController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline, PrivacySetting> {
        private const string CREATE_ACCOUNT_TITLE = "User account created!";
        private const string EDIT_SUCCESS = "Your account has been edited successfully!";
        private const string CREATE_AUTHORITY_ACCOUNT_SUCCESS = "The authority account has been created! You may now proceed to login.";
        private const string CREATE_ACCOUNT_ERROR_MESSAGE = "An error has occurred. Please try again.";
        private const string CREATE_ACCOUNT_ERROR = "Unable to create a user account.";

        private IValidationDictionary theValidationDictionary;
        private IUofMeUserService theUserService;

        public UserPrivacySettingController()
            : base(new BaseService<User>(new EntityBaseRepository()), 
                    UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()),
                    InstanceHelper.CreateAuthencationService(), 
                    new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()),
                    new UserPrivacySettingsService<User, PrivacySetting>(new EntityUserPrivacySettingsRepository())) {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theUserService = new UofMeUserService(theValidationDictionary);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Edit() {
            return base.Edit();
        }


        [AcceptVerbs(HttpVerbs.Post)]
        new public ActionResult Edit(UpdatePrivacySettingsModel<PrivacySetting> aSettings) {
            return base.Edit(aSettings);
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

        protected override ActionResult RedirectToProfile() {
            return RedirectToAction(UOMConstants.UNVIERSITY_MAIN_VIEW, UOMConstants.UNVIERSITY_MAIN_CONTROLLER, new { universityId = UniversityHelper.GetMainUniversityId(GetUserInformaton()) });
        }

        protected override ILoggedInModel<DisplayPrivacySettingsModel<PrivacySetting>> CreatedLoggedInModel(User aUser) {
            return new LoggedInWrapperModel<DisplayPrivacySettingsModel<PrivacySetting>>(aUser);
        }

        protected override string WarningMessage(string aMessage) {
            return MessageHelper.WarningMessage(aMessage);
        }
    }
}