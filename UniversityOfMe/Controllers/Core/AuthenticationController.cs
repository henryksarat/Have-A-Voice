using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers.Core;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.Generic.Models;
using Social.Generic.Services;
using UniversityOfMe.Controllers.Helpers;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Repositories;

namespace UniversityOfMe.Controllers.Core {
    public class AuthenticationController : AbstractAuthenticationController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline> {
        private const string ACCOUNT_ACTIVATED_BODY = "You may now login and access have a voice!";

        public AuthenticationController()
            : base(new BaseService<User>(new EntityBaseRepository()),
                   UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())),
                   InstanceHelper.CreateAuthencationService(),
                   new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())) {
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Login() {
            return base.Login();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        new public ActionResult Login(string email, string password, bool rememberMe) {
            return base.Login(email, password, rememberMe);
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
            return aMessage;
        }

        protected override string NormalMessage(string aMessage) {
            return aMessage;
        }

        protected override string SuccessMessage(string aMessage) {
            return aMessage;
        }
    }
}
