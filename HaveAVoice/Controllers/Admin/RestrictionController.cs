using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Services;
using HaveAVoice.Services.AdminFeatures;
using Social.Admin.Helpers;
using Social.Generic.Helpers;
using Social.Validation;
using Social.Admin.Services;
using HaveAVoice.Repositories.AdminFeatures;

namespace HaveAVoice.Controllers.Admin {
    public class RestrictionController : AdminBaseController {
        private static string PAGE_NOT_FOUND = "You do not have access.";

        private IHAVRestrictionService theService;
        private IRoleService<User, Role> theRoleService;


        public RestrictionController() 
            : base(new HAVBaseService(new HAVBaseRepository())) {
                IValidationDictionary myModelState = new ModelStateWrapper(this.ModelState);
                theService = new HAVRestrictionService(myModelState);
                theRoleService = new RoleService<User, Role>(myModelState, new EntityHAVRoleRepository());
        }

        public RestrictionController(IHAVRestrictionService service, IHAVBaseService baseService, IRoleService<User, Role> roleService)
            : base(baseService) {
            theService = service;
            theRoleService = roleService;
        }

        public ActionResult Index() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            if (!PermissionHelper<User>.AllowedToPerformAction(GetUserInformatonModel(), SocialPermission.View_Restrictions)) {
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
                ViewData["Message"] = MessageHelper.NormalMessage("There are no myUsers to display.");
            }

            return View("Index", restrictions);
        }

        public ActionResult Create() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            if (!PermissionHelper<User>.AllowedToPerformAction(GetUserInformatonModel(), SocialPermission.Create_Restriction)) {
                return SendToErrorPage(PAGE_NOT_FOUND);
            }
            
            return View("Create", new RestrictionModel.Builder(0).Build());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(RestrictionModel aRestriction) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            try {
                if (theService.CreateRestriction(GetUserInformatonModel(), aRestriction.Restriction)) {
                    return RedirectToAction("Index");
                }
            } catch (Exception e) {
                LogError(e, "Unable to create the restrictionModel.");
                ViewData["Message"] = MessageHelper.ErrorMessage("Error creating the myRestriction. Check the error log and try again.");
            }
            return View("Create", aRestriction);

        }

        public ActionResult Delete(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            if (!PermissionHelper<User>.AllowedToPerformAction(GetUserInformatonModel(), SocialPermission.Delete_Restriction)) {
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
                return RedirectToLogin();
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
                return RedirectToLogin();
            }

            if (!PermissionHelper<User>.AllowedToPerformAction(GetUserInformatonModel(), SocialPermission.Edit_Restriction)) {
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
                return RedirectToLogin();
            }

            try {
                if (theService.EditRestriction(GetUserInformatonModel(), restriction)) {
                    return RedirectToAction("Index");
                }
            } catch (Exception e) {
                LogError(e, "Error occurred while clicking the submit button while editing a myRestriction.");
                ViewData["Message"] = MessageHelper.ErrorMessage("Error updating restrictionModel, please check the error log.");
            }

            return View("Edit", restriction);
        }
    }
}
