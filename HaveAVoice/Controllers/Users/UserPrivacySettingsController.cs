using System;
using System.Web.Mvc;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services;
using Social.User.Models;
using Social.User.Services;
using Social.Generic.Services;

namespace HaveAVoice.Controllers.Users
{
    public class UserPrivacySettingsController : HAVBaseController {
        private static string EDIT_SUCCESS = "Your account has been edited successfully!";
        private const string RETRIEVE_FAIL = "Error retreiving your privacy settings. Please try again.";
        private const string EDIT_FAIL = "Error updating privacy settings. Please make your selection again and click submit.";

        private const string EDIT_VIEW = "Edit";

        private IUserPrivacySettingsService<User, PrivacySetting> thePrivacyService;

        public UserPrivacySettingsController() : 
            base(new BaseService<User>(new HAVBaseRepository())) {
                thePrivacyService = new UserPrivacySettingsService<User, PrivacySetting>(new EntityHAVUserPrivacySettingsRepository());
        }

        public UserPrivacySettingsController(IBaseService<User> aBaseService, IUserPrivacySettingsService<User, PrivacySetting> aPrivacyService)
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
                myPrivacySettingSelections.PrivacySettings = thePrivacyService.GetPrivacySettingsGrouped(myUser);
            } catch (Exception e) {
                LogError(e, RETRIEVE_FAIL);
                return SendToErrorPage(RETRIEVE_FAIL);
            }

            return View("Edit", myPrivacySettingSelections);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(UpdatePrivacySettingsModel<PrivacySetting> aSettings) {
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
