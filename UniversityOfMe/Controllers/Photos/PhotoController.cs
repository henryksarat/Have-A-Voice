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
using UniversityOfMe.Services.Photos;
using UniversityOfMe.UserInformation;
using System;

namespace UniversityOfMe.Controllers.Photos {
    public class PhotoController : AbstractPhotosController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline, PhotoAlbum, Photo, Friend> {
        private IUofMePhotoService thePhotoService;
        
        public PhotoController() : this(new UofMePhotoService()) { }
        
        public PhotoController(IUofMePhotoService aPhotoService)
            : base(new BaseService<User>(new EntityBaseRepository()), 
                    UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())), 
                    InstanceHelper.CreateAuthencationService(), 
                    new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()),
                    aPhotoService) {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())));
            thePhotoService = aPhotoService;
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        new public ActionResult Create(int albumId, HttpPostedFileBase imageFile) {
            return base.Create(albumId, imageFile);
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
