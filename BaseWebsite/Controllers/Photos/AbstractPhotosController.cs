using System;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication;
using Social.Authentication.Services;
using Social.BaseWebsite.Models;
using Social.Friend.Repositories;
using Social.Friend.Services;
using Social.Generic.Exceptions;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Photo.Repositories;
using Social.Photo.Services;
using Social.Users.Services;

namespace BaseWebsite.Controllers.Photos {
    //T = User
    //U = Role
    //V = Permission
    //W = UserRole
    //X = PrivacySetting
    //Y = RolePermission
    //Z = WhoIsOnline
    //A = PhotoAlbum
    //B = Photo
    //C = Friend
    public abstract class AbstractPhotosController<T, U, V, W, X, Y, Z, A, B, C> : BaseController<T, U, V, W, X, Y, Z> {
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

        private IPhotoService<T, A, B, C> thePhotoService;


        public AbstractPhotosController(IBaseService<T> aBaseService, IUserInformation<T, Z> aUserInformation, IAuthenticationService<T, U, V, W, X, Y> anAuthService,
                                        IWhoIsOnlineService<T, Z> aWhoIsOnlineService, IPhotoAlbumRepository<T, A, B> aPhotoAlbumRepo, IPhotoRepository<T, B> aPhotoRepo, 
                                        IFriendRepository<T, C> aFriendRepo)
            : base(aBaseService, aUserInformation, anAuthService, aWhoIsOnlineService) {
                thePhotoService = new PhotoService<T, A, B, C>(new FriendService<T, C>(aFriendRepo), aPhotoAlbumRepo, aPhotoRepo);
        }

        protected abstract ILoggedInModel<B> CreateLoggedInWrapperModel(T aUser);

        protected ActionResult Create(int albumId, HttpPostedFileBase imageFile) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<T> myUser = GetUserInformatonModel();

            try {
                thePhotoService.UploadImageWithDatabaseReference(CreateSocialUserModel(myUser.Details), albumId, imageFile);
                TempData["Message"] = SuccessMessage(UPLOAD_SUCCESS);
            } catch (CustomException myException) {
                TempData["Message"] = NormalMessage(myException.Message);
            } catch (Exception myException) {
                TempData["Message"] = ErrorMessage(UPLOAD_ERROR);
                LogError(myException, UPLOAD_ERROR);
            }
            return RedirectToAction(PHOTO_ALBUM_DETAILS, PHOTO_ALBUM_CONTROLLER, new { id = albumId });
        }

        protected ActionResult Display(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            T myUser = GetUserInformaton();

            ILoggedInModel<B> myModel = CreateLoggedInWrapperModel(myUser);
            try {
                myModel.Set(thePhotoService.GetPhoto(CreateSocialUserModel(myUser), id).Model);
            } catch (Exception e) {
                LogError(e, DISPLAY_ERROR);
                return SendToErrorPage(DISPLAY_ERROR);
            }

            return View(DISPLAY_VIEW, myModel);
        }

        protected ActionResult SetProfilePicture(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            T myUser = GetUserInformaton();
            try {
                thePhotoService.SetToProfilePicture(CreateSocialUserModel(GetUserInformaton()), id);
                TempData["Message"] = SuccessMessage("Profile picture changed!");
                return RedirectToProfile();
            } catch (CustomException myException) {
                return SendToErrorPage(myException.Message);
            } catch (Exception e) {
                TempData["Message"] = ErrorMessage(SET_PROFILE_PICTURE_ERRROR);
                return RedirectToAction(DISPLAY_VIEW, new { id = id });
            }
        }

        protected ActionResult Delete(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            T myUser = GetUserInformaton();
            try {
                thePhotoService.DeletePhoto(CreateSocialUserModel(myUser), id);
                TempData["Message"] = SuccessMessage(DELETE_SUCCESS);
            } catch(CustomException e) {
                TempData["Message"] = NormalMessage(e.Message);
            } catch (Exception e) {
                TempData["Message"] = ErrorMessage(DELETE_ERROR);
            }

            return RedirectToAction(DISPLAY_VIEW, new { id = id });
        }

        protected ActionResult SetAlbumCover(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            T myUser = GetUserInformaton();
            try {
                thePhotoService.SetPhotoAsAlbumCover(CreateSocialUserModel(myUser), id);
                TempData["Message"] = SuccessMessage(ALBUM_COVER_SET);
                return RedirectToAction(PHOTO_ALBUM_LIST, PHOTO_ALBUM_CONTROLLER);
            } catch (CustomException e) {
                return SendToErrorPage(e.Message);
            } catch (Exception e) {
                LogError(e, ALBUM_COVER_ERROR);
                TempData["Message"] = ErrorMessage(ALBUM_COVER_ERROR);
            }

            return RedirectToAction(DISPLAY_VIEW, new { id = id });
        }
    }
}
