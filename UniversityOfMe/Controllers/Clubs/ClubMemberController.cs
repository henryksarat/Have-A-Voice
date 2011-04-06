using System;
using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers;
using HaveAVoice.Services.Clubs;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.Generic.ActionFilters;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Validation;
using UniversityOfMe.Controllers.Helpers;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Repositories;
using UniversityOfMe.Services;
using UniversityOfMe.Services.Professors;

namespace UniversityOfMe.Controllers.Clubs {
    public class ClubMemberController : BaseController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline> {
        public const string USER_REMVOED = "User removed successfully!";
        public const string REMOVE_ERROR = "Error while removing the member from the club. Please try again.";

        IClubService theClubService;
        IUniversityService theUniversityService;

        public ClubMemberController()
            : base(new BaseService<User>(new EntityBaseRepository()),
                   UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())),
                   InstanceHelper.CreateAuthencationService(),
                   new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())) {
            IValidationDictionary myModelState = new ModelStateWrapper(this.ModelState);
            theClubService = new ClubService(myModelState);
            theUniversityService = new UniversityService();
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        public ActionResult Remove(int userId, int clubId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                bool myResult = theClubService.RemoveClubMember(GetUserInformatonModel(), userId, clubId);

                if (myResult) {
                    TempData["Message"] = USER_REMVOED;
                }
            } catch (Exception myException) {
                LogError(myException, REMOVE_ERROR);
                TempData["Message"] = REMOVE_ERROR;
            }

            return RedirectToAction("Details", "Club", new { id = clubId });
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
    }
}
