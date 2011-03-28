using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Social.Generic;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Admin.Services;
using Social.Admin;
using Social.Validation;

namespace BaseWebsite.Controllers.Admin {
    //T = User
    //U = Role
    //V = Permission
    //X = RolePermission
    public class RoleController<T, U, V, X> : AdminBaseController<T> {
        /*
        private IRoleService<T, U, X> theRoleService;
        private IPermissionService<T, V> thePermissionService;
        private ModelStateWrapper theModelState;

        public RoleController(IBaseService<T> aBaseService, IRoleService<T, U, X> myRoleService, IPermissionService<T, V> myPermissionService) : base(aBaseService) {
            theRoleService = myRoleService;
            thePermissionService = myPermissionService;
        }

        public ActionResult Index() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                IEnumerable<Role> myRoles = theRoleService.GetAllRoles(GetUserInformatonModel()).ToList<Role>();

                if (myRoles.Count() == 0) {
                    ViewData["Message"] = MessageHelper.NormalMessage(RoleKeys.NO_ROLES);
                }

                return View("Index", myRoles);
            } catch (PermissionDenied anException) {
                return SendToErrorPage(anException.Message);
            } catch (Exception e) {
                LogError(e, RoleKeys.GET_ALL_ROLES_ERROR);
                return SendToErrorPage(RoleKeys.GET_ALL_ROLES_ERROR);
            }
        }

        public ActionResult Create() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            if(!PermissionHelper<User>.AllowedToPerformAction(GetUserInformatonModel(), SocialPermission.Create_Role)) {
                return SendToErrorPage(ErrorKeys.PERMISSION_DENIED);
            }

            try {
                Role myRole = Role.CreateRole(0, string.Empty, string.Empty, false, false);
                RoleModel myModel = CreateRoleModel(myRole);

                return View("Create", myModel);
            } catch (Exception e) {
                LogError(e, RoleKeys.CREATE_MODEL_ERROR);
                return SendToErrorPage(ErrorKeys.ERROR_MESSAGE);
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
            } catch (PermissionDenied anException) {
                return SendToErrorPage(anException.Message);
            } catch (Exception e) {
                LogError(e, RoleKeys.CREATE_ROLE_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            }

            try {
                RoleModel myModel = CreateRoleModel(model);
                return View("Create", myModel);
            } catch (Exception e) {
                LogError(e, RoleKeys.CREATE_MODEL_ERROR);
                return SendToErrorPage(ErrorKeys.ERROR_MESSAGE);
            }
        }

        public ActionResult Edit(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            Role role = null;
            try {
                role = theRoleService.FindRole(GetUserInformatonModel(), id, SocialPermission.Edit_Role);
            } catch (PermissionDenied anException) {
                return SendToErrorPage(anException.Message);
            } catch (Exception e) {
                LogError(e, RoleKeys.GET_ROLE_ERROR);
                return SendToErrorPage(ErrorKeys.ERROR_MESSAGE);
            }

            if (role == null) {
                return SendToResultPage(RoleKeys.ROLE_NOT_FOUND);
            }
            
            try {
                RoleModel myModel = CreateRoleModel(role);
                return View("Edit", myModel);
            } catch (Exception e) {
                LogError(e, RoleKeys.CREATE_MODEL_ERROR);
                return SendToErrorPage(ErrorKeys.ERROR_MESSAGE);
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
                    TempData["Message"] = MessageHelper.SuccessMessage(RoleKeys.EDIT_SUCCESS); 
                    return RedirectToAction("Index");
                }
            } catch (PermissionDenied anException) {
                return SendToErrorPage(anException.Message);
            } catch (Exception e) {
                LogError(e, RoleKeys.EDIT_ROLE_ERROR);
                ViewData["Message"] = MessageHelper.ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            }

            try {
                RoleModel myModel = CreateRoleModel(role);
                return View("Edit", myModel);
            } catch (Exception e) {
                LogError(e, RoleKeys.CREATE_MODEL_ERROR);
                return SendToErrorPage(ErrorKeys.ERROR_MESSAGE);
            }            
        }

        public ActionResult Delete(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                Role myRole = theRoleService.FindRole(GetUserInformatonModel(), id, SocialPermission.Delete_Role);

                if (myRole == null) {
                    return SendToResultPage(RoleKeys.ROLE_NOT_FOUND);
                }

                return View("Delete", myRole);
            } catch (PermissionDenied anException) {
                return SendToErrorPage(anException.Message);
            } catch (Exception e) {
                LogError(e, RoleKeys.GET_ROLE_ERROR);
                return SendToErrorPage(ErrorKeys.ERROR_MESSAGE);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Role roleToDelete) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                theRoleService.Delete(GetUserInformatonModel(), roleToDelete);
                TempData["Message"] = MessageHelper.SuccessMessage(RoleKeys.DELETE_SUCCESS); 
                return RedirectToAction("Index");
            } catch (PermissionDenied anException) {
                return SendToErrorPage(anException.Message);
            } catch (Exception e) {
                LogError(e, RoleKeys.DELETE_ERROR);
                return SendToErrorPage(ErrorKeys.ERROR_MESSAGE);
            }
        }

        public ActionResult SwitchUserRoles() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            if (!PermissionHelper<User>.AllowedToPerformAction(myUserInformation, SocialPermission.Switch_Users_Role)) {
                return SendToErrorPage(ErrorKeys.PERMISSION_DENIED);
            }
            List<Role> myRoles = new List<Role>();
            try {
                myRoles = theRoleService.GetAllRoles(GetUserInformatonModel()).ToList<Role>();

                SwitchUserRoles mySwitchRoles = new SwitchUserRoles.Builder()
                                                    .Roles(myRoles)
                                                    .SelectedCurrentRoleId(0)
                                                    .SelectedMoveToRoleId(0)
                                                    .Build();

                return View("SwitchUserRoles", mySwitchRoles);
            } catch (Exception e) {
                LogError(e, "Error getting the list of roles.");
                return SendToErrorPage("Error getting the list of roles. Please check the error log.");
            }
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
                return SendToErrorPage(ErrorKeys.PERMISSION_DENIED);
            }
            try {
                SwitchUserRoles mySwitchRoles = CreateSwitchUserRolesModel(null, CurrentRoleId, MoveToRoleId);
                return View("SwitchUserRoles", mySwitchRoles);
            } catch (Exception e) {
                LogError(e, "Error creating the SwitchUserRolesModel.");
                return SendToErrorPage("Error getting the content to load on the page. Please check the error log.");
            }
        }

        [ActionName("SwitchUserRoles")]
        [AcceptParameter(Name = "button", Value = "Move users to this role")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SwitchUserRoles(int[] SelectedUserIds, int CurrentRoleId, int MoveToRoleId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            List<int> mySelectedUsers = SelectedUserIds == null ? new List<int>() : SelectedUserIds.ToList<int>();
            try {
                if (theRoleService.MoveUsersToRole(GetUserInformatonModel(), mySelectedUsers, CurrentRoleId, MoveToRoleId)) {
                    return SendToResultPage("Users moved!");
                }
            } catch (Exception e) {
                LogError(e, new StringBuilder().AppendFormat("Error moving the users to a new role. "
                    + "[selectedUserIds={0};currentRoldId={1};moveToRoleId={2}]", 
                    SelectedUserIds, CurrentRoleId, MoveToRoleId).ToString());
                return SendToErrorPage("Error getting the content to load on the page. Please check the error log.");
            }

            try {
                SwitchUserRoles mySwitchRoles = CreateSwitchUserRolesModel(SelectedUserIds, CurrentRoleId, MoveToRoleId);
                return View("SwitchUserRoles", mySwitchRoles);
            } catch (Exception e) {
                LogError(e, "Error creating the SwitchUserRolesModel.");
                return SendToErrorPage("Error getting the content to load on the page. Please check the error log.");
            }
        }

        private RoleModel CreateRoleModel(RoleModel aModel) {
            RoleModel myModel = CreateRoleModel(aModel.Role);
            myModel.SelectedPermissionsIds = aModel.SelectedPermissionsIds;
            return myModel;
        }

        private RoleModel CreateRoleModel(Role aModel) {
            List<Permission> myPermissions = thePermissionService.GetAllPermissions().ToList<Permission>();
            if (myPermissions.Count == 0) {
                ViewData["PermissionMessage"] = MessageHelper.NormalMessage("There are currently no permissions created, please create a permission first.");
            }

            RoleModel myModel = new RoleModel(aModel) {
                AllPermissions = myPermissions
            };

            return myModel;
        }

        private SwitchUserRoles CreateSwitchUserRolesModel(int[] SelectedUserIds, int aCurrentRole, int aMoveToRole) {
            List<int> mySelectedUsers = SelectedUserIds == null ? new List<int>() : SelectedUserIds.ToList<int>();
            IEnumerable<Role> myRoles = theRoleService.GetAllRoles(GetUserInformatonModel());
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
        }*/
    }
}
