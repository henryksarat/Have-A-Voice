using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers.Boards;
using BaseWebsite.Helpers;
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
using Social.Generic.ActionFilters;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Users.Services;

namespace HaveAVoice.Controllers.Users {
    public class BoardReplyController : AbstractBoardReplyController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline, Board, BoardReply> {
        public BoardReplyController()
            : base(new BaseService<User>(new HAVBaseRepository()),
                   UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository()), new GetUserStrategy()),
                   new HAVAuthenticationService(),
                   new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository()),
                   new EntityHAVBoardRepository()) {
            HAVUserInformationFactory.SetInstance(GetUserInformationInstance());
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult Create(int boardId, string boardReply, SiteSection source, int sourceId) {
            BaseWebsite.Helpers.BaseSiteSection mySiteSection = (source == SiteSection.Profile) ? BaseSiteSection.Profile : BaseSiteSection.Board;

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
            return MessageHelper.ErrorMessage(aMessage);
        }

        protected override int GetBoardReplyUserId(BoardReply aBoardReply) {
            return aBoardReply.UserId;
        }

        protected override ActionResult RedirectToProfile() {
            return RedirectToAction("Show", "Profile");
        }
    }
}
