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
using HaveAVoice.Helpers;
using HaveAVoice.Controllers.Helpers;

namespace HaveAVoice.Controllers.Users
{
    public class UserPicturesController : HAVBaseController {
        private static string PROFILE_PICTURE_SUCCESS = "New profile picture set!";
        private static string DELETE_SUCCESS = "Pictures deleted successfully!";

        private static string PROFILE_PICTURE_ERROR = "Error setting the profile picture. Please try again.";
        private static string GALLERY_ERROR = "Unable to load your gallery. Please try again.";
        private static string SELECT_ONE_ERROR = "Please select only ONE image.";
        private static string SELECT_ONE_OR_MORE_ERROR = "Please select AT LEAST one or more images.";

        private const string EDIT_VIEW = "Edit";
        private static string GALLERY_VIEW = "Gallery";
        

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
                LogError(e, GALLERY_ERROR);
                SendToErrorPage(GALLERY_ERROR);
            }

            return View(GALLERY_VIEW, myPictures);
        }

        public ActionResult Edit() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            IEnumerable<UserPicture> userPictures = theUserPictureService.GetUserPictures(myUser.Id);
            string profilePicture = theUserPictureService.GetProfilePictureURL(myUser);

            UserPicturesModel myModel = new UserPicturesModel() {
                UserId = myUser.Id,
                ProfilePictureURL = profilePicture,
                UserPictures = userPictures
            };

            return View(EDIT_VIEW, myModel);

        }

        [ActionName(EDIT_VIEW)]
        [AcceptParameter(Name = "button", Value = "SetProfilePicture")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit_SetProfilePicture(UserPicturesModel aUserPicturesModel) {
            if (aUserPicturesModel.SelectedUserPictures.Count != 1) {
                ViewData["Message"] = SELECT_ONE_ERROR;
            } else {
                try {
                    theUserPictureService.SetToProfilePicture(GetUserInformaton(), aUserPicturesModel.SelectedUserPictures.First());
                    TempData["Message"] = PROFILE_PICTURE_SUCCESS;
                    return RedirectToProfile();
                } catch (Exception e) {
                    LogError(e, PROFILE_PICTURE_ERROR);
                    ViewData["Message"] = PROFILE_PICTURE_ERROR;
                }
            }

            return View(EDIT_VIEW, aUserPicturesModel);
        }

        [ActionName(EDIT_VIEW)]
        [AcceptParameter(Name = "button", Value = "Delete")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit_Delete(UserPicturesModel aUserPicturesModel) {
            if (aUserPicturesModel.SelectedUserPictures.Count == 0) {
                ViewData["Message"] = SELECT_ONE_OR_MORE_ERROR;
            } else {
                try {
                    theUserPictureService.DeleteUserPictures(aUserPicturesModel.SelectedUserPictures);
                    return SendToResultPage(DELETE_SUCCESS);
                } catch (Exception e) {
                    LogError(e, HAVConstants.ERROR);
                    ViewData["Message"] = HAVConstants.ERROR;
                }
            }

            return View(EDIT_VIEW, aUserPicturesModel);
        }

        private ActionResult RedirectToProfile() {
            return RedirectToAction("LoggedIn", "Home");
        }

    }
}
