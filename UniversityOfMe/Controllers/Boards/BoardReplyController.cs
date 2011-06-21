using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers.Boards;
using BaseWebsite.Helpers;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.Generic.ActionFilters;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Users.Services;
using UniversityOfMe.Controllers.Helpers;
using UniversityOfMe.Helpers;
using UniversityOfMe.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.Repositories;
using UniversityOfMe.Repositories.Boards;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Boards {
    public class BoardReplyController : AbstractBoardReplyController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline, Board, BoardReply> {
        public BoardReplyController()
            : base(new BaseService<User>(new EntityBaseRepository()),
                   UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()),
                   InstanceHelper.CreateAuthencationService(), 
                   new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()),
                   new EntityBoardRepository()) {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(int boardId, SiteSection siteSection, string boardReply, int sourceId) {
            BaseWebsite.Helpers.BaseSiteSection mySiteSection = siteSection == SiteSection.Board ? BaseSiteSection.Board : BaseSiteSection.Profile;

            return base.Create(boardId, boardReply, mySiteSection, sourceId);
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        new public ActionResult Edit(int id) {
            return base.Edit(id);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        new public ActionResult Edit(SocialBoardReplyModel aBoardReply) {
            return base.Edit(aBoardReply);
        }

        [AcceptVerbs(HttpVerbs.Get), ExportModelStateToTempData]
        new public ActionResult Delete(int boardId, int boardReplyId) {
            return base.Delete(boardId, boardReplyId);
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

        protected override int GetBoardReplyUserId(BoardReply aBoardReply) {
            return aBoardReply.UserId;
        }

        protected override ActionResult RedirectToProfile() {
            return RedirectToAction("Show", "Profile");
        }
    }
}
