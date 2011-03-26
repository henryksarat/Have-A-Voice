using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.ActionMethods;
using HaveAVoice.Models;
using HaveAVoice.Models.SocialWrappers;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.AdminFeatures;
using HaveAVoice.Services;
using Social.Admin;
using Social.Admin.Helpers;
using Social.Admin.Services;
using Social.Generic;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;

namespace HaveAVoice.Controllers.Admin {
    public class RoleController : AdminBaseController {
        private static string ERROR_MESSAGE = "An error has occurred. Please try again later.";
        private static string CREATE_MODEL_ERROR = "Unable to create model.";
        private static string GET_ROLE_ERROR = "Unable to get role.";
        private static string GET_ALL_ROLES_ERROR = "Unable to get all roles.";
        private static string NO_ROLES = "There are no roles.";
        private static string CREATE_ROLE_ERROR = "Unable to create role.";
        private static string EDIT_ROLE_ERROR = "Unable to edit role.";
        private static string ROLE_NOT_FOUND = "Role not found.";
        private static string PAGE_NOT_FOUND = "You do not have access.";

        private IRoleService<User, Role> theRoleService;
        private IPermissionService<User, Permission> thePermissionService;
        private ModelStateWrapper theModelState;

        public RoleController() 
            : base(new HAVBaseService(new HAVBaseRepository())) {
            theModelState = new ModelStateWrapper(this.ModelState);
            theRoleService = new RoleService<User, Role>(theModelState, new EntityHAVRoleRepository());
            thePermissionService = new PermissionService<User, Permission>(theModelState, new EntityHAVPermissionRepository());
        }

        public RoleController(IHAVBaseService aBaseService, IRoleService<User, Role> myRoleService, IPermissionService<User, Permission> myPermissionService)
            : base(aBaseService) {
            theRoleService = myRoleService;
            thePermissionService = myPermissionService;
        }

        public ActionResult Index() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            if (!PermissionHelper<User>.AllowedToPerformAction(GetUserInformatonModel(), SocialPermission.View_Roles)) {
                return SendToErrorPage(PAGE_NOT_FOUND);
            }

            IEnumerable<Role> roles = null;

            try {
                roles = theRoleService.GetAllRoles().ToList<Role>();
            } catch (Exception e) {
                LogError(e, GET_ALL_ROLES_ERROR);
                return SendToErrorPage(GET_ALL_ROLES_ERROR);
            }
            if (roles.Count() == 0) {
                ViewData["Message"] = MessageHelper.NormalMessage(NO_ROLES);
            }
            
            return View("Index", roles);
        }

        public ActionResult Create() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            if(!PermissionHelper<User>.AllowedToPerformAction(GetUserInformatonModel(), SocialPermission.Create_Role)) {
                return SendToErrorPage(PAGE_NOT_FOUND);
            }

            try {
                Role role = Role.CreateRole(0, string.Empty, string.Empty, false, false);
                RoleModel myModel = CreateRoleModel(role);

                return View("Create", myModel);
            } catch (Exception e) {
                LogError(e, CREATE_MODEL_ERROR);
                return SendToErrorPage(ERROR_MESSAGE);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(RoleModel model) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                AbstractRoleModel<Role> mySocialModel = SocialRoleModel.Create(model.Role);
                if (theRoleService.Create(GetUserInformatonModel(), mySocialModel, model.SelectedPermissionsIds)) {
                    return RedirectToAction("Index");
                }
            } catch (Exception e) {
                LogError(e, CREATE_ROLE_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(ERROR_MESSAGE);
            }

            try {
                RoleModel myModel = CreateRoleModel(model);
                return View("Create", myModel);
            } catch (Exception e) {
                LogError(e, CREATE_MODEL_ERROR);
                return SendToErrorPage(ERROR_MESSAGE);
            }
        }

        public ActionResult Edit(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            if (!PermissionHelper<User>.AllowedToPerformAction(myUserInformation, SocialPermission.Edit_Role)) {
                return SendToErrorPage(PAGE_NOT_FOUND);
            }

            Role role = null;
            try {
                role = theRoleService.FindRole(id);
            } catch (Exception e) {
                LogError(e, GET_ROLE_ERROR);
                return SendToErrorPage(ERROR_MESSAGE);
            }

            if (role == null) {
                return SendToResultPage(ROLE_NOT_FOUND);
            }
            
            try {
                RoleModel myModel = CreateRoleModel(role);
                return View("Edit", myModel);
            } catch (Exception e) {
                LogError(e, CREATE_MODEL_ERROR);
                return SendToErrorPage(ERROR_MESSAGE);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(RoleModel role) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                AbstractRoleModel<Role> mySocialModel = SocialRoleModel.Create(role.Role);
                if (theRoleService.Edit(GetUserInformatonModel(), mySocialModel, role.SelectedPermissionsIds)) {
                    return RedirectToAction("Index");
                }
            } catch (Exception e) {
                LogError(e, EDIT_ROLE_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(ERROR_MESSAGE);
            }

            try {
                RoleModel myModel = CreateRoleModel(role);
                return View("Edit", myModel);
            } catch (Exception e) {
                LogError(e, CREATE_MODEL_ERROR);
                return SendToErrorPage(ERROR_MESSAGE);
            }            
        }

        public ActionResult Delete(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            if (!PermissionHelper<User>.AllowedToPerformAction(myUserInformation, SocialPermission.Delete_Role)) {
                return SendToErrorPage(PAGE_NOT_FOUND);
            }
            Role role = null;
            try {
                role = theRoleService.FindRole(id);
            } catch (Exception e) {
                LogError(e, GET_ROLE_ERROR);
                return SendToErrorPage(ERROR_MESSAGE);
            }

            if (role == null) {
                return SendToResultPage(ROLE_NOT_FOUND);
            }

            return View("Delete", role);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Role roleToDelete) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                theRoleService.Delete(GetUserInformatonModel(), roleToDelete);
                return RedirectToAction("Index");
            } catch (Exception e) {
                LogError(e, "Error occurred while clicking the submit button when deleting a restrictionModel.");
                return SendToErrorPage("Error while deleting the restrictionModel. Please check the error log.");
            }
        }

        public ActionResult SwitchUserRoles() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            if (!PermissionHelper<User>.AllowedToPerformAction(myUserInformation, SocialPermission.Switch_Users_Role)) {
                return SendToErrorPage(PAGE_NOT_FOUND);
            }
            List<Role> myRoles = new List<Role>();
            try {
                myRoles = theRoleService.GetAllRoles().ToList<Role>();
            } catch (Exception e) {
                LogError(e, "Error getting the list of roles.");
                return SendToErrorPage("Error getting the list of roles. Please check the error log.");
            }
            SwitchUserRoles mySwitchRoles = new SwitchUserRoles.Builder()
                .Roles(myRoles)
                .SelectedCurrentRoleId(0)
                .SelectedMoveToRoleId(0)
                .Build();

            return View("SwitchUserRoles", mySwitchRoles);
        }

        [ActionName("SwitchUserRoles")]
        [AcceptParameter(Name = "button", Value = "Get users for this role")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SwitchUserRoles(int CurrentRoleId, int MoveToRoleId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            if (!PermissionHelper<User>.AllowedToPerformAction(myUserInformation, SocialPermission.Switch_Users_Role)) {
                return SendToErrorPage(PAGE_NOT_FOUND);
            }
            SwitchUserRoles mySwitchRoles = new SwitchUserRoles.Builder().Build();
            try {
                mySwitchRoles = CreateSwitchUserRolesModel(null, CurrentRoleId, MoveToRoleId);
            } catch (Exception e) {
                LogError(e, "Error creating the SwitchUserRolesModel.");
                return SendToErrorPage("Error getting the content to load on the page. Please check the error log.");
            }
            return View("SwitchUserRoles", mySwitchRoles);
        }

        [ActionName("SwitchUserRoles")]
        [AcceptParameter(Name = "button", Value = "Move users to this role")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SwitchUserRoles(int[] SelectedUserIds, int CurrentRoleId, int MoveToRoleId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            if (!PermissionHelper<User>.AllowedToPerformAction(myUserInformation, SocialPermission.Switch_Users_Role)) {
                return SendToErrorPage(PAGE_NOT_FOUND);
            }
            List<int> mySelectedUsers = SelectedUserIds == null ? new List<int>() : SelectedUserIds.ToList<int>();
            try {
                if (theRoleService.MoveUsersToRole(mySelectedUsers, CurrentRoleId, MoveToRoleId)) {
                    return SendToResultPage("Users moved!");
                }
            } catch (Exception e) {
                LogError(e, new StringBuilder().AppendFormat("Error moving the users to a new role. "
                    + "[selectedUserIds={0};currentRoldId={1};moveToRoleId={2}]", 
                    SelectedUserIds, CurrentRoleId, MoveToRoleId).ToString());
                return SendToErrorPage("Error getting the content to load on the page. Please check the error log.");
            }

            SwitchUserRoles mySwitchRoles = new SwitchUserRoles.Builder().Build();
            try {
                mySwitchRoles = CreateSwitchUserRolesModel(SelectedUserIds, CurrentRoleId, MoveToRoleId);
            } catch (Exception e) {
                LogError(e, "Error creating the SwitchUserRolesModel.");
                return SendToErrorPage("Error getting the content to load on the page. Please check the error log.");
            }
            return View("SwitchUserRoles", mySwitchRoles);
        }

        private RoleModel CreateRoleModel(Role aModel) {
            RoleModel myModel = new RoleModel(aModel);
            List<Permission> myPermissions = thePermissionService.GetAllPermissions().ToList<Permission>();
            if (myPermissions.Count == 0) {
                ViewData["PermissionMessage"] = MessageHelper.NormalMessage("There are currently no permissions created, please create a permission first.");
            }
            myModel.AllPermissions = myPermissions;

            return myModel;
        }

        private RoleModel CreateRoleModel(RoleModel aModel) {
            RoleModel myModel = CreateRoleModel(aModel.Role);
            myModel.SelectedPermissionsIds = aModel.SelectedPermissionsIds;
            return myModel;
        }


        private SwitchUserRoles CreateSwitchUserRolesModel(int[] SelectedUserIds, int aCurrentRole, int aMoveToRole) {
            List<int> mySelectedUsers = SelectedUserIds == null ? new List<int>() : SelectedUserIds.ToList<int>();
            IEnumerable<Role> myRoles = theRoleService.GetAllRoles();
            List<Pair<User, bool>> myUsers = UserSelection(mySelectedUsers, aCurrentRole);

            return new SwitchUserRoles.Builder()
                .Roles(myRoles)
                .Users(myUsers)
                .SelectedCurrentRoleId(aCurrentRole)
                .SelectedMoveToRoleId(aMoveToRole)
                .Build();
        }

        private List<Pair<User, bool>> UserSelection(List<int> aSelectedUsers, int aRoleId) {
            List<User> myUsers = theRoleService.UsersInRole(aRoleId).ToList<User>();
            return SelectionHelper.UserSelection(aSelectedUsers, myUsers);
        }
    }
}
