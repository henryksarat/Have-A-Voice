using System;
using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers.Users;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.ActionMethods;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Models;
using HaveAVoice.Models.SocialWrappers;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Users.Services;
using Social.Validation;
using Social.BaseWebsite.Models;

namespace HaveAVoice.Controllers.Users {
    public class UserSpecificRegionsController : HAVBaseController {
        private IHAVUserService theService;
        private IValidationDictionary theValidationDictionary;

        public UserSpecificRegionsController() {
            HAVUserInformationFactory.SetInstance(GetUserInformationInstance());
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theService = new HAVUserService(theValidationDictionary);
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Edit() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            } 
            User myUser = GetUserInformaton();

            EditUserSpecificRegionModel myModel = null;
            ILoggedInModel<EditUserSpecificRegionModel> myLoggedInWrapperModel =
                new LoggedInWrapperModel<EditUserSpecificRegionModel>(myUser, SiteSection.MyProfile);
            try {
                myModel = theService.GetUserSpecificRegionForEdit(myUser);
                myLoggedInWrapperModel.Set(myModel);
            } catch (Exception e) {
                LogError(e, "Unable to get the model to edit the user specific regioin");
                SendToErrorPage("Unable to edit your settings. Please try again.");
            }

            return View("Edit", myLoggedInWrapperModel);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Edit(EditUserSpecificRegionModel aModel) {
            try {
                UserInformationModel<User> myUserInfo = GetUserInformatonModel();
                theService.EditUserSpecificRegions(myUserInfo, aModel);
                TempData["Message"] += MessageHelper.SuccessMessage("Settings saved!");
            } catch (Exception exception) {
                LogError(exception, "Error editing the user specific regions.");
                TempData["Message"] += MessageHelper.ErrorMessage("An error has occurred please try your submission again later.");
            }

            return RedirectToAction("Edit");
        }

        protected override AbstractUserModel<User> GetSocialUserInformation() {
            return SocialUserModel.Create(GetUserInformaton());
        }

        protected override AbstractUserModel<User> CreateSocialUserModel(User aUser) {
            return SocialUserModel.Create(aUser);
        }

        protected override IProfilePictureStrategy<User> ProfilePictureStrategy() {
            return new ProfilePictureStrategy();
        }

        protected override string UserEmail() {
            return GetUserInformaton().Email;
        }

        protected override string UserPassword() {
            return GetUserInformaton().Password;
        }

        protected override int UserId() {
            return GetUserInformaton().Id;
        }

        protected override string ErrorMessage(string aMessage) {
            return MessageHelper.ErrorMessage(aMessage);
        }

        protected override string NormalMessage(string aMessage) {
            return MessageHelper.NormalMessage(aMessage);
        }

        protected override string SuccessMessage(string aMessage) {
            return MessageHelper.SuccessMessage(aMessage);
        }

        protected override string WarningMessage(string aMessage) {
            return MessageHelper.ErrorMessage(aMessage);
        }

        protected override ActionResult RedirectToProfile() {
            return RedirectToAction("Show", "Profile");
        }
    }
}