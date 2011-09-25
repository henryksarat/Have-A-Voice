using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers.Photos;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.BaseWebsite.Models;
using Social.Generic.ActionFilters;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Users.Services;
using UniversityOfMe.Controllers.Helpers;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.SocialModels;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services.Photos;
using UniversityOfMe.UserInformation;
using System;
using UniversityOfMe.Helpers.AWS;
using UniversityOfMe.Helpers.Configuration;
using Social.Generic.Exceptions;

namespace UniversityOfMe.Controllers.Photos {
    public class PhotoController : AbstractPhotosController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline, PhotoAlbum, Photo, Friend> {
        private const int MAX_SIZE = 840;
        private const int MAX_SIZE_PROFILE = 120;
        private IUofMePhotoService thePhotoService;

        private const string PROFILE_UPLOAD_SUCCESS = "Your profile picture has been uploaded and set!";

        private const string PROFILE_UPLOAD_ERROR = "Error uploading your profile picture. Please try again.";
        
        public PhotoController() : this(new UofMePhotoService()) { }
        
        public PhotoController(IUofMePhotoService aPhotoService)
            : base(new BaseService<User>(new EntityBaseRepository()),
                    UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()), 
                    InstanceHelper.CreateAuthencationService(), 
                    new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()),
                    aPhotoService) {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            thePhotoService = aPhotoService;
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        new public ActionResult Create(int albumId, HttpPostedFileBase imageFile) {
            return base.Create(albumId, imageFile, AWSHelper.GetClient(), SiteConfiguration.UserPhotosBucket(), MAX_SIZE);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult CreateProfilePicture(HttpPostedFileBase profileFile) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                UserInformationModel<User> myUserInfo = GetUserInformatonModel();
                thePhotoService.UploadProfilePicture(myUserInfo, profileFile, AWSHelper.GetClient(), SiteConfiguration.UserPhotosBucket(), MAX_SIZE);
                TempData["Message"] += MessageHelper.SuccessMessage(PROFILE_UPLOAD_SUCCESS);
            } catch (CustomException e) {
                TempData["Message"] += MessageHelper.ErrorMessage(e.Message);
            } catch (Exception e) {
                LogError(e, PROFILE_UPLOAD_ERROR);
                TempData["Message"] += MessageHelper.ErrorMessage(PROFILE_UPLOAD_ERROR);
            }

            return RedirectToHomePage();
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        new public ActionResult Display(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();

            ILoggedInModel<PhotoDisplayView> myModel = new LoggedInWrapperModel<PhotoDisplayView>(myUser);
            try {
                myModel.Set(thePhotoService.GetPhotoDisplayView(myUser, id));
            } catch (Exception e) {
                LogError(e, DISPLAY_ERROR);
                return SendToErrorPage(DISPLAY_ERROR);
            }

            return View("Display", myModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult SetProfilePicture(int id) {
            return base.SetProfilePicture(id, AWSHelper.GetClient(), SiteConfiguration.UserPhotosBucket(), MAX_SIZE_PROFILE);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Delete(int id, int albumId) {
            return base.Delete(id, AWSHelper.GetClient(), SiteConfiguration.UserPhotosBucket(), albumId);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult SetAlbumCover(int id) {
            return base.SetAlbumCover(id);
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
            return MessageHelper.WarningMessage(aMessage);
        }

        protected override ILoggedInModel<Photo> CreateLoggedInWrapperModel(User aUser) {
            return new LoggedInWrapperModel<Photo>(aUser);
        }

        protected override ActionResult RedirectToProfile() {
            return RedirectToAction(UOMConstants.UNVIERSITY_MAIN_VIEW, UOMConstants.UNVIERSITY_MAIN_CONTROLLER, new { universityId = UniversityHelper.GetMainUniversityId(GetUserInformaton()) });
        }
    }
}
