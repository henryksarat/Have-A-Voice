using System;
using System.Web.Mvc;
using Social.Authentication;
using Social.Authentication.Services;
using Social.BaseWebsite.Models;
using Social.Generic.Services;
using Social.User.Models;
using Social.User.Services;
using Social.Users.Services;

namespace BaseWebsite.Controllers.Users {
    //T = User
    //U = Role
    //V = Permission
    //W = UserRole
    //X = PrivacySetting
    //Y = RolePermission
    //Z = WhoIsOnline
    //A = PrivacySetting
    public abstract class AbstractUserPrivacySettingController<T, U, V, W, X, Y, Z, A> : BaseController<T, U, V, W, X, Y, Z> {
        private static string EDIT_SUCCESS = "Your account has been edited successfully!";
        private const string RETRIEVE_FAIL = "Error retreiving your privacy settings. Please try again.";
        private const string EDIT_FAIL = "Error updating privacy settings. Please make your selection again and click submit.";

        private const string EDIT_VIEW = "Edit";

        private IUserPrivacySettingsService<T, A> thePrivacyService;

        public AbstractUserPrivacySettingController(IBaseService<T> aBaseService, 
                                                    IUserInformation<T, Z> aUserInformation, 
                                                    IAuthenticationService<T, U, V, W, X, Y> anAuthService, 
                                                    IWhoIsOnlineService<T, Z> aWhoIsOnlineService,
                                                    IUserPrivacySettingsService<T, A> aPrivacySettingsService)
            : base(aBaseService, aUserInformation, anAuthService, aWhoIsOnlineService) {
                thePrivacyService = aPrivacySettingsService;
        }

        protected abstract ILoggedInModel<DisplayPrivacySettingsModel<A>> CreatedLoggedInModel(T aUser);

        protected ActionResult Edit() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            T myUser = GetUserInformaton();
            DisplayPrivacySettingsModel<A> myPrivacySettingSelections = new DisplayPrivacySettingsModel<A>();
            try {
                myPrivacySettingSelections.PrivacySettings = thePrivacyService.GetPrivacySettingsGrouped(myUser);
            } catch (Exception e) {
                LogError(e, RETRIEVE_FAIL);
                return SendToErrorPage(RETRIEVE_FAIL);
            }

            ILoggedInModel<DisplayPrivacySettingsModel<A>> myLoggedIn = CreatedLoggedInModel(myUser);
            myLoggedIn.Set(myPrivacySettingSelections);

            return View("Edit", myLoggedIn);
        }


        protected ActionResult Edit(UpdatePrivacySettingsModel<A> aSettings) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            T myUser = GetUserInformaton();
            try {
                thePrivacyService.UpdatePrivacySettings(myUser, aSettings);
                TempData["Message"] += SuccessMessage(EDIT_SUCCESS);
            } catch (Exception e) {
                LogError(e, EDIT_FAIL);
                TempData["Message"] += ErrorMessage(EDIT_FAIL);
            }
            return RedirectToAction(EDIT_VIEW);
        }
    }
}