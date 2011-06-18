using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers.Messaging;
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
using UniversityOfMe.Repositories.Messaging;
using UniversityOfMe.Repositories.UserRepos;
using UniversityOfMe.UserInformation;

namespace UniversityOfMe.Controllers.Messaging {
    public class MessageController : AbstractMessageController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline, Message, MessageReply> {
        public MessageController()
            : base(new BaseService<User>(new EntityBaseRepository()),
            UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()),
            InstanceHelper.CreateAuthencationService(),
            new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new EntityUserRetrievalRepository(), new EntityMessageRepository()) {
            UserInformationFactory.SetInstance(UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityWhoIsOnlineRepository()), new GetUserStrategy()));
        }


        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Inbox() {
            return base.Inbox();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        new public ActionResult Inbox(List<Int32> selectedMessages) {
            return base.DeleteMessages(selectedMessages);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int messageId) {
            List<Int32> myDeleted = new List<Int32>();
            myDeleted.Add(messageId);
            return base.DeleteMessages(myDeleted);
        }

        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        new public ActionResult Create(int id) {
            return base.Create(id);
        }

        [AcceptVerbs(HttpVerbs.Post), ImportModelStateFromTempData]
        public ActionResult CreateByButtonClick(int id) {
            return base.Create(id);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        new public ActionResult Create(int toUserId, string subject, string body) {
            return base.Create(toUserId, subject, body);
        }
        
        [AcceptVerbs(HttpVerbs.Get), ImportModelStateFromTempData]
        new public ActionResult Details(int id) {
            return base.Details(id);
        }

        [AcceptVerbs(HttpVerbs.Post), ExportModelStateToTempData]
        new public ActionResult CreateReply(int messageId, string reply) {
            return base.CreateReply(messageId, reply);
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
            return new LoggedInListModel<InboxMessage<User>>(aUser);
        }

        protected override AbstractMessageModel<Message, User> CreateNewMessageSocialMessageModel(User aUser) {
            return new SocialMessageModel() {
                ToUserId = aUser.Id,
                ToUserFullName = NameHelper.FullName(aUser),
                ToUserProfilePictureUrl = PhotoHelper.ProfilePicture(aUser)
            };
        }

        protected override ILoggedInModel<Message> CreatedLoggedInModelForViewingMessage(User aUser) {
            return new LoggedInWrapperModel<Message>(aUser);
        }

        protected override ActionResult RedirectToProfile() {
            return RedirectToAction("Show", "Profile");
        }

        protected override ILoggedInModel<User> CreatedLoggedInModelForCreatingAMessage(User aUser) {
            return new LoggedInWrapperModel<User>(aUser);
        }
    }
}
