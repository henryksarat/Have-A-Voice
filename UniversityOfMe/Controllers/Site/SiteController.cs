using System.Web.Mvc;
using Social.Generic.ActionFilters;
using Social.Generic.Constants;
using Social.Generic.Models;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Services.Users;
using BaseWebsite.Controllers.Core;
using UniversityOfMe.UserInformation;
using Social.Authentication;
using System.Web;
using UniversityOfMe.Helpers;
using UniversityOfMe.Repositories;
using Social.Users.Services;
using Social.Generic.Services;
using UniversityOfMe.Controllers.Helpers;
using UniversityOfMe.Repositories.Site;
using Social.Authentication.Helpers;
using UniversityOfMe.Models.SocialModels;
using Social.Validation;

namespace UniversityOfMe.Controllers.Clubs {
    public class SiteController : AbstractSiteController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline> {
        private IUofMeUserService theUserService;
        private IValidationDictionary theValidationDictionary;

        public SiteController() :
            base(new BaseService<User>(new EntityBaseRepository()),
           UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()),
           InstanceHelper.CreateAuthencationService(),
           new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()),
           new EntityContactUsRepository()) {
           UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
           theValidationDictionary = new ModelStateWrapper(this.ModelState);
           theUserService = new UofMeUserService(theValidationDictionary);
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        public ActionResult Main(string universityId) {           
            if (IsLoggedIn()) {
                return RedirectToProfile();
            }
           
            return View("Main", new CreateUserModel() {
                RegisteredUserCount = theUserService.GetNumberOfRegisteredUsers(),
                Genders = new SelectList(Constants.GENDERS, Constants.SELECT)
            });
        }

        public ActionResult About() {
            return View("About");
        }

        public ActionResult Terms() {
            return View("Terms");
        }

        public ActionResult Privacy() {
            return View("Privacy");
        }

        [ImportModelStateFromTempData]
        public ActionResult ContactUs() {
            return View("ContactUs");
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult ContactUs(string email, string inquiryType, string inquiry) {
            return base.ContactUs(email, inquiryType, inquiry);
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

        protected override ActionResult RedirectToProfile() {
            return RedirectToAction(UOMConstants.UNVIERSITY_MAIN_VIEW, UOMConstants.UNVIERSITY_MAIN_CONTROLLER, new { universityId = UniversityHelper.GetMainUniversityId(GetUserInformaton()) });
        }
    }
}
