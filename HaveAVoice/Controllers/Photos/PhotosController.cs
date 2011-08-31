using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers.Photos;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Models;
using HaveAVoice.Models.SocialWrappers;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.BaseWebsite.Models;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Users.Services;
using HaveAVoice.Helpers.AWS;
using HaveAVoice.Helpers.Configuration;

namespace HaveAVoice.Controllers.Users.Photos {
    public class PhotosController : AbstractPhotosController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline, PhotoAlbum, Photo, Friend> {
        private const int PHOTO_MAX_SIZE = 840;
        private const int MAX_SIZE_PROFILE = 120;

        public PhotosController() : 
            base(new BaseService<User>(new HAVBaseRepository()),
                  UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository()), new GetUserStrategy()),
                  new HAVAuthenticationService(),
                  new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository()),
                  new EntityHAVPhotoAlbumRepository(),
                  new EntityHAVPhotoRepository(),
                  new EntityHAVFriendRepository()) {
            HAVUserInformationFactory.SetInstance(GetUserInformationInstance());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(int albumId, HttpPostedFileBase imageFile) {
            return base.Create(albumId, imageFile, AWSHelper.GetClient(), SiteConfiguration.UserPhotosBucket(), PHOTO_MAX_SIZE);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Display(int id) {
            return base.Display(id);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SetProfilePicture(int id) {
            return base.SetProfilePicture(id, AWSHelper.GetClient(), SiteConfiguration.UserPhotosBucket(), MAX_SIZE_PROFILE);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id, int albumId) {
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
            return MessageHelper.ErrorMessage(aMessage);
        }

        protected override ILoggedInModel<Photo> CreateLoggedInWrapperModel(User aUser) {
            return new LoggedInWrapperModel<Photo>(aUser, SiteSection.Photos);
        }

        protected override ActionResult RedirectToProfile() {
            return RedirectToAction("Show", "Profile");
        }
    }
}
