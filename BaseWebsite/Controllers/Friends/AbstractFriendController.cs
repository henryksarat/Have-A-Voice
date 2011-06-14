using System;
using System.Linq;
using System.Web.Mvc;
using Social.Authentication;
using Social.Authentication.Services;
using Social.BaseWebsite.Models;
using Social.Friend.Repositories;
using Social.Friend.Services;
using Social.Generic.Constants;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Users.Services;

namespace BaseWebsite.Controllers.Friends {
    public abstract class AbstractFriendController<T, U, V, W, X, Y, Z, A> : BaseController<T, U, V, W, X, Y, Z> {
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


        private IFriendService<T, A> theFriendService;

        public AbstractFriendController(IBaseService<T> aBaseService, IUserInformation<T, Z> aUserInformation, IAuthenticationService<T, U, V, W, X, Y> anAuthService,
                                        IWhoIsOnlineService<T, Z> aWhoIsOnlineService, IFriendRepository<T, A> aFriendRepo) : base(aBaseService, aUserInformation, anAuthService, aWhoIsOnlineService) {
                theFriendService = new FriendService<T, A>(aFriendRepo);
        }

        protected abstract ILoggedInListModel<A> CreateLoggedInListModel(T aUser);

        protected ActionResult Add(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            AbstractUserModel<T> myUser = GetSocialUserInformation();

            try {
                if (theFriendService.IsPending(id, myUser)) {
                    TempData["Message"] = NormalMessage(PENDING_REQUEST);
                } else if (theFriendService.IsPendingForResponse(myUser, id)) {
                    TempData["Message"] = NormalMessage(PENDING_RESPONSE);
                } else if (theFriendService.IsFriend(id, myUser)) {
                    TempData["Message"] = NormalMessage(ALREADY_FRIEND);
                } else if (myUser.Id == id) {
                    TempData["Message"] = NormalMessage(FRIEND_YOURSELF);
                } else {
                    theFriendService.AddFriend(myUser, id);
                    TempData["Message"] = SuccessMessage(REQUEST_SENT);
                }
            } catch (Exception e) {
                LogError(e, FRIEND_ERROR);
                TempData["Message"] = ErrorMessage(FRIEND_REQUEST_ERROR);
            }

            return RedirectToAction("Show", "Profile", new { id = id });
        }

        protected ActionResult Delete(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            AbstractUserModel<T> myUser = GetSocialUserInformation();

            try {
                if (!theFriendService.IsFriend(id, myUser)) {
                    TempData["Message"] = NormalMessage(DELETE_NON_FRIEND);
                } else {
                    theFriendService.RemoveFriend(myUser, id);
                    TempData["Message"] = SuccessMessage(DELETE_SUCCESS);
                }
            } catch (Exception e) {
                LogError(e, FRIEND_ERROR);
                TempData["Message"] = ErrorMessage(FRIEND_DELETE_ERROR);
            }

            return RedirectToAction("List");
        }

        protected ActionResult List() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            T myUser = GetUserInformatonModel().Details;
            ILoggedInListModel<A> myModel = CreateLoggedInListModel(myUser);
            try {
                myModel.Set(theFriendService.FindFriendsForUser(UserId()));
                if (myModel.Get().Count<A>() == 0) {
                    TempData["Message"] = NormalMessage(NO_FRIENDS);
                }
            } catch (Exception e) {
                LogError(e, FRIENDS_ERROR);
                ViewData[ERROR_MESSAGE_VIEWDATA] = ErrorMessage(FRIENDS_ERROR);
            }

            return View(LIST_VIEW, myModel);
        }

        protected ActionResult Pending() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            T myUser = GetUserInformatonModel().Details;
            ILoggedInListModel<A> myModel = CreateLoggedInListModel(myUser);
            try {
                myModel.Set(theFriendService.FindPendingFriendsForUser(UserId()));
                if (myModel.Get().Count<A>() == 0) {
                    TempData["Message"] = NormalMessage(NO_FRIEND_REQUESTS);
                }
                ForceUserInformationRefresh();
            } catch (Exception e) {
                LogError(e, ErrorKeys.ERROR_MESSAGE);
                ViewData[ERROR_MESSAGE_VIEWDATA] = ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            }

            return View(PENDING_FRINEDS_VIEW, myModel);
        }

        protected ActionResult Approve(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                theFriendService.ApproveFriend(id, GetUserInformatonModel().UserId);
                TempData[ERROR_MESSAGE_VIEWDATA] = SuccessMessage(APPROVED);
                ForceUserInformationRefresh();
            } catch (Exception e) {
                LogError(e, ErrorKeys.ERROR_MESSAGE);
                TempData[ERROR_MESSAGE_VIEWDATA] = ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            }

            return RedirectToAction("Pending");
        }

        protected ActionResult Decline(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                theFriendService.DeclineFriend(id, GetUserInformatonModel().UserId);
                TempData[ERROR_MESSAGE_VIEWDATA] = SuccessMessage(DECLINED);
                ForceUserInformationRefresh();
            } catch (Exception e) {
                LogError(e, ErrorKeys.ERROR_MESSAGE);
                TempData[ERROR_MESSAGE_VIEWDATA] = ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            }

            return RedirectToAction("Pending");
        }
    }
}
