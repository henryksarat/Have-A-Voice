using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Models;
using HaveAVoice.Services;
using HaveAVoice.Validation;
using HaveAVoice.Repositories;
using HaveAVoice.Helpers;
using HaveAVoice.Models.View;
using HaveAVoice.Services.AdminFeatures;

namespace HaveAVoice.Controllers.Admin {
    public class PermissionController : AdminBaseController {
        private static string PAGE_NOT_FOUND = "You do not have access.";
        private IHAVRoleService theService;

         public PermissionController() 
            : base(new HAVBaseService(new HAVBaseRepository())) {
            theService = new HAVRoleService(new ModelStateWrapper(this.ModelState));
        }

         public PermissionController(IHAVRoleService service, IHAVBaseService baseService)
            : base(baseService) {
            theService = service;
        }

        public ActionResult Index() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            if (!HAVPermissionHelper.AllowedToPerformAction(GetUserInformatonModel(), HAVPermission.View_Permissions)) {
                return SendToErrorPage(PAGE_NOT_FOUND);
            }

             IEnumerable<Permission> permissions = null;
             try {
                 permissions = theService.GetAllPermissions();
             } catch (Exception e) {
                 LogError(e, "Unable to get all myPermissions.");
                 return SendToErrorPage("Unable to get all myPermissions.");
             }

             if (permissions.Count() == 0) {
                 ViewData["Message"] = "There are no myPermissions to display.";
             }

             return View("Index", permissions);
        }

        public ActionResult Create() {
            UserInformationModel myUserInformation = GetUserInformatonModel();
            if (!HAVPermissionHelper.AllowedToPerformAction(myUserInformation, HAVPermission.Create_Permission)) {
                return SendToErrorPage(PAGE_NOT_FOUND);
            }
            return View("Create");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(PermissionModel model) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                if (theService.CreatePermission(GetUserInformatonModel(), model.Permission)) {
                    return RedirectToAction("Index");
                }
            } catch (Exception e) {
                LogError(e, "Unable to create the restrictionModel.");
                ViewData["Message"] = "Error creating the restrictionModel. Check the error log and try again.";
            }

            return View("Create", model);
        }

        public ActionResult Edit(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel myUserInformation = GetUserInformatonModel();
            if (!HAVPermissionHelper.AllowedToPerformAction(myUserInformation, HAVPermission.Edit_Permission)) {
                return SendToErrorPage(PAGE_NOT_FOUND);
            }
            Permission permission = null;
            try {
                permission = theService.GetPermission(id);
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
                if (theService.EditPermission(GetUserInformatonModel(), permission)) {
                    return RedirectToAction("Index");
                }
            } catch (Exception e) {
                LogError(e, "Unable to edit the restrictionModel.");
                ViewData["Message"] = "Error editing the restrictionModel. Check the error log and try again.";
            }
            return View("Edit", permission);
        }

        public ActionResult Delete(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel myUserInformation = GetUserInformatonModel();
            if (!HAVPermissionHelper.AllowedToPerformAction(myUserInformation, HAVPermission.Delete_Permission)) {
                return SendToErrorPage(PAGE_NOT_FOUND);
            }
            Permission permission = null;
            try {
                permission = theService.GetPermission(id);
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
                theService.DeletePermission(GetUserInformatonModel(), permission);
                return RedirectToAction("Index");
            } catch (Exception e) {
                LogError(e, "Error occurred while clicking the submit button when deleting a restrictionModel.");
                return SendToErrorPage("Error while deleting the restrictionModel. Please check the error log.");
            }
        }

        protected override ActionResult SendToResultPage(string title, string details) {
            return SendToResultPage(SiteSectionsEnum.Permission, title, details);
        }
    }
}
