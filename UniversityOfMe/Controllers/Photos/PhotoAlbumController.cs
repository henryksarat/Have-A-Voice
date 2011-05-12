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
using UniversityOfMe.Models.Social;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Repositories.Friends;
using UniversityOfMe.Repositories.Photos;
using UniversityOfMe.Repositories.UserRepos;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Photos {
    public class PhotoAlbumController : AbstractPhotoAlbumController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline, PhotoAlbum, Photo, Friend> {
        public PhotoAlbumController()
            : base(new BaseService<User>(new EntityBaseRepository()), 
                    UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())), 
                    InstanceHelper.CreateAuthencationService(), 
                    new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), 
                    new EntityPhotoAlbumRepository(),
                    new EntityPhotoRepository(),
                    new EntityFriendRepository(),
                    new EntityUserRetrievalRepository()) {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())));
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

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        new public ActionResult Create() {
            return View("Create");
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

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
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
            return aMessage;
        }

        protected override string NormalMessage(string aMessage) {
            return aMessage;
        }

        protected override string SuccessMessage(string aMessage) {
            return aMessage;
        }

        protected override ILoggedInModel<PhotoAlbum> CreateLoggedInWrapperModel(User aUser) {
            return new LoggedInWrapperModel<PhotoAlbum>(aUser);
        }

        protected override ILoggedInListModel<PhotoAlbum> CreateLoggedInListModel(User myUserOfAlbum, User aRequestingUser) {
            return new LoggedInListModel<PhotoAlbum>(myUserOfAlbum);
        }

        protected override ActionResult RedirectToProfile() {
            return RedirectToAction(UOMConstants.UNVIERSITY_MAIN_VIEW, UOMConstants.UNVIERSITY_MAIN_CONTROLLER, new { universityId = UniversityHelper.GetMainUniversity(GetUserInformaton()) });
        }
    }
}
