using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers.Friends;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.BaseWebsite.Models;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Users.Services;
using UniversityOfMe.Controllers.Helpers;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Repositories.Friends;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Users {
    public class FriendController : AbstractFriendController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline, Friend> {

        public FriendController()
            : base(new BaseService<User>(new EntityBaseRepository()), 
                   UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()),
                   InstanceHelper.CreateAuthencationService(), 
                   new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()),
                   new EntityFriendRepository()) {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Add(int id) {
            return base.Add(id);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        new public ActionResult Delete(int id) {
            return base.Delete(id);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult List() {
            return base.List();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Pending() {
            return base.Pending();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        new public ActionResult Approve(int id) {
            return base.Approve(id);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        new public ActionResult Decline(int id) {
            return base.Decline(id);
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

        protected override ILoggedInListModel<Friend> CreateLoggedInListModel(User aUser) {
            return new LoggedInListModel<Friend>(aUser);
        }

        protected override ActionResult RedirectToProfile() {
            return RedirectToAction(UOMConstants.UNVIERSITY_MAIN_VIEW, UOMConstants.UNVIERSITY_MAIN_CONTROLLER, new { universityId = UniversityHelper.GetMainUniversityId(GetUserInformaton()) });
        }
    }
}
