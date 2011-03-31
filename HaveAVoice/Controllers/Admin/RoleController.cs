using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers.Admin;
using BaseWebsite.Models;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.ActionMethods;
using HaveAVoice.Models;
using HaveAVoice.Models.SocialWrappers;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.AdminFeatures;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.UserFeatures;
using Social.Admin;
using Social.Admin.Services;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.Generic.Models;
using Social.Generic.Services;

namespace HaveAVoice.Controllers.Admin {
    public class RoleController : AbstractRoleController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline> {
        public RoleController()
            : base(new BaseService<User>(new HAVBaseRepository()), 
            UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository()))
            , new HAVAuthenticationService(), new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository()), new EntityHAVRoleRepository(), new EntityHAVPermissionRepository()) {
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Index() {
            return base.Index();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Create() {
            return base.Create();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(RoleViewModel model) {
             return base.Create(model);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Edit(int id) {
            return base.Edit(id);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        new public ActionResult Edit(RoleViewModel role) {
            return base.Edit(role);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Delete(int id) {
            return base.Delete(id);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        new public ActionResult Delete(Role roleToDelete) {
            return base.Delete(roleToDelete);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult SwitchUserRoles() {
            return base.SwitchUserRoles();
        }

        [ActionName("SwitchUserRoles")]
        [AcceptParameter(Name = "button", Value = "Get users for this role")]
        [AcceptVerbs(HttpVerbs.Post)]
        new public ActionResult SwitchUserRoles(int CurrentRoleId, int MoveToRoleId) {
            return base.SwitchUserRoles(CurrentRoleId, MoveToRoleId);
        }

        [ActionName("SwitchUserRoles")]
        [AcceptParameter(Name = "button", Value = "Move users to this role")]
        [AcceptVerbs(HttpVerbs.Post)]
        new public ActionResult SwitchUserRoles(int[] SelectedUserIds, int CurrentRoleId, int MoveToRoleId) {
            return base.SwitchUserRoles(SelectedUserIds, CurrentRoleId, MoveToRoleId);
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

        protected override Role CreateEmptyRole() {
            return Role.CreateRole(0, string.Empty, string.Empty, false, false);
        }

        protected override AbstractRoleViewModel<Role, Permission> CreateRoleViewModel(Role aRole, IEnumerable<Permission> aPermissions) {
            return new RoleViewModel(aRole) {
                AllPermissions = aPermissions
            };
        }

        protected override AbstractRoleModel<Role> CreateSocialRoleModel(Role aRole) {
            return SocialRoleModel.Create(aRole);
        }

        protected override AbstractUserModel<User> GetSocialUserInformation() {
            return SocialUserModel.Create(GetUserInformaton());
        }

        protected override AbstractUserModel<User> CreateSocialUserModel(User aUser) {
            return SocialUserModel.Create(aUser);
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

        protected override IProfilePictureStrategy<User> ProfilePictureStrategy() {
            return new ProfilePictureStrategy();
        }

        protected override SelectList GetCurrentRoles(IEnumerable<Role> aRoles, int aSelectedCurrentRoleId) {
            List<SelectListItem> myFinalRoles = new List<SelectListItem>();

            SelectListItem mySelectList = new SelectListItem();
            mySelectList.Text = "Select role";
            mySelectList.Value = "0";
            myFinalRoles.Add(mySelectList);
            foreach (Role myRole in aRoles) {
                mySelectList = new SelectListItem();
                mySelectList.Text = myRole.Name;
                mySelectList.Value = myRole.Id.ToString();
                mySelectList.Selected = true;
                myFinalRoles.Add(mySelectList);
            }

            return new SelectList(myFinalRoles, "Value", "Text", aSelectedCurrentRoleId);
        }

        protected override SelectList GetMoveToRoles(IEnumerable<Role> aRoles, int aSelectedMoveToRoleId) {
            List<SelectListItem> myFinalRoles = new List<SelectListItem>();

            SelectListItem mySelectList = new SelectListItem();
            mySelectList.Text = "Select role";
            mySelectList.Value = "0";
            myFinalRoles.Add(mySelectList);
            foreach (Role myRole in aRoles) {
                mySelectList = new SelectListItem();
                mySelectList.Text = myRole.Name;
                mySelectList.Value = myRole.Id.ToString();
                mySelectList.Selected = true;
                myFinalRoles.Add(mySelectList);
            }

            return new SelectList(myFinalRoles, "Value", "Text", aSelectedMoveToRoleId);
        }
    }
}
