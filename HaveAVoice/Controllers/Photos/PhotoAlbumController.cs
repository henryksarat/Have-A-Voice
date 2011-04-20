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
using Social.Generic.ActionFilters;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Users.Services;

namespace HaveAVoice.Controllers.Users.Photos {
    public class PhotoAlbumController : AbstractPhotoAlbumController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline, PhotoAlbum, Photo, Friend> {
        public PhotoAlbumController() :
            base(new BaseService<User>(new HAVBaseRepository()),
                  UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository())),
                  new HAVAuthenticationService(),
                  new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository()),
                  new EntityHAVPhotoAlbumRepository(),
                  new EntityHAVPhotoRepository(),
                  new EntityHAVFriendRepository(),
                  new EntityHAVUserRetrievalRepository()) {
            HAVUserInformationFactory.SetInstance(GetUserInformationInstance());
        }

        [RequiredRouteValueAttribute.RequireRouteValues(new string[] { })]
        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        new public ActionResult List() {
            return base.List();
        }

        [RequiredRouteValueAttribute.RequireRouteValues(new[] { "id" })]
        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        new public ActionResult List(int id) {
            return base.List(id);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        new public ActionResult Create(string name, string description) {
            return base.Create(name, description);
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        new public ActionResult Delete(int id) {
            return base.Delete(id);
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        new public ActionResult Details(int id) {
            return base.Details(id);
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData, ImportModelStateFromTempData]
        new public ActionResult Edit(int id) {
            return base.Edit(id);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        new public ActionResult Edit(int albumId, string name, string description) {
            return base.Edit(albumId, name, description);
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

        protected override ILoggedInModel<PhotoAlbum> CreateLoggedInWrapperModel(User aUser) {
            return new LoggedInWrapperModel<PhotoAlbum>(aUser, SiteSection.Photos);
        }

        protected override ILoggedInListModel<PhotoAlbum> CreateLoggedInListModel(User myUserOfAlbum, User aRequestingUser) {
            return new LoggedInListModel<PhotoAlbum>(myUserOfAlbum, aRequestingUser, SiteSection.Photos);
        }

        protected override ActionResult RedirectToProfile() {
            return RedirectToAction("Show", "Profile");
        }
    }
}
