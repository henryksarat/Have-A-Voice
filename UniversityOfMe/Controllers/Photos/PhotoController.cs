using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers.Photos;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.BaseWebsite.Models;
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
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Photos {
    public class PhotoController : AbstractPhotosController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline, PhotoAlbum, Photo, Friend> {
        public PhotoController()
            : base(new BaseService<User>(new EntityBaseRepository()), 
                    UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())), 
                    InstanceHelper.CreateAuthencationService(), 
                    new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), 
                    new EntityPhotoAlbumRepository(),
                    new EntityPhotoRepository(),
                    new EntityFriendRepository()) {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())));
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        new public ActionResult Create(int albumId, HttpPostedFileBase imageFile) {
            return base.Create(albumId, imageFile);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Display(int id) {
            return base.Display(id);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult SetProfilePicture(int id) {
            return base.SetProfilePicture(id);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Delete(int id, int albumId) {
            return base.Delete(id, albumId);
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
            return aMessage;
        }

        protected override string NormalMessage(string aMessage) {
            return aMessage;
        }

        protected override string SuccessMessage(string aMessage) {
            return aMessage;
        }

        protected override ILoggedInModel<Photo> CreateLoggedInWrapperModel(User aUser) {
            return new LoggedInWrapperModel<Photo>(aUser);
        }

        protected override ActionResult RedirectToProfile() {
            return RedirectToAction(UOMConstants.UNVIERSITY_MAIN_VIEW, UOMConstants.UNVIERSITY_MAIN_CONTROLLER, new { universityId = UniversityHelper.GetMainUniversity(GetUserInformaton()) });
        }
    }
}
