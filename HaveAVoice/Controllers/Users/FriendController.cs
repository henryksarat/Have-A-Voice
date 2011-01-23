using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Services;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Repositories;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers;
using HaveAVoice.Controllers.Helpers;

namespace HaveAVoice.Controllers.Users
{
    public class FriendController : HAVBaseController {
        private const string APPROVED = "Friend approved!";
        private const string DECLINED = "Friend declined!";

        private const string FRIEND_ERROR = "Unable to become a friend. Please try again.";
        private static string FRIENDS_ERROR = "Unable to get the friends list. Please try again.";
        private static string FRIENDS_OF_ERROR = "Unable to get the people who are friends of this user. Please try again.";

        private const string ERROR_MESSAGE_VIEWDATA = "Message";
        private const string FRIENDS_VIEW = "Friends";
        private const string LIST_VIEW = "List";
        private const string FRIENDS_OF_VIEW = "FriendsOf";
        private const string PENDING_FRINEDS_VIEW = "Pending";
      

        private IHAVFriendService theFriendService;

        public FriendController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
                theFriendService = new HAVFriendService();
        }

        public FriendController(IHAVBaseService aBaseService, IHAVFriendService aFriendService)
            : base(aBaseService) {
                theFriendService = aFriendService;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();

            try {
                theFriendService.AddFriend(myUser, id);
            } catch (Exception e) {
                LogError(e, FRIEND_ERROR);
                return SendToErrorPage(FRIEND_ERROR);
            }

            return RedirectToAction("Show", "Profile", new { id = id });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult List() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformatonModel().Details;
            LoggedInListModel<Friend> myModel = new LoggedInListModel<Friend>(myUser, SiteSection.Message);
            try {
                myModel.Models = theFriendService.FindFriendsForUser(myUser.Id);
            } catch (Exception e) {
                LogError(e, FRIENDS_ERROR);
                ViewData[ERROR_MESSAGE_VIEWDATA] = MessageHelper.ErrorMessage(FRIENDS_ERROR);
            }

            return View(LIST_VIEW, myModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Friends(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            IEnumerable<Friend> myFriends = new List<Friend>();
            try {
                myFriends = theFriendService.FindFriendsForUser(id);
            } catch (Exception e) {
                LogError(e, FRIENDS_ERROR);
                ViewData[ERROR_MESSAGE_VIEWDATA] = MessageHelper.ErrorMessage(FRIENDS_ERROR);
            }

            return View(FRIENDS_VIEW, myFriends);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Pending() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            IEnumerable<Friend> myPendingFriends = new List<Friend>();
            try {
                myPendingFriends = theFriendService.FindPendingFriendsForUser(GetUserInformaton().Id);
            } catch (Exception e) {
                LogError(e, HAVConstants.ERROR);
                ViewData[ERROR_MESSAGE_VIEWDATA] = MessageHelper.ErrorMessage(HAVConstants.ERROR);
            }

            return View(PENDING_FRINEDS_VIEW, myPendingFriends);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Approve(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                theFriendService.ApproveFriend(id);
                TempData[ERROR_MESSAGE_VIEWDATA] = MessageHelper.SuccessMessage(APPROVED);
            } catch (Exception e) {
                LogError(e, HAVConstants.ERROR);
                TempData[ERROR_MESSAGE_VIEWDATA] = MessageHelper.ErrorMessage(HAVConstants.ERROR);
            }

            return RedirectToAction("Pending");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Decline(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                theFriendService.DeclineFriend(id);
                TempData[ERROR_MESSAGE_VIEWDATA] = MessageHelper.SuccessMessage(DECLINED);
            } catch (Exception e) {
                LogError(e, HAVConstants.ERROR);
                TempData[ERROR_MESSAGE_VIEWDATA] = MessageHelper.ErrorMessage(HAVConstants.ERROR);
            }

            return RedirectToAction("Pending");
        }
    }
}
