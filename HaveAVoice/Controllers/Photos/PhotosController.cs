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
        private const string PROFILE_PICTURE_SUCCESS = "New profile picture set!";
        private const string DELETE_SUCCESS = "Photo deleted successfully!";
        private const string UPLOAD_SUCCESS = "Photo uploaded!";
        private const string ALBUM_COVER_SET = "Album cover set!";

        private const string PROFILE_PICTURE_ERROR = "Error setting the profile picture. Please try again.";
        private const string DISPLAY_ERROR = "Unable to display the photo. Please try again.";
        private const string SELECT_ONE_ERROR = "Please select only ONE image.";
        private const string SELECT_ONE_OR_MORE_ERROR = "Please select AT LEAST one or more images.";
        private const string SET_PROFILE_PICTURE_ERRROR = "Error settings profile picture.";
        private const string DELETE_ERROR = "Error deleting photo.";
        private const string UPLOAD_ERROR = "Error uploading the picture. Please try again.";
        private const string ALBUM_COVER_ERROR = "Error settings photo as the album cover. Please try again.";

        private const string PHOTO_ALBUM_DETAILS = "Details";
        private const string PHOTO_ALBUM_CONTROLLER = "PhotoAlbum";
        private const string PHOTO_ALBUM_LIST = "List";
        private const string EDIT_VIEW = "Edit";
        private const string DISPLAY_VIEW = "Display";

        private IHAVPhotoService thePhotoService;

        public PhotosController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
            thePhotoService = new HAVPhotoService();
        }

        public PhotosController(IHAVPhotoService aPhotoService, IHAVBaseService aBaseService)
            : base(aBaseService) {
                thePhotoService = aPhotoService;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(int albumId, HttpPostedFileBase imageFile) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel myUser = GetUserInformatonModel();

            try {
                thePhotoService.UploadImageWithDatabaseReference(myUser, albumId, imageFile);
                TempData["Message"] = UPLOAD_SUCCESS;
            } catch (CustomException myException) {
                TempData["Message"] = myException.Message;
            } catch (Exception myException) {
                TempData["Message"] = UPLOAD_ERROR;
                LogError(myException, UPLOAD_ERROR);
            }
            return RedirectToAction(PHOTO_ALBUM_DETAILS, PHOTO_ALBUM_CONTROLLER, new { id = albumId });
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
        public ActionResult SetProfilePicture(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            try {
                thePhotoService.SetToProfilePicture(GetUserInformaton(), id);
                return RedirectToProfile();
            } catch (CustomException myException) {
                return SendToErrorPage(myException.Message);
            } catch (Exception e) {
                TempData["Message"] = SET_PROFILE_PICTURE_ERRROR;
                return RedirectToAction(DISPLAY_VIEW, new { id = id });
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            try {
                thePhotoService.DeletePhoto(myUser, id);
                TempData["Message"] = DELETE_SUCCESS;
            } catch(CustomException e) {
                TempData["Message"] = e.Message;
            } catch (Exception e) {
                TempData["Message"] = DELETE_ERROR;
            }

            return RedirectToAction(DISPLAY_VIEW, new { id = id });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SetAlbumCover(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            try {
                thePhotoService.SetPhotoAsAlbumCover(myUser, id);
                TempData["Message"] = ALBUM_COVER_SET;
                return RedirectToAction(PHOTO_ALBUM_LIST, PHOTO_ALBUM_CONTROLLER);
            } catch (CustomException e) {
                return SendToErrorPage(e.Message);
            } catch (Exception e) {
                LogError(e, ALBUM_COVER_ERROR);
                TempData["Message"] = ALBUM_COVER_ERROR;
            }

            return RedirectToAction(DISPLAY_VIEW, new { id = id });
        }

        /*
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
                    //thePhotoService.DeletePhotos(aPhotosModel.SelectedPhotos);
                    return SendToResultPage(DELETE_SUCCESS);
                } catch (Exception e) {
                    LogError(e, HAVConstants.ERROR);
                    ViewData["Message"] = HAVConstants.ERROR;
                }
            }

            return View(EDIT_VIEW, aPhotosModel);
        }
         * */
    }
}
