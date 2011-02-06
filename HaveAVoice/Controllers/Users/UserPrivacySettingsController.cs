using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Services;
using HaveAVoice.Repositories;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Models;
using System.Text;
using HaveAVoice.Models.View;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Helpers;

namespace HaveAVoice.Controllers.Users
{
    public class UserPrivacySettingsController : HAVBaseController {
        private static string EDIT_SUCCESS = "Your account has been edited successfully!";
        private const string RETRIEVE_FAIL = "Error retreiving your privacy settings. Please try again.";
        private const string EDIT_FAIL = "Error updating privacy settings. Please make your selection again and click submit.";

        private const string EDIT_VIEW = "Edit";

        private IHAVUserPrivacySettingsService thePrivacyService;

        public UserPrivacySettingsController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
            thePrivacyService = new HAVUserPrivacySettingsService();
        }

        public UserPrivacySettingsController(IHAVBaseService aBaseService, IHAVUserPrivacySettingsService aPrivacyService)
            : base(aBaseService) {
            thePrivacyService = aPrivacyService;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            DisplayPrivacySettingsModel myPrivacySettingSelections = new DisplayPrivacySettingsModel(myUser);
            try {
                myPrivacySettingSelections = thePrivacyService.GetPrivacySettingsForEdit(myUser);
            } catch (Exception e) {
                LogError(e, RETRIEVE_FAIL);
                return SendToErrorPage(RETRIEVE_FAIL);
            }

            return View("Edit", myPrivacySettingSelections);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(UpdatePrivacySettingsModel aSettings) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            try {
                thePrivacyService.UpdatePrivacySettings(myUser, aSettings);
                TempData["Message"] = MessageHelper.SuccessMessage(EDIT_SUCCESS);
            } catch (Exception e) {
                LogError(e, EDIT_FAIL);
                TempData["Message"] = MessageHelper.ErrorMessage(EDIT_FAIL);
            }
            return RedirectToAction(EDIT_VIEW);
        }
    }
}
