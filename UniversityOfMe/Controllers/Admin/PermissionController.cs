using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers.Admin;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.Generic.ActionFilters;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Users.Services;
using UniversityOfMe.Controllers.Helpers;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Repositories;
using UniversityOfMe.Repositories.AdminRepos;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Admin {
    public class PermissionController : AbstractPermissionController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline> {
        public PermissionController()
            : base(new BaseService<User>(new EntityBaseRepository()), 
            UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())),
            InstanceHelper.CreateAuthencationService(), 
            new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new EntityPermissionRepository()) {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())));
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Index() {
            return base.Index();
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        new public ActionResult Create() {
            return base.Create();
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(SocialPermissionModel model) {
            return base.Create(model);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Edit(int id) {
            return base.Edit(id);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        new public ActionResult Edit(Permission permission) {
            return base.Edit(permission);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Delete(int id) {
            return base.Delete(id);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        new public ActionResult Delete(Permission permission) {
            return base.Delete(permission);
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

        protected override AbstractPermissionModel<Permission> CreateSocialPermissionModel(Permission aPermission) {
            return SocialPermissionModel.Create(aPermission);
        }

        protected override ActionResult RedirectToProfile() {
            return RedirectToAction(UOMConstants.UNVIERSITY_MAIN_VIEW, UOMConstants.UNVIERSITY_MAIN_CONTROLLER, new { universityId = UniversityHelper.GetMainUniversity(GetUserInformaton()) });
        }
    }
}
