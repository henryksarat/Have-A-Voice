using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers.Core;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.Generic.ActionFilters;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Users.Services;
using UniversityOfMe.Controllers.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Repositories;
using UniversityOfMe.Repositories.Password;
using UniversityOfMe.Repositories.UserRepos;
using UniversityOfMe.Services.Email;
using UniversityOfMe.UserInformation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models.SocialModels;

namespace UniversityOfMe.Controllers.Core {
    public class PasswordController : AbstractPasswordController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline> {
        private const string FORGOT_PASSWORD_TITLE = "UniversityOf.Me | forgot password";
        private const string FORGOT_PASSWORD_BODY = "Hello! <br/ ><br/ > To continue with the forgot password process and change your password please click the following link or copy and paste it into your browser: <br/ >";

        public PasswordController()
            : base(new BaseService<User>(new EntityBaseRepository()),
                   UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()),
                   InstanceHelper.CreateAuthencationService(), 
                   new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()),
                   new EntityUserRetrievalRepository(),
                   new EntityPasswordRepository(),
                   new EmailService()) {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        new public ActionResult Request() {
            return base.Request();
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Request(string email) {
            return base.Request(UOMConstants.BASE_URL, email, FORGOT_PASSWORD_TITLE, FORGOT_PASSWORD_BODY);
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        new public ActionResult Process(string id) {
            return base.Process(id);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        new public ActionResult Process(string email, string forgotPasswordHash, string password, string retypedPassword) {
            return base.Process(email, forgotPasswordHash, password, retypedPassword);
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
            return MessageHelper.ErrorMessage(aMessage);
        }

        protected override ActionResult RedirectToProfile() {
            return RedirectToAction("Show", "Profile");
        }
    }
}
