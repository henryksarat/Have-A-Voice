using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Services;
using HaveAVoice.Validation;
using HaveAVoice.Services.AdminFeatures;


namespace HaveAVoice.Controllers.Admin {
    public class RestrictionController : AdminBaseController {
        private static string PAGE_NOT_FOUND = "You do not have access.";

        private IHAVRestrictionService theService;
        private IHAVRoleService theRoleService;


        public RestrictionController() 
            : base(new HAVBaseService(new HAVBaseRepository())) {
            theService = new HAVRestrictionService(new ModelStateWrapper(this.ModelState));
            theRoleService = new HAVRoleService(new ModelStateWrapper(this.ModelState));
        }

        public RestrictionController(IHAVRestrictionService service, IHAVBaseService baseService, IHAVRoleService roleService)
            : base(baseService) {
            theService = service;
            theRoleService = roleService;
        }

        public ActionResult Index() {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }
            if (!HAVPermissionHelper.AllowedToPerformAction(GetUserInformatonModel(), HAVPermission.View_Restrictions)) {
                return SendToErrorPage(PAGE_NOT_FOUND);
            }

            IEnumerable<Restriction> restrictions = null;

            try {
                restrictions = theService.GetAllRestrictions().ToList<Restriction>();

            } catch (Exception e) {
                LogError(e, "Unable to get all myUsers.");
                return SendToErrorPage("Unable to get all myUsers.");
            }
            if (restrictions.Count() == 0) {
                ViewData["Message"] = "There are no myUsers to display.";
            }

            return View("Index", restrictions);
        }

        public ActionResult Create() {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }

            if (!HAVPermissionHelper.AllowedToPerformAction(GetUserInformatonModel(), HAVPermission.Create_Restriction)) {
                return SendToErrorPage(PAGE_NOT_FOUND);
            }
            
            return View("Create", new RestrictionModel.Builder(0).Build());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(RestrictionModel aRestriction) {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }
            User myUser = GetUserInformaton();
            try {
                if (theService.CreateRestriction(GetUserInformatonModel(), aRestriction.Restriction)) {
                    return RedirectToAction("Index");
                }
            } catch (Exception e) {
                LogError(e, "Unable to create the restrictionModel.");
                ViewData["Message"] = "Error creating the myRestriction. Check the error log and try again.";
            }
            return View("Create", aRestriction);

        }

        public ActionResult Delete(int id) {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }

            if (!HAVPermissionHelper.AllowedToPerformAction(GetUserInformatonModel(), HAVPermission.Delete_Restriction)) {
                return SendToErrorPage(PAGE_NOT_FOUND);
            }

            Restriction restriction = null;
            try {
                restriction = theService.GetRestriction(id);
            } catch (Exception e) {
                LogError(e, "Error getting restrictionModel.");
                return SendToErrorPage("Error getting restrictionModel to delete.");
            }

            if (restriction == null) {
                return SendToResultPage("Restriction not found.");
            }

            return View("Delete", restriction);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Restriction restrictionToDelete) {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }
            try {
                bool myResult = theService.DeleteRestriction(GetUserInformatonModel(), restrictionToDelete);
                if (myResult) {
                    return RedirectToAction("Index");
                }
                return View("Delete", restrictionToDelete);
            } catch (Exception e) {
                LogError(e, "Error occurred while clicking the submit button when deleting a restrictionModel.");
                return SendToErrorPage("Error while deleting the restrictionModel. Please check the error log.");
            }
        }

        public ActionResult Edit(int id) {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }

            if (!HAVPermissionHelper.AllowedToPerformAction(GetUserInformatonModel(), HAVPermission.Edit_Restriction)) {
                return SendToErrorPage(PAGE_NOT_FOUND);
            }

            Restriction restriction = null;
            try {
                restriction = theService.GetRestriction(id);
            } catch (Exception e) {
                LogError(e, "Unable to get the restrictionModel to edit.");
                return SendToErrorPage("Unable to get restrictionModel to edit.");
            }

            if (restriction == null) {
                return SendToResultPage("Restriction not found.");
            }

            return View("Edit", restriction);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Restriction restriction) {
            if (!IsLoggedIn()) {
                return RedirectToAction("Login", "User");
            }

            try {
                if (theService.EditRestriction(GetUserInformatonModel(), restriction)) {
                    return RedirectToAction("Index");
                }
            } catch (Exception e) {
                LogError(e, "Error occurred while clicking the submit button while editing a myRestriction.");
                ViewData["Message"] = "Error updating restrictionModel, please check the error log.";
            }

            return View("Edit", restriction);
        }

        public override ActionResult SendToResultPage(string title, string details) {
            return SendToResultPage(SiteSectionsEnum.Restriction, title, details);
        }
    }
}
