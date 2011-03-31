using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Social.Admin;
using Social.Admin.Exceptions;
using Social.Admin.Helpers;
using Social.Authentication;
using Social.Authentication.Services;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Users.Services;
using Social.Validation;

namespace BaseWebsite.Controllers.Admin {
    //T = User
    //U = Role
    //V = Permission
    //W = UserRole
    //X = PrivacySetting
    //Y = RolePermission
    //Z = WhoIsOnline
    public abstract class AbstractPermissionController<T, U, V, W, X, Y, Z> : BaseController<T, U, V, W, X, Y, Z> {
        private IPermissionService<T, V> thePermissionService;

        public AbstractPermissionController(IBaseService<T> aBaseService, IUserInformation<T, Z> aUserInformation, IAuthenticationService<T, U, V, W, X, Y> anAuthService, 
                                            IWhoIsOnlineService<T, Z> aWhoIsOnlineService, IPermissionRepository<T, V> aPermissionRepo)
            : base(aBaseService, aUserInformation, anAuthService, aWhoIsOnlineService) {
                thePermissionService = new PermissionService<T, V>(new ModelStateWrapper(this.ModelState), aPermissionRepo);
        }

        protected abstract AbstractPermissionModel<V> CreateSocialPermissionModel(V aPermission);

        protected ActionResult Index() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            if (!PermissionHelper<T>.AllowedToPerformAction(GetUserInformatonModel(), SocialPermission.View_Permissions)) {
                return SendToErrorPage(ErrorKeys.PERMISSION_DENIED);
            }

             try {
                 IEnumerable<V> permissions = thePermissionService.GetAllPermissions(GetUserInformatonModel());

                 if (permissions.Count() == 0) {
                     ViewData["Message"] = "There are no permissions to display.";
                 }

                 return View("Index", permissions);
             } catch (PermissionDenied anException) {
                 return SendToErrorPage(anException.Message);
             } catch (Exception e) {
                 LogError(e, "Unable to get all permissions.");
                 return SendToErrorPage("Unable to get all myPermissions.");
             }
        }

        protected ActionResult Create() {
            UserInformationModel<T> myUserInformation = GetUserInformatonModel();
            if (!PermissionHelper<T>.AllowedToPerformAction(myUserInformation, SocialPermission.Create_Permission)) {
                return SendToErrorPage(ErrorKeys.PERMISSION_DENIED);
            }
            return View("Create");
        }

        protected ActionResult Create(AbstractPermissionModel<V> model) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                if (thePermissionService.Create(GetUserInformatonModel(), model)) {
                    return RedirectToAction("Index");
                }
            } catch (PermissionDenied anException) {
                return SendToErrorPage(anException.Message);
            } catch (Exception e) {
                LogError(e, "Unable to create the restrictionModel.");
                ViewData["Message"] = ErrorMessage("Error creating the restrictionModel. Check the error log and try again.");
            }

            return RedirectToAction("Create");
        }

        protected ActionResult Edit(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<T> myUserInformation = GetUserInformatonModel();
            if (!PermissionHelper<T>.AllowedToPerformAction(myUserInformation, SocialPermission.Edit_Permission)) {
                return SendToErrorPage(ErrorKeys.PERMISSION_DENIED);
            }

            try {
                V permission = thePermissionService.FindPermission(id);

                if (permission == null) {
                    return SendToResultPage("Permission not found.");
                }

                return View("Edit", permission);
            } catch (Exception e) {
                LogError(e, "Unable to get the restrictionModel to edit.");
                return SendToErrorPage("Unable to get restrictionModel to edit.");
            }
        }

        protected ActionResult Edit(V permission) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            
            try {
                AbstractPermissionModel<V> myPermissionWrapper = CreateSocialPermissionModel(permission);
                if (thePermissionService.Edit(GetUserInformatonModel(), myPermissionWrapper)) {
                    return RedirectToAction("Index");
                }
            } catch (PermissionDenied anException) {
                return SendToErrorPage(anException.Message);
            } catch (Exception e) {
                LogError(e, "Unable to edit the restrictionModel.");
                ViewData["Message"] = ErrorMessage("Error editing the restrictionModel. Check the error log and try again.");
            }
            return View("Edit", permission);
        }

        protected ActionResult Delete(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<T> myUserInformation = GetUserInformatonModel();
            if (!PermissionHelper<T>.AllowedToPerformAction(myUserInformation, SocialPermission.Delete_Permission)) {
                return SendToErrorPage(ErrorKeys.PERMISSION_DENIED);
            }
            
            try {
                V permission = thePermissionService.FindPermission(id);

                if (permission == null) {
                    return SendToResultPage("Permission not found.");
                }

                return View("Delete", permission);
            } catch (Exception e) {
                LogError(e, "Unable to get the restrictionModel to delete.");
                return SendToErrorPage("Unable to get restrictionModel to delete.");
            }
        }

        protected ActionResult Delete(V permission) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                thePermissionService.Delete(GetUserInformatonModel(), permission);
                return RedirectToAction("Index");
            } catch (PermissionDenied anException) {
                return SendToErrorPage(anException.Message);
            } catch (Exception e) {
                LogError(e, "Error occurred while clicking the submit button when deleting a restrictionModel.");
                return SendToErrorPage("Error while deleting the restrictionModel. Please check the error log.");
            }
        }
    }
}
