using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Social.Authentication;
using Social.Authentication.Services;
using Social.BaseWebsite.Models;
using Social.Generic.Constants;
using Social.Generic.Models;
using Social.Generic.Services;
using Social.Messaging.Repositories;
using Social.Messaging.Services;
using Social.User.Repositories;
using Social.User.Services;
using Social.Users.Services;
using Social.Validation;

namespace BaseWebsite.Controllers.Messaging {
    //T = User
    //U = Role
    //V = Permission
    //W = UserRole
    //X = PrivacySetting
    //Y = RolePermission
    //Z = WhoIsOnline
    //A = Message
    //B = Reply
    public abstract class AbstractMessageController<T, U, V, W, X, Y, Z, A, B> : BaseController<T, U, V, W, X, Y, Z> {
        private const string NO_MESSAGES = "You have no messages";
        private const string SEND_SUCCESS = "Message sent successfully!";
        private const string REPLY_SUCCESS = "Reply sent successfully!";
        private const string NO_MESSAGES_TO_DELETE = "No messages selected to be deleted.";
        private const string MESSAGES_DELETED = "Messages deleted successfully!";

        private const string INBOX_LOAD_ERROR = "An error occurred while trying to load your inbox. Please try again.";
        private const string USER_RETRIEVAL_ERROR = "Unable to retreieve the users information. Please try again.";
        private const string SEND_ERROR = "An error occurred while sending the message. Please try again.";
        private const string RETRIEVE_ERROR = "Error retrieving the message. Please try again.";
        private const string REPLY_ERROR = "An error occurred while sending the reply. Please try again.";

        private const string CONTROLLER_NAME = "Message";
        private const string ERROR_MESSAGE_VIEWDATA = "Message";
        private const string INBOX_VIEW = "Inbox";
        private const string CREATE_VIEW = "Create";
        private const string VIEW_MESSAGE_VIEW = "Details";

        private IMessageService<T, A, B> theService;
        private IUserRetrievalService<T> theUserRetrievalService;
        private IValidationDictionary theValidationDictionary;

        public AbstractMessageController(IBaseService<T> aBaseService, IUserInformation<T, Z> aUserInformation, IAuthenticationService<T, U, V, W, X, Y> anAuthService,
                                 IWhoIsOnlineService<T, Z> aWhoIsOnlineService, IUserRetrievalRepository<T> aUserRetrievalRepo, IMessageRepository<T, A, B> aMessageRepo)
            : base(aBaseService, aUserInformation, anAuthService, aWhoIsOnlineService) {
            theValidationDictionary = new ModelStateWrapper(this.ModelState);
            theService = new MessageService<T, A, B>(theValidationDictionary, aMessageRepo);
            theUserRetrievalService = new UserRetrievalService<T>(aUserRetrievalRepo);
        }

        protected abstract ILoggedInListModel<InboxMessage<T>> CreateLoggedInListModelForInbox(T aUser);
        protected abstract ILoggedInModel<A> CreatedLoggedInModelForViewingMessage(T aUser);
        protected abstract ILoggedInModel<T> CreatedLoggedInModelForCreatingAMessage(T aUser);
        protected abstract AbstractMessageModel<A, T> CreateNewMessageSocialMessageModel(T aUser);

        protected ActionResult Inbox() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            T myUser = GetUserInformaton();
            ILoggedInListModel<InboxMessage<T>> myModel = CreateLoggedInListModelForInbox(myUser);
            try {
                AbstractUserModel<T> mySocialModel = CreateSocialUserModel(myUser);
                myModel.Set(theService.GetMessagesForUser(mySocialModel).ToList<InboxMessage<T>>());
                if (myModel.Get().Count<InboxMessage<T>>() == 0) {
                    ViewData[ERROR_MESSAGE_VIEWDATA] = NormalMessage(NO_MESSAGES);
                }
            } catch (Exception e) {
                LogError(e, INBOX_LOAD_ERROR);
                ViewData[ERROR_MESSAGE_VIEWDATA] = ErrorMessage(INBOX_LOAD_ERROR);
            }

            return View(INBOX_VIEW, myModel);
        }

        protected ActionResult DeleteMessages(List<Int32> selectedMessages) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                T myUser = GetUserInformaton();
                if (selectedMessages == null || selectedMessages.Count == 0) {
                    TempData["Message"] += NormalMessage(NO_MESSAGES_TO_DELETE);
                } else {
                    theService.DeleteMessages(selectedMessages, myUser);
                    ForceUserInformationRefresh();
                    TempData["Message"] += SuccessMessage(MESSAGES_DELETED);
                }
            } catch (Exception e) {
                LogError(e, ErrorKeys.ERROR_MESSAGE);
                TempData[ERROR_MESSAGE_VIEWDATA] = ErrorMessage(ErrorKeys.ERROR_MESSAGE);
            }

            return RedirectToAction(INBOX_VIEW);
        }

        protected ActionResult Create(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            if (isSelf(id)) {
                return RedirectToAction(INBOX_VIEW);
            }

            T myUserSendingMessageTo = theUserRetrievalService.GetUser(id);
            T myLoggedInUser = GetUserInformatonModel().Details;
            ILoggedInModel<T> myModel = CreatedLoggedInModelForCreatingAMessage(myLoggedInUser);

            try {
                myModel.Set(myUserSendingMessageTo);
                return View(CREATE_VIEW, myModel);
            } catch (Exception e) {
                LogError(e, USER_RETRIEVAL_ERROR);
                return SendToErrorPage(USER_RETRIEVAL_ERROR);
            }
        }

        protected ActionResult Create(int aToUserId, string aSubject, string aBody) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                if (theService.CreateMessage(UserId(), aToUserId, aSubject, aBody)) {
                    TempData["Message"] += SuccessMessage(SEND_SUCCESS);
                    return RedirectToProfile(aToUserId);
                }
            } catch (Exception e) {
                LogError(e, SEND_ERROR);
                TempData[ERROR_MESSAGE_VIEWDATA] = ErrorMessage(SEND_ERROR);
                theValidationDictionary.ForceModleStateExport();
            }

            return RedirectToAction(CREATE_VIEW, CONTROLLER_NAME, new { id = aToUserId });
        }

        protected ActionResult Details(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            T myUser = GetUserInformaton();

            try {
                if (!theService.AllowedToViewMessageThread(myUser, id)) {
                    return RedirectToAction(INBOX_VIEW);
                }
            } catch (Exception e) {
                LogError(e, RETRIEVE_ERROR);
                return SendToErrorPage(RETRIEVE_ERROR);
            }

            try {
                A myMessageToDisplay = theService.GetMessage(id, myUser);
                ILoggedInModel<A> myModel = CreatedLoggedInModelForViewingMessage(myUser);
                myModel.Set(myMessageToDisplay);
                return View(VIEW_MESSAGE_VIEW, myModel);
            } catch (Exception e) {
                LogError(e, RETRIEVE_ERROR);
                return SendToErrorPage(RETRIEVE_ERROR);
            }
        }

        protected ActionResult CreateReply(int aMessageId, string aReply) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                T myUser = GetUserInformaton();
                if (theService.CreateReply(aMessageId, myUser, aReply)) {
                    TempData["Message"] += SuccessMessage(REPLY_SUCCESS);
                }
            } catch (Exception e) {
                LogError(e, REPLY_ERROR);
                TempData[ERROR_MESSAGE_VIEWDATA] = ErrorMessage(REPLY_ERROR);
                theValidationDictionary.ForceModleStateExport();
            }

            return RedirectToAction(VIEW_MESSAGE_VIEW, new { id = aMessageId });
        }

        private bool isSelf(int id) {
            return UserId() == id;
        }
    }
}
