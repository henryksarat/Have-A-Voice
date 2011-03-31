using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BaseWebsite.Models;
using Social.Admin;
using Social.Admin.Exceptions;
using Social.Admin.Helpers;
using Social.Admin.Repositories;
using Social.Admin.Services;
using Social.Authentication;
using Social.Authentication.Services;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Users.Services;
using Social.Validation;
using Social.Generic;
using BaseWebsite.Helpers;
using System.Text;

namespace BaseWebsite.Controllers.Admin {
    //T = User
    //U = Role
    //V = Permission
    //W = UserRole
    //X = PrivacySetting
    //Y = RolePermission
    //Z = WhoIsOnline
    public abstract class AbstractRoleController<T, U, V, W, X, Y, Z> : BaseController<T, U, V, W, X, Y, Z> {
        private IRoleService<T, U, Y> theRoleService;
        private IPermissionService<T, V> thePermissionService;
        private ModelStateWrapper theModelState;

        public AbstractRoleController(IBaseService<T> aBaseService, IUserInformation<T, Z> aUserInformation, IAuthenticationService<T, U, V, W, X, Y> anAuthService, 
                                      IWhoIsOnlineService<T, Z> aWhoIsOnlineService, IRoleRepository<T, U, Y> aRoleRepository, IPermissionRepository<T, V> myPermissionRepository) : 
            base(aBaseService, aUserInformation, anAuthService, aWhoIsOnlineService) {
                theModelState = new ModelStateWrapper(this.ModelState);
                theRoleService = new RoleService<T, U, Y>(theModelState, aRoleRepository);
                thePermissionService = new PermissionService<T, V>(theModelState, myPermissionRepository);
        }

        protected abstract U CreateEmptyRole();
        protected abstract AbstractRoleModel<U> CreateSocialRoleModel(U aRole);
        protected abstract AbstractRoleViewModel<U, V> CreateRoleViewModel(U aRole, IEnumerable<V> aPermissions);
        protected abstract SelectList GetCurrentRoles(IEnumerable<U> aRoles, int aSelectedCurrentRoleId);
        protected abstract SelectList GetMoveToRoles(IEnumerable<U> aRoles, int aSelectedMoveToRoleId);

        public ActionResult Index() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                IEnumerable<U> myRoles = theRoleService.GetAllRoles(GetUserInformatonModel()).ToList<U>();

                if (myRoles.Count() == 0) {
                    ViewData["Message"] = NormalMessage(RoleKeys.NO_ROLES);
                }

                return View("Index", myRoles);
            } catch (PermissionDenied anException) {
                return SendToErrorPage(anException.Message);
            } catch (Exception e) {
                LogError(e, RoleKeys.GET_ALL_ROLES_ERROR);
                return SendToErrorPage(RoleKeys.GET_ALL_ROLES_ERROR);
            }
        }

        protected ActionResult Create() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            if(!PermissionHelper<T>.AllowedToPerformAction(GetUserInformatonModel(), SocialPermission.Create_Role)) {
                return SendToErrorPage(ErrorKeys.PERMISSION_DENIED);
            }

            try {
                U myRole = CreateEmptyRole();
                  return View("Create", CreateRoleModel(myRole));
            } catch (Exception e) {
                LogError(e, RoleKeys.CREATE_MODEL_ERROR);
                return SendToErrorPage(ErrorKeys.ERROR_MESSAGE);
            }
        }

        protected ActionResult Create(AbstractRoleViewModel<U, V> model) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                AbstractRoleModel<U> mySocialModel = CreateSocialRoleModel(model.Role);
                if (theRoleService.Create(GetUserInformatonModel(), mySocialModel, model.SelectedPermissionsIds)) {
                    return RedirectToAction("Index");
                }
            } catch (PermissionDenied anException) {
                return SendToErrorPage(anException.Message);
            } catch (Exception e) {
                LogError(e, RoleKeys.CREATE_ROLE_ERROR);
                ViewData["Message"] = ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            }

            try {
                AbstractRoleViewModel<U, V> myModel = CreateRoleModel(model);
                return View("Create", myModel);
            } catch (Exception e) {
                LogError(e, RoleKeys.CREATE_MODEL_ERROR);
                return SendToErrorPage(ErrorKeys.ERROR_MESSAGE);
            }
        }

        protected ActionResult Edit(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            U role = default(U);
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
                AbstractRoleViewModel<U, V> myModel = CreateRoleModel(role);
                return View("Edit", myModel);
            } catch (Exception e) {
                LogError(e, RoleKeys.CREATE_MODEL_ERROR);
                return SendToErrorPage(ErrorKeys.ERROR_MESSAGE);
            }
        }

        protected ActionResult Edit(AbstractRoleViewModel<U, V> role) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                AbstractRoleModel<U> mySocialModel = CreateSocialRoleModel(role.Role);
                if (theRoleService.Edit(GetUserInformatonModel(), mySocialModel, role.SelectedPermissionsIds)) {
                    TempData["Message"] = SuccessMessage(RoleKeys.EDIT_SUCCESS); 
                    return RedirectToAction("Index");
                }
            } catch (PermissionDenied anException) {
                return SendToErrorPage(anException.Message);
            } catch (Exception e) {
                LogError(e, RoleKeys.EDIT_ROLE_ERROR);
                ViewData["Message"] = ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            }

            try {
                AbstractRoleViewModel<U, V> myModel = CreateRoleModel(role);
                return View("Edit", myModel);
            } catch (Exception e) {
                LogError(e, RoleKeys.CREATE_MODEL_ERROR);
                return SendToErrorPage(ErrorKeys.ERROR_MESSAGE);
            }            
        }

        protected ActionResult Delete(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                U myRole = theRoleService.FindRole(GetUserInformatonModel(), id, SocialPermission.Delete_Role);

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

        protected ActionResult Delete(U roleToDelete) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                theRoleService.Delete(GetUserInformatonModel(), roleToDelete);
                TempData["Message"] = SuccessMessage(RoleKeys.DELETE_SUCCESS); 
                return RedirectToAction("Index");
            } catch (PermissionDenied anException) {
                return SendToErrorPage(anException.Message);
            } catch (Exception e) {
                LogError(e, RoleKeys.DELETE_ERROR);
                return SendToErrorPage(ErrorKeys.ERROR_MESSAGE);
            }
        }

        protected ActionResult SwitchUserRoles() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<T> myUserInformation = GetUserInformatonModel();
            if (!PermissionHelper<T>.AllowedToPerformAction(myUserInformation, SocialPermission.Switch_Users_Role)) {
                return SendToErrorPage(ErrorKeys.PERMISSION_DENIED);
            }
            List<U> myRoles = new List<U>();
            try {
                myRoles = theRoleService.GetAllRoles(GetUserInformatonModel()).ToList<U>();

                SwitchUserRoles<T, U> mySwitchRoles = new SwitchUserRoles<T, U>() {
                    MoveToRoles = GetCurrentRoles(myRoles, 0),
                    CurrentRoles = GetMoveToRoles(myRoles, 0)
                };

                return View("SwitchUserRoles", mySwitchRoles);
            } catch (Exception e) {
                LogError(e, "Error getting the list of roles.");
                return SendToErrorPage("Error getting the list of roles. Please check the error log.");
            }
        }

        protected ActionResult SwitchUserRoles(int CurrentRoleId, int MoveToRoleId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<T> myUserInformation = GetUserInformatonModel();
            if (!PermissionHelper<T>.AllowedToPerformAction(myUserInformation, SocialPermission.Switch_Users_Role)) {
                return SendToErrorPage(ErrorKeys.PERMISSION_DENIED);
            }
            try {
                SwitchUserRoles<T, U> mySwitchRoles = CreateSwitchUserRolesModel(null, CurrentRoleId, MoveToRoleId);
                return View("SwitchUserRoles", mySwitchRoles);
            } catch (Exception e) {
                LogError(e, "Error creating the SwitchUserRolesModel.");
                return SendToErrorPage("Error getting the content to load on the page. Please check the error log.");
            }
        }

        protected ActionResult SwitchUserRoles(int[] SelectedUserIds, int CurrentRoleId, int MoveToRoleId) {
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
                SwitchUserRoles<T, U> mySwitchRoles = CreateSwitchUserRolesModel(SelectedUserIds, CurrentRoleId, MoveToRoleId);
                return View("SwitchUserRoles", mySwitchRoles);
            } catch (Exception e) {
                LogError(e, "Error creating the SwitchUserRolesModel.");
                return SendToErrorPage("Error getting the content to load on the page. Please check the error log.");
            }
        }

        private SwitchUserRoles<T, U> CreateSwitchUserRolesModel(int[] SelectedUserIds, int aCurrentRole, int aMoveToRole) {
            List<int> mySelectedUsers = SelectedUserIds == null ? new List<int>() : SelectedUserIds.ToList<int>();
            IEnumerable<U> myRoles = theRoleService.GetAllRoles(GetUserInformatonModel());
            List<Pair<T, bool>> myUsers = UserSelection(mySelectedUsers, aCurrentRole);

            return new SwitchUserRoles<T, U>() {
                Users = myUsers,
                SelectedCurrentRoleId = aCurrentRole,
                SelectedMoveToRoleId = aMoveToRole,
                MoveToRoles = GetCurrentRoles(myRoles, aMoveToRole),
                CurrentRoles = GetMoveToRoles(myRoles, aCurrentRole)
            };
        }

        private List<Pair<T, bool>> UserSelection(List<int> aSelectedUsers, int aRoleId) {
            List<AbstractUserModel<T>> myUsers = theRoleService.UsersInRole(aRoleId).Select(u => CreateSocialUserModel(u)).ToList<AbstractUserModel<T>>();
            return SelectionHelper<T>.UserSelection(aSelectedUsers, myUsers);
        }


        private AbstractRoleViewModel<U, V> CreateRoleModel(AbstractRoleViewModel<U, V> aModel) {
            AbstractRoleViewModel<U, V> myModel = CreateRoleModel(aModel.Role);
            myModel.SelectedPermissionsIds = aModel.SelectedPermissionsIds;
            return myModel;
        }

        private AbstractRoleViewModel<U, V> CreateRoleModel(U aModel) {
            List<V> myPermissions = thePermissionService.GetAllPermissions(GetUserInformatonModel()).ToList<V>();
            if (myPermissions.Count == 0) {
                ViewData["PermissionMessage"] = NormalMessage("There are currently no permissions created, please create a permission first.");
            }

            return CreateRoleViewModel(aModel, myPermissions);
        }
    }
}
