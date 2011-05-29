using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers.Messaging;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Models;
using HaveAVoice.Models.SocialWrappers;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.Helpers;
using HaveAVoice.Services.UserFeatures;
using Social.Authentication;
using Social.Authentication.Helpers;
using Social.BaseWebsite.Models;
using Social.Generic.ActionFilters;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Messaging.Services;
using Social.User.Services;
using Social.Users.Services;

namespace HaveAVoice.Controllers.Users {
    public class MessageController : AbstractMessageController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline, Message, Reply> {
        public MessageController() :
            base(new BaseService<User>(new HAVBaseRepository()),
                  UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository())),
                  new HAVAuthenticationService(),
                  new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository()),
                  new EntityHAVUserRetrievalRepository(),
                  new EntityHAVMessageRepository()) {
            HAVUserInformationFactory.SetInstance(GetUserInformationInstance());
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Inbox() {
            return base.Inbox();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        new public ActionResult Inbox(List<Int32> selectedMessages) {
            return base.Inbox(selectedMessages);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Create(int id) {
            return base.Create(id);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        new public ActionResult Create(int toUserId, string subject, string body) {
            return base.Create(toUserId, subject, body);
        }
        
        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        new public ActionResult Details(int id) {
            return base.Details(id);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        public ActionResult CreateReply(Message message, string reply) {
            return base.CreateReply(message.Id, reply);
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

        private bool isSelf(int id) {
            return GetUserInformaton().Id == id;
        }

        protected override ILoggedInListModel<InboxMessage<User>> CreateLoggedInListModelForInbox(User aUser) {
            return new LoggedInListModel<InboxMessage<User>>(aUser, SiteSection.Message);
        }

        protected override AbstractMessageModel<Message, User> CreateNewMessageSocialMessageModel(User aUser) {
            return new SocialMessageModel() {
                ToUserId = aUser.Id,
                ToUserFullName = NameHelper.FullName(aUser),
                ToUserProfilePictureUrl = PhotoHelper.ProfilePicture(aUser)
            };
        }

        protected override ILoggedInModel<Message> CreatedLoggedInModelForViewingMessage(User aUser) {
            return new LoggedInWrapperModel<Message>(aUser, SiteSection.Message);
        }

        protected override ActionResult RedirectToProfile() {
            return RedirectToAction("Show", "Profile");
        }

        protected override ILoggedInModel<User> CreatedLoggedInModelForCreatingAMessage(User aUser) {
            return new LoggedInWrapperModel<User>(aUser, SiteSection.Message);
        }
    }
}
