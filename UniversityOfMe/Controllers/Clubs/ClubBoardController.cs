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
using UniversityOfMe.Services.Professors;

namespace UniversityOfMe.Controllers.Clubs {
    public class ClubBoardController : BaseController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline> {
        private const string POSTED_TO_BOARD = "Posted to the clubs board!";

        private const string POSTING_TO_BOARD_ERROR = "An error occurred while posting to the club board. Please try again.";

        IClubService theClubService;

        public ClubBoardController()
            : base(new BaseService<User>(new EntityBaseRepository()),
                   UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())),
                   InstanceHelper.CreateAuthencationService(),
                   new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository())) {
            IValidationDictionary myModelState = new ModelStateWrapper(this.ModelState);
            theClubService = new ClubService(myModelState);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(string boardMessage, int clubId) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            UserInformationModel<User> myUserInformation = GetUserInformatonModel();

            try {
                bool myResult = theClubService.PostToClubBoard(myUserInformation, clubId, boardMessage);
                if (myResult) {
                    TempData["Message"] = POSTED_TO_BOARD;
                }
            } catch (Exception myException) {
                LogError(myException, POSTING_TO_BOARD_ERROR);
                TempData["Message"] = POSTING_TO_BOARD_ERROR;
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
