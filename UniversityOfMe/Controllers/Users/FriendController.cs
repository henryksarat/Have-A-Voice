using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers.Friends;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.BaseWebsite.Models;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Users.Services;
using UniversityOfMe.Controllers.Helpers;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.SocialModels;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories;
using UniversityOfMe.Repositories.Friends;
using UniversityOfMe.UserInformation;
using UniversityOfMe.Services.Users;
using System;
using Social.Generic.ActionFilters;

namespace UniversityOfMe.Controllers.Users {
    public class FriendController : AbstractFriendController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline, Friend> {
        private IUofMeUserRetrievalService theUserRetrievalService;
        private const string FRIENDS_ERROR = "Unable to get the list of friends.";

        public FriendController()
            : base(new BaseService<User>(new EntityBaseRepository()), 
                   UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()),
                   InstanceHelper.CreateAuthencationService(), 
                   new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()),
                   new EntityFriendRepository()) {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
            theUserRetrievalService = new UofMeUserRetrievalService();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Add(int id) {
            return base.Add(id);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Delete(int id) {
            return base.Delete(id);
        }

        [ExportModelStateToTempData]
        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult List() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInfo = GetUserInformatonModel();
            LoggedInWrapperModel<FriendListModel> myLoggedIn = new LoggedInWrapperModel<FriendListModel>(myUserInfo.Details);
            try {
                FriendListModel myFriendListModel = new FriendListModel() {
                    Friends = GetFriendService().FindFriendsForUser(myUserInfo.Details.Id),
                    User = theUserRetrievalService.GetUser(myUserInfo.Details.Id)
                };

                myLoggedIn.Set(myFriendListModel);
                return View("List", myLoggedIn);
            } catch (Exception e) {
                LogError(e, FRIENDS_ERROR);
                ViewData["Message"] = ErrorMessage(FRIENDS_ERROR);
                return RedirectToHomePage();
            }
        }

        [ExportModelStateToTempData]
        public ActionResult ListForUser(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            UserInformationModel<User> myUserInfo = GetUserInformatonModel();
            LoggedInWrapperModel<FriendListModel> myLoggedIn = new LoggedInWrapperModel<FriendListModel>(myUserInfo.Details);
            try {
                FriendListModel myFriendListModel = new FriendListModel() {
                    Friends = GetFriendService().FindFriendsForUser(id),
                    User = theUserRetrievalService.GetUser(id)
                };               
                
                myLoggedIn.Set(myFriendListModel);
                return View("List", myLoggedIn);
            } catch (Exception e) {
                LogError(e, FRIENDS_ERROR);
                ViewData["Message"] = ErrorMessage(FRIENDS_ERROR);
                return RedirectToAction("Show", "Profile", new { id = id });
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Pending() {
            return base.Pending();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Approve(int id) {
            return base.Approve(id);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Decline(int id) {
            return base.Decline(id);
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

        protected override ILoggedInListModel<Friend> CreateLoggedInListModel(User aUser) {
            return new LoggedInListModel<Friend>(aUser);
        }

        protected override ActionResult RedirectToProfile() {
            return RedirectToAction(UOMConstants.UNVIERSITY_MAIN_VIEW, UOMConstants.UNVIERSITY_MAIN_CONTROLLER, new { universityId = UniversityHelper.GetMainUniversityId(GetUserInformaton()) });
        }
    }
}
