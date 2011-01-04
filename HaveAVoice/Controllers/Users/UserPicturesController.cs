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

namespace HaveAVoice.Controllers.Users
{
    public class UserPicturesController : HAVBaseController {
        private static string PROFILE_PICTURE_SUCCESS = "New profile picture set!";
        private static string DELETE_SUCCESS = "Pictures deleted successfully!";

        private static string PROFILE_PICTURE_ERROR = "Error setting the profile picture. Please try again.";
        private static string GALLERY_ERROR = "Unable to load your gallery. Please try again.";
        private static string SELECT_ONE_ERROR = "Please select only ONE image.";
        private static string SELECT_ONE_OR_MORE_ERROR = "Please select AT LEAST one or more images.";
        private static string UPLOAD_ERROR = "Error uploading image.";

        private const string EDIT_VIEW = "Edit";
        private static string SHOW_VIEW = "Show";
        private static string LIST_VIEW = "List";
        

        private IHAVUserPictureService theUserPictureService;

        public UserPicturesController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
            theUserPictureService = new HAVUserPictureService();
        }

        public UserPicturesController(IHAVUserPictureService aUserPictureService, IHAVBaseService aBaseService)
            : base(aBaseService) {
                theUserPictureService = aUserPictureService;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Show(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            User myUser = GetUserInformaton();

            IEnumerable<UserPicture> myPictures = new List<UserPicture>();
            try {
                myPictures = theUserPictureService.GetUserPictures(myUser, myUser.Id);
            } catch(NotFanException e) {
                return SendToErrorPage(HAVConstants.NOT_FAN);
            } catch (Exception e) {
                LogError(e, GALLERY_ERROR);
                return SendToErrorPage(GALLERY_ERROR);
            }

            return View(SHOW_VIEW, myPictures);
        }

        public ActionResult List() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();

            IEnumerable<UserPicture> myPictures = new List<UserPicture>();
            try {
                myPictures = theUserPictureService.GetUserPictures(myUser, myUser.Id);
            } catch (Exception e) {
                LogError(e, GALLERY_ERROR);
                return SendToErrorPage(GALLERY_ERROR);
            }

            return View(LIST_VIEW, myPictures);
        }

        public ActionResult Edit() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            IEnumerable<UserPicture> userPictures = theUserPictureService.GetUserPictures(myUser, myUser.Id);
            string profilePicture = ProfilePictureHelper.ProfilePicture(myUser);

            UserPicturesModel myModel = new UserPicturesModel() {
                UserId = myUser.Id,
                ProfilePictureURL = profilePicture,
                UserPictures = userPictures
            };

            return View(EDIT_VIEW, myModel);
        }

        public ActionResult Add(HttpPostedFileBase imageFile ) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                User myUser = GetUserInformatonModel().Details;
                theUserPictureService.UploadImageWithDatabaseReference(myUser, imageFile);
                TempData["Message"] = "Image uploaded!";
                return RedirectToAction(LIST_VIEW);
            } catch (Exception myException) {
                TempData["Message"] = UPLOAD_ERROR;
                LogError(myException, UPLOAD_ERROR);
            }

            return RedirectToAction(LIST_VIEW);
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
