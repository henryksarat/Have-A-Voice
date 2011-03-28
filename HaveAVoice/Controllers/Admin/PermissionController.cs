using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Models.SocialWrappers;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.AdminFeatures;
using HaveAVoice.Services;
using Social.Admin;
using Social.Admin.Helpers;
using Social.Generic.ActionFilters;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;
using Social.Generic.Services;

namespace HaveAVoice.Controllers.Admin {
    public class PermissionController : AdminBaseController {
        private static string PAGE_NOT_FOUND = "You do not have access.";
        private IPermissionService<User, Permission> thePermissionService;

         public PermissionController() 
            : base(new BaseService<User>(new HAVBaseRepository())) {
                thePermissionService = new PermissionService<User, Permission>(
                    new ModelStateWrapper(this.ModelState),
                    new EntityHAVPermissionRepository());
        }

         public PermissionController(IBaseService<User> myBaseService, IPermissionService<User, Permission> myPermissionService)
            : base(myBaseService) {
            thePermissionService = myPermissionService;
        }

        public ActionResult Index() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            if (!PermissionHelper<User>.AllowedToPerformAction(GetUserInformatonModel(), SocialPermission.View_Permissions)) {
                return SendToErrorPage(PAGE_NOT_FOUND);
            }

             IEnumerable<Permission> permissions = null;
             try {
                 permissions = thePermissionService.GetAllPermissions();
             } catch (Exception e) {
                 LogError(e, "Unable to get all permissions.");
                 return SendToErrorPage("Unable to get all myPermissions.");
             }

             if (permissions.Count() == 0) {
                 ViewData["Message"] = "There are no permissions to display.";
             }

             return View("Index", permissions);
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Create() {
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            if (!PermissionHelper<User>.AllowedToPerformAction(myUserInformation, SocialPermission.Create_Permission)) {
                return SendToErrorPage(PAGE_NOT_FOUND);
            }
            return View("Create");
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(SocialPermissionModel model) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                if (thePermissionService.Create(GetUserInformatonModel(), model)) {
                    return RedirectToAction("Index");
                }
            } catch (Exception e) {
                LogError(e, "Unable to create the restrictionModel.");
                ViewData["Message"] = MessageHelper.ErrorMessage("Error creating the restrictionModel. Check the error log and try again.");
            }

            return RedirectToAction("Create");
        }

        public ActionResult Edit(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            if (!PermissionHelper<User>.AllowedToPerformAction(myUserInformation, SocialPermission.Edit_Permission)) {
                return SendToErrorPage(PAGE_NOT_FOUND);
            }
            Permission permission = null;
            try {
                permission = thePermissionService.FindPermission(id);
            } catch (Exception e) {
                LogError(e, "Unable to get the restrictionModel to edit.");
                return SendToErrorPage("Unable to get restrictionModel to edit.");
            }

            if (permission == null) {
                return SendToResultPage("Permission not found.");
            }

            return View("Edit", permission);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Permission permission) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            
            try {
                AbstractPermissionModel<Permission> myPermissionWrapper = SocialPermissionModel.Create(permission);
                if (thePermissionService.Edit(GetUserInformatonModel(), myPermissionWrapper)) {
                    return RedirectToAction("Index");
                }
            } catch (Exception e) {
                LogError(e, "Unable to edit the restrictionModel.");
                ViewData["Message"] = MessageHelper.ErrorMessage("Error editing the restrictionModel. Check the error log and try again.");
            }
            return View("Edit", permission);
        }

        public ActionResult Delete(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInformation = GetUserInformatonModel();
            if (!PermissionHelper<User>.AllowedToPerformAction(myUserInformation, SocialPermission.Delete_Permission)) {
                return SendToErrorPage(PAGE_NOT_FOUND);
            }
            Permission permission = null;
            try {
                permission = thePermissionService.FindPermission(id);
            } catch (Exception e) {
                LogError(e, "Unable to get the restrictionModel to delete.");
                return SendToErrorPage("Unable to get restrictionModel to delete.");
            }

            if (permission == null) {
                return SendToResultPage("Permission not found.");
            }

            return View("Delete", permission);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Permission permission) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                thePermissionService.Delete(GetUserInformatonModel(), permission);
                return RedirectToAction("Index");
            } catch (Exception e) {
                LogError(e, "Error occurred while clicking the submit button when deleting a restrictionModel.");
                return SendToErrorPage("Error while deleting the restrictionModel. Please check the error log.");
            }
        }
    }
}
