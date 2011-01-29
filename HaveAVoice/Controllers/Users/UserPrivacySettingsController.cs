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
using HaveAVoice.Models.DataStructures;

namespace HaveAVoice.Controllers.Users
{
    public class UserPrivacySettingsController : HAVBaseController {
        private static string EDIT_SUCCESS = "Your account has been edited successfully!";
        private const string RETRIEVE_FAIL = "Error retreiving your privacy settings. Please try again.";
        private const string UPDATE_FAIL = "Error updating privacy settings. Please try again.";

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
            Dictionary<string, bool> myPrivacySettingSelections = new Dictionary<string, bool>();
            try {
                myPrivacySettingSelections = thePrivacyService.GetPrivacySettingsForEdit(myUser);
            } catch (Exception e) {
                LogError(e, RETRIEVE_FAIL);
                return SendToErrorPage(RETRIEVE_FAIL);
            }

            return View("Edit", myPrivacySettingSelections);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(IEnumerable<Pair<PrivacySetting>> aSettings) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            try {
                thePrivacyService.UpdatePrivacySettings(myUser, aSettings);
                return SendToResultPage(EDIT_SUCCESS);
            } catch (Exception e) {
                LogError(e, UPDATE_FAIL);
                ViewData["Message"] = MessageHelper.ErrorMessage(UPDATE_FAIL);

            }
            return View("Edit", aSettings);
        }
    }
}
