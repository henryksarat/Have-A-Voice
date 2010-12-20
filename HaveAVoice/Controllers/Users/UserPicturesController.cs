using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Services;
using HaveAVoice.Repositories;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers.ActionMethods;
using System.Text;

namespace HaveAVoice.Controllers.Users
{
    public class UserPicturesController : HAVBaseController {
        private IHAVUserPictureService theUserPictureService;

        public UserPicturesController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
            theUserPictureService = new HAVUserPictureService();
        }

        public UserPicturesController(IHAVUserPictureService aUserPictureService, IHAVBaseService aBaseService)
            : base(aBaseService) {
                theUserPictureService = aUserPictureService;
        }

        public ActionResult List() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            int myUserId = GetUserInformaton().Id;

            IEnumerable<UserPicture> myPictures = new List<UserPicture>();
            try {
                myPictures = theUserPictureService.GetUserPictures(myUserId);
            } catch (Exception e) {
                LogError(e, new StringBuilder().AppendFormat("Unable to get the user pictures. [userId={0}]", myUserId).ToString());
                SendToErrorPage("Unable to load your gallery. Please try again.");
            }

            return View("Gallery", myPictures);
        }

        public ActionResult UserPictures() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            int myUserId = GetUserInformaton().Id;
            IEnumerable<UserPicture> userPictures = theUserPictureService.GetUserPictures(myUserId);
            UserPicture profilePicture = theUserPictureService.GetProfilePicture(myUserId);

            return View("UserPictures", new UserPicturesModel(profilePicture, userPictures, new List<int>()));

        }

        [ActionName("UserPictures")]
        [AcceptParameter(Name = "button", Value = "SetProfilePicture")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserPictures_SetProfilePicture(UserPicturesModel aUserPicturesModel) {
            if (aUserPicturesModel.SelectedUserPictures.Count != 1) {
                ViewData["Message"] = "Please select only ONE image to be your profile image.";
            } else {
                try {
                    theUserPictureService.SetToProfilePicture(GetUserInformaton(), aUserPicturesModel.SelectedUserPictures.First());
                    return SendToResultPage("New profile picture set!");
                } catch (Exception e) {
                    LogError(e, "Error setting profile picture. Please try again.");
                    ViewData["Message"] = "An error occurred while trying to set your new profile picture.";
                }
            }

            return View("UserPictures", aUserPicturesModel);
        }

        [ActionName("UserPictures")]
        [AcceptParameter(Name = "button", Value = "Delete")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserPictures_Delete(UserPicturesModel aUserPicturesModel) {
            if (aUserPicturesModel.SelectedUserPictures.Count == 0) {
                ViewData["Message"] = "To delete a picture you have to at least select one.";
            } else {
                try {
                    theUserPictureService.DeleteUserPictures(aUserPicturesModel.SelectedUserPictures);
                    return SendToResultPage("Pictures deleted.");
                } catch (Exception e) {
                    LogError(e, "Error deleting userToListenTo pictures.");
                    ViewData["Message"] = "An error occurred while trying to delete the pictures.";
                }
            }

            return View("UserPictures", aUserPicturesModel);
        }

        protected override ActionResult SendToResultPage(string aTitle, string aDetails) {
            return SendToResultPage(SiteSectionsEnum.UserPictures, aTitle, aDetails);
        }
    }
}
