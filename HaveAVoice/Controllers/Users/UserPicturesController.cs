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

namespace HaveAVoice.Controllers.Users {
    public class PhotosController : HAVBaseController {
        private static string PROFILE_PICTURE_SUCCESS = "New profile picture set!";
        private static string DELETE_SUCCESS = "Pictures deleted successfully!";

        private static string PROFILE_PICTURE_ERROR = "Error setting the profile picture. Please try again.";
        private static string GALLERY_ERROR = "Unable to load your gallery. Please try again.";
        private static string DISPLAY_ERROR = "Unable to display the photo. Please try again.";
        private static string SELECT_ONE_ERROR = "Please select only ONE image.";
        private static string SELECT_ONE_OR_MORE_ERROR = "Please select AT LEAST one or more images.";
        private static string UPLOAD_ERROR = "Error uploading image.";

        private const string EDIT_VIEW = "Edit";
        private static string SHOW_VIEW = "Show";
        private static string LIST_VIEW = "List";
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
        public ActionResult Show(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            User myUser = GetUserInformaton();

            IEnumerable<Photo> myPictures = new List<Photo>();
            try {
                myPictures = thePhotoService.GetPhotos(myUser, id);
            } catch(NotFriendException e) {
                return SendToErrorPage(HAVConstants.NOT_FRIEND);
            } catch (Exception e) {
                LogError(e, GALLERY_ERROR);
                return SendToErrorPage(GALLERY_ERROR);
            }

            return View(SHOW_VIEW, myPictures);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult List() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();

            LoggedInListModel<Photo> myModel = new LoggedInListModel<Photo>(myUser, SiteSection.Photos);
            try {
                myModel.Models = thePhotoService.GetPhotos(myUser, myUser.Id);
            } catch (Exception e) {
                LogError(e, GALLERY_ERROR);
                return SendToErrorPage(GALLERY_ERROR);
            }

            return View(LIST_VIEW, myModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Display(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();

            LoggedInWrapperModel<string> myModel = new LoggedInWrapperModel<string>(myUser, SiteSection.Photos);
            try {
                myModel.Model = PhotoHelper.ConstructUrl(thePhotoService.GetPhoto(myUser, id).ImageName);
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
            IEnumerable<Photo> myPhotos = thePhotoService.GetPhotos(myUser, myUser.Id);
            string profilePicture = PhotoHelper.ProfilePicture(myUser);

            PhotosModel myModel = new PhotosModel() {
                UserId = myUser.Id,
                ProfilePictureURL = profilePicture,
                Photos = myPhotos
            };

            return View(EDIT_VIEW, myModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(int albumId, HttpPostedFileBase imageFile) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                User myUser = GetUserInformatonModel().Details;
                thePhotoService.UploadImageWithDatabaseReference(myUser, albumId, imageFile);
                TempData["Message"] = "Image uploaded!";
                return RedirectToAction(LIST_VIEW);
            } catch(CustomException myException) {
                TempData["Message"] = myException.Message;
            } catch (Exception myException) {
                TempData["Message"] = UPLOAD_ERROR;
                LogError(myException, UPLOAD_ERROR);
            }

            return RedirectToAction(LIST_VIEW);
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
