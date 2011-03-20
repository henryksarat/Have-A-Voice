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
using Social.Friend.Services;
using HaveAVoice.Repositories.UserFeatures;
using Social.User.Models;
using HaveAVoice.Models.SocialWrappers;

namespace HaveAVoice.Controllers.Users
{
    public class FriendController : HAVBaseController {
        private const string APPROVED = "Friend approved!";
        private const string DECLINED = "Friend declined!";
        private const string ALREADY_FRIEND = "You are already friends with the user.";
        private const string PENDING_REQUEST = "There already is a pending friend request.";
        private const string PENDING_RESPONSE = "This user already sent you a friend request. Please response to it via the navigation on top.";
        private const string REQUEST_SENT = "Friend request sent!";
        private const string NO_FRIENDS = "You currently have no friends in your friend list.";
        private const string NO_FRIEND_REQUESTS = "You have no pending friend requests.";
        private const string DELETE_NON_FRIEND = "Unable to delete someone that isn't your friend./";
        private const string DELETE_SUCCESS = "User has been deleted from your friend list successfully.";
        private const string FRIEND_YOURSELF = "You can't friend yourself.";

        private const string FRIEND_REQUEST_ERROR = "Error sending friend request. Please try again.";
        private const string FRIEND_ERROR = "Unable to become a friend. Please try again.";
        private const string FRIENDS_ERROR = "Unable to get the friends list. Please try again.";
        private const string FRIENDS_OF_ERROR = "Unable to get the people who are friends of this user. Please try again.";
        private const string FRIEND_DELETE_ERROR = "An error has occurred trying to remove the user from your list. Please try again.";

        private const string ERROR_MESSAGE_VIEWDATA = "Message";
        private const string LIST_VIEW = "List";
        private const string FRIENDS_OF_VIEW = "FriendsOf";
        private const string PENDING_FRINEDS_VIEW = "Pending";


        private IFriendService<User, Friend> theFriendService;

        public FriendController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
                theFriendService = new FriendService<User, Friend>(new EntityHAVFriendRepository());
        }

        public FriendController(IHAVBaseService aBaseService, IFriendService<User, Friend> aFriendService)
            : base(aBaseService) {
                theFriendService = aFriendService;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            AbstractUserModel<User> myUser = GetSocialUserInformation();

            try {
                if (theFriendService.IsPending(id, myUser)) {
                    TempData["Message"] = MessageHelper.NormalMessage(PENDING_REQUEST);
                } else if (theFriendService.IsPendingForResponse(myUser, id)) {
                    TempData["Message"] = MessageHelper.NormalMessage(PENDING_RESPONSE);
                } else if (theFriendService.IsFriend(id, myUser)) {
                    TempData["Message"] = MessageHelper.NormalMessage(ALREADY_FRIEND);
                } else if (myUser.Id == id) {
                    TempData["Message"] = MessageHelper.NormalMessage(FRIEND_YOURSELF);
                } else {
                    theFriendService.AddFriend(myUser, id);
                    TempData["Message"] = MessageHelper.SuccessMessage(REQUEST_SENT);
                }
            } catch (Exception e) {
                LogError(e, FRIEND_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(FRIEND_REQUEST_ERROR);
            }

            return RedirectToAction("Show", "Profile", new { id = id });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            AbstractUserModel<User> myUser = GetSocialUserInformation();

            try {
                if (!theFriendService.IsFriend(id, myUser)) {
                    TempData["Message"] = MessageHelper.NormalMessage(DELETE_NON_FRIEND);
                } else {
                    theFriendService.RemoveFriend(myUser, id);
                    TempData["Message"] = MessageHelper.SuccessMessage(DELETE_SUCCESS);
                }
            } catch (Exception e) {
                LogError(e, FRIEND_ERROR);
                TempData["Message"] = MessageHelper.ErrorMessage(FRIEND_DELETE_ERROR);
            }

            return RedirectToAction("List");
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
        public ActionResult Pending() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformatonModel().Details;
            LoggedInListModel<Friend> myModel = new LoggedInListModel<Friend>(myUser, SiteSection.Friend);
            try {
                myModel.Models = theFriendService.FindPendingFriendsForUser(GetUserInformaton().Id);
                if (myModel.Models.Count<Friend>() == 0) {
                    TempData["Message"] = MessageHelper.NormalMessage(NO_FRIEND_REQUESTS);
                }
                RefreshUserInformation();
            } catch (Exception e) {
                LogError(e, HAVConstants.ERROR);
                ViewData[ERROR_MESSAGE_VIEWDATA] = MessageHelper.ErrorMessage(HAVConstants.ERROR);
            }

            return View(PENDING_FRINEDS_VIEW, myModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Approve(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                theFriendService.ApproveFriend(id);
                TempData[ERROR_MESSAGE_VIEWDATA] = MessageHelper.SuccessMessage(APPROVED);
                RefreshUserInformation();
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
                RefreshUserInformation();
            } catch (Exception e) {
                LogError(e, HAVConstants.ERROR);
                TempData[ERROR_MESSAGE_VIEWDATA] = MessageHelper.ErrorMessage(HAVConstants.ERROR);
            }

            return RedirectToAction("Pending");
        }
    }
}
