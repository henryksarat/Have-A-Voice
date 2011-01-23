﻿using System;
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

namespace HaveAVoice.Controllers.Users
{
    public class UserPrivacySettingsController : HAVBaseController {
        private static string EDIT_SUCCESS = "Your account has been edited successfully!";

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
            UserPrivacySetting myPrivacy;
            try {
                myPrivacy = thePrivacyService.FindUserPrivacySettingsForUser(myUser);
            } catch (Exception e) {
                LogError(e, new StringBuilder().AppendFormat("Error retrieving the user privacy settings. [userId={0}]", myUser.Id).ToString());
                return SendToErrorPage("Error retrieving your privacy settings. Please try again.");
            }

            return View("Edit", myPrivacy);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(UserPrivacySetting aUserPrivacySetting) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            try {
                thePrivacyService.UpdatePrivacySettings(myUser, aUserPrivacySetting);
                return SendToResultPage(EDIT_SUCCESS);
            } catch (Exception e) {
                LogError(e, new StringBuilder().AppendFormat("Unable to update the user privacy settings. [userId={0};userPrivacySettingsId{1}]", myUser.Id, aUserPrivacySetting.Id).ToString());
                ViewData["Message"] = MessageHelper.ErrorMessage("Error updating your privacy settings. Please try again.");

            }
            return View("Edit", aUserPrivacySetting);
        }
    }
}
