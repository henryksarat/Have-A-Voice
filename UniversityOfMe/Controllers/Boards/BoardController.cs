using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers.Boards;
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
using UniversityOfMe.Repositories.Boards;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Boards {
    public class BoardController : AbstractBoardController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline, Board, BoardReply> {

        public BoardController()
            : base(new BaseService<User>(new EntityBaseRepository()),
                   UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()),
                   InstanceHelper.CreateAuthencationService(), 
                   new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()),
                   new EntityBoardRepository()) {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        new public ActionResult Details(int id) {
            return base.Details(id);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        new public ActionResult Create(int sourceUserId, string boardMessage) {
            return base.Create(sourceUserId, boardMessage);
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        new public ActionResult Edit(int id) {
            return base.Edit(id);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Edit(SocialBoardModel aBoard) {
            return base.Edit(aBoard);
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        new public ActionResult Delete(int sourceId, int boardId, string sourceController, string sourceAction) {
            return base.Delete(sourceId, boardId, sourceController, sourceAction);
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

        protected override ILoggedInModel<Board> CreateLoggedInWrapperModel(User aUser) {
            return new LoggedInWrapperModel<Board>(aUser);
        }

        protected override int GetPostedByUserId(Board aMessage) {
            return aMessage.PostedUserId;
        }

        protected override ActionResult RedirectToProfile() {
            return RedirectToAction("Show", "Profile");
        }
    }
}
