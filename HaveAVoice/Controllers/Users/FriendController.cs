﻿using System.Web;
using System.Web.Mvc;
using BaseWebsite.Controllers.Friends;
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
using Social.BaseWebsite.Models;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Users.Services;

namespace HaveAVoice.Controllers.Users {
    public class FriendController : AbstractFriendController<User, Role, Permission, UserRole, PrivacySetting, RolePermission, WhoIsOnline, Friend> {

        public FriendController() : 
            base(new BaseService<User>(new HAVBaseRepository()),
                 UserInformation<User, WhoIsOnline>.Instance(new HttpContextWrapper(System.Web.HttpContext.Current), new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository()), new GetUserStrategy()),
                 new HAVAuthenticationService(),
                 new WhoIsOnlineService<User, WhoIsOnline>(new EntityHAVWhoIsOnlineRepository()),
                 new EntityHAVFriendRepository()) {
            HAVUserInformationFactory.SetInstance(GetUserInformationInstance());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        new public ActionResult Add(int id) {
            return base.Add(id);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult Delete(int id) {
            return base.Delete(id);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        new public ActionResult List() {
            return base.List();
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

        protected override ILoggedInListModel<Friend> CreateLoggedInListModel(User aUser) {
            return new LoggedInListModel<Friend>(aUser, SiteSection.Friend);
        }

        protected override ActionResult RedirectToProfile() {
            return RedirectToAction("Show", "Profile");
        }
    }
}
