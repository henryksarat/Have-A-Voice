using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers.Core;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Models;
using HaveAVoice.Models.SocialWrappers;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Users.Services;

namespace HaveAVoice.Controllers.Core {
    public class PasswordController : AbstractPasswordController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline> {
        public PasswordController()
            : base(new BaseService<User>(new HAVBaseRepository()), 
                   UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository())),
                   new HAVAuthenticationService(), 
                   new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository()),
                   new EntityHAVUserRetrievalRepository(),
                   new EntityHAVPasswordRepository()) {
            HAVUserInformationFactory.SetInstance(GetUserInformationInstance());
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Request() {
            return base.Request();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        new public ActionResult Request(string email) {
            return base.Request(email, HAVConstants.BASE_URL);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Process(string id) {
            return base.Process(id);
        }

        [AcceptVerbs(HttpVerbs.Post)]
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

        protected override ActionResult RedirectToProfile() {
            return RedirectToAction("Show", "Profile");
        }
    }
}
