using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers.Admin;
using HaveAVoice.Models;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication;
using Social.Generic.Services;
using Social.Generic.Models;
using Social.Authentication.Helpers;
using HaveAVoice.Models.SocialWrappers;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Helpers;

namespace HaveAVoice.Controllers.Admin {
    public class AdminController<T, U, V, W, X, Y, Z> : AbstractAdminController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline> {
        public AdminController()
            : base(new BaseService<User>(new HAVBaseRepository()), 
                   UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository())),
              new HAVAuthenticationService(), 
              new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository())) {
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index() {
            return base.Index();
        }

        protected override AbstractUserModel<User> GetSocialUserInformation() {
            return SocialUserModel.Create(GetUserInformaton());
        }

        protected override AbstractUserModel<User> GetSocialUserInformation(User aUser) {
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
    }
}
