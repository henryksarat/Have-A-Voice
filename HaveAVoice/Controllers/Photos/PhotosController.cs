using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Exceptions;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.ActionMethods;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Services;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Services.Helpers;
using System.Web;

namespace HaveAVoice.Controllers.Users.Photos {
    public class PhotosController : HAVBaseController {
        private static string PROFILE_PICTURE_SUCCESS = "New profile picture set!";
        private static string DELETE_SUCCESS = "Pictures deleted successfully!";

        private static string PROFILE_PICTURE_ERROR = "Error setting the profile picture. Please try again.";
        private static string DISPLAY_ERROR = "Unable to display the photo. Please try again.";
        private static string SELECT_ONE_ERROR = "Please select only ONE image.";
        private static string SELECT_ONE_OR_MORE_ERROR = "Please select AT LEAST one or more images.";
        private static string SET_PROFILE_PICTURE_ERRROR = "Error settings profile picture.";

        private const string EDIT_VIEW = "Edit";
        private static string DISPLAY_VIEW = "Display";      

        private IHAVPhotoService thePhotoService;

        public PhotosController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
            thePhotoService = new HAVPhotoService();
        }

        public PhotosController(IHAVPhotoService aPhotoService, IHAVBaseService aBaseService)
            : base(aBaseService) {
                thePhotoService = aPhotoService;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Display(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();

            LoggedInWrapperModel<Photo> myModel = new LoggedInWrapperModel<Photo>(myUser, SiteSection.Photos);
            try {
                myModel.Model = thePhotoService.GetPhoto(myUser, id);
            } catch (Exception e) {
                LogError(e, DISPLAY_ERROR);
                return SendToErrorPage(DISPLAY_ERROR);
            }

            return View(DISPLAY_VIEW, myModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            //IEnumerable<Photo> myPhotos = thePhotoService.GetPhotos(myUser, myUser.Id);
            string profilePicture = PhotoHelper.ProfilePicture(myUser);

            PhotosModel myModel = new PhotosModel() {
                UserId = myUser.Id,
                ProfilePictureURL = profilePicture,
                //Photos = myPhotos
            };

            return View(EDIT_VIEW, myModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SetProfilePicture(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            try {
                thePhotoService.SetToProfilePicture(GetUserInformaton(), id);
                return RedirectToProfile();
            } catch (Exception e) {
                TempData["Message"] = SET_PROFILE_PICTURE_ERRROR;
                return RedirectToAction(DISPLAY_VIEW, new { id = id});
            }
        }

        [ActionName(EDIT_VIEW)]
        [AcceptParameter(Name = "button", Value = "SetProfilePicture")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit_SetProfilePicture(PhotosModel aPhotosModel) {
            if (aPhotosModel.SelectedPhotos.Count != 1) {
                ViewData["Message"] = SELECT_ONE_ERROR;
            } else {
                try {
                    thePhotoService.SetToProfilePicture(GetUserInformaton(), aPhotosModel.SelectedPhotos.First());
                    TempData["Message"] = PROFILE_PICTURE_SUCCESS;
                    return RedirectToProfile();
                } catch (Exception e) {
                    LogError(e, PROFILE_PICTURE_ERROR);
                    ViewData["Message"] = PROFILE_PICTURE_ERROR;
                }
            }

            return View(EDIT_VIEW, aPhotosModel);
        }

        [ActionName(EDIT_VIEW)]
        [AcceptParameter(Name = "button", Value = "Delete")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit_Delete(PhotosModel aPhotosModel) {
            if (aPhotosModel.SelectedPhotos.Count == 0) {
                ViewData["Message"] = SELECT_ONE_OR_MORE_ERROR;
            } else {
                try {
                    thePhotoService.DeletePhotos(aPhotosModel.SelectedPhotos);
                    return SendToResultPage(DELETE_SUCCESS);
                } catch (Exception e) {
                    LogError(e, HAVConstants.ERROR);
                    ViewData["Message"] = HAVConstants.ERROR;
                }
            }

            return View(EDIT_VIEW, aPhotosModel);
        }
    }
}
