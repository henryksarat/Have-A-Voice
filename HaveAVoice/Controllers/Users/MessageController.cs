using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Controllers.Helpers;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Models.SocialWrappers;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services;
using HaveAVoice.Services.Helpers;
using HaveAVoice.Services.UserFeatures;
using Social.Generic.Models;
using Social.Messaging.Services;
using Social.User.Models;
using Social.Validation;
using Social.User.Services;
using Social.Generic.Services;

namespace HaveAVoice.Controllers.Users {
    public class MessageController : HAVBaseController {
        private const string NO_MESSAGES = "You have no messages";
        private const string SEND_SUCCESS = "Message sent successfully!";
        private const string REPLY_SUCCESS = "Reply sent successfully!";
        private const string NO_MESSAGES_TO_DELETE = "No messages selected to be deleted.";

        private const string INBOX_LOAD_ERROR = "An error occurred while trying to load your inbox. Please try again.";
        private const string USER_RETRIEVAL_ERROR = "Unable to retreieve the users information. Please try again.";
        private const string SEND_ERROR = "An error occurred while sending the message. Please try again.";
        private const string RETRIEVE_ERROR = "Error retrieving the message. Please try again.";
        private const string REPLY_ERROR = "An error occurred while sending the reply. Please try again.";

        private const string ERROR_MESSAGE_VIEWDATA = "Message";
        private const string INBOX_VIEW = "Inbox";
        private const string CREATE_VIEW = "Create";
        private const string VIEW_MESSAGE_VIEW = "View";

        private IMessageService<User, Message, Reply> theService;
        private IUserRetrievalService<User> theUserRetrievalService;

        public MessageController() : 
            base(new BaseService<User>(new HAVBaseRepository())) {
            IValidationDictionary myValidationDictionary = new ModelStateWrapper(this.ModelState);
            theService = new MessageService<User, Message, Reply>(myValidationDictionary, new EntityHAVMessageRepository());
            theUserRetrievalService = new UserRetrievalService<User>(new EntityHAVUserRetrievalRepository());
        }

        public MessageController(IBaseService<User> aBaseService, IMessageService<User, Message, Reply> aMessageService, IUserRetrievalService<User> aUserRetrievalService)
            : base(aBaseService) {
            theService = aMessageService;
            theUserRetrievalService = aUserRetrievalService;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Inbox() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            User myUser = GetUserInformaton();
            LoggedInListModel<InboxMessage> myModel = new LoggedInListModel<InboxMessage>(myUser, SiteSection.Message);
            try {
                AbstractUserModel<User> mySocialModel = SocialUserModel.Create(myUser);
                myModel.Models = theService.GetMessagesForUser(mySocialModel).ToList<InboxMessage>();
                if (myModel.Models.Count<InboxMessage>() == 0) {
                    ViewData[ERROR_MESSAGE_VIEWDATA] = MessageHelper.NormalMessage(NO_MESSAGES);
                }
            } catch (Exception e) {
                LogError(e, INBOX_LOAD_ERROR);
                ViewData[ERROR_MESSAGE_VIEWDATA] = MessageHelper.ErrorMessage(INBOX_LOAD_ERROR);
            }

            return View(INBOX_VIEW, myModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Inbox(List<Int32> selectedMessages) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                User myUser = GetUserInformaton();
                if (selectedMessages == null || selectedMessages.Count == 0) {
                    TempData["Message"] = MessageHelper.NormalMessage(NO_MESSAGES_TO_DELETE);
                } else {
                    theService.DeleteMessages(selectedMessages, myUser);
                }
            } catch (Exception e) {
                LogError(e, HAVConstants.ERROR);
                TempData[ERROR_MESSAGE_VIEWDATA] = MessageHelper.ErrorMessage(HAVConstants.ERROR);
            }

            return RedirectToAction(INBOX_VIEW);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            if (isSelf(id)) {
                return RedirectToAction(INBOX_VIEW);
            }

            User myUser = theUserRetrievalService.GetUser(id);

            LoggedInWrapperModel<MessageWrapper> myModel = new LoggedInWrapperModel<MessageWrapper>(myUser, SiteSection.Message);

            try {
                string myProfilePictureUrl = PhotoHelper.ProfilePicture(myUser);
                myModel.Model = MessageWrapper.Build(myUser, myProfilePictureUrl);
                return View(CREATE_VIEW, myModel);
            } catch (Exception e) {
                LogError(e,  USER_RETRIEVAL_ERROR);
                return SendToErrorPage(USER_RETRIEVAL_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(MessageWrapper aMessage) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            User myUser = GetUserInformaton();
            
            try {
                AbstractMessageModel<Message> mySocialMessage = SocialMessageWrapper.Create(aMessage.ToModel());
                if (theService.CreateMessage(myUser.Id, mySocialMessage)) {
                    TempData["Message"] = MessageHelper.SuccessMessage(SEND_SUCCESS);
                    return RedirectToProfile(aMessage.ToUserId);
                }
            } catch (Exception e) {
                LogError(e, SEND_ERROR);
                ViewData[ERROR_MESSAGE_VIEWDATA] = MessageHelper.ErrorMessage(SEND_ERROR);
            }

            LoggedInWrapperModel<MessageWrapper> myModel = new LoggedInWrapperModel<MessageWrapper>(myUser, SiteSection.Message);
            myModel.Model = aMessage;
            return View(CREATE_VIEW, myModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult View(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            User myUser = GetUserInformaton();
            
            try {
                if (!theService.AllowedToViewMessageThread(myUser, id)) {
                    return RedirectToAction(INBOX_VIEW);
                }
            } catch (Exception e) {
                LogError(e, RETRIEVE_ERROR);
                return SendToErrorPage(RETRIEVE_ERROR);
            }
            
            Message myMessageToDisplay;
            try {
                myMessageToDisplay = theService.GetMessage(id, myUser);
            } catch (Exception e) {
                LogError(e, RETRIEVE_ERROR);
                return SendToErrorPage(RETRIEVE_ERROR);
            }

            LoggedInWrapperModel<ViewMessageModel> myModel = new LoggedInWrapperModel<ViewMessageModel>(myUser, SiteSection.Message);
            myModel.Model = new ViewMessageModel(myMessageToDisplay);
            return View(VIEW_MESSAGE_VIEW, myModel);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateReply(ViewMessageModel viewMessageModel) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                User myUser = GetUserInformaton();
                if (theService.CreateReply(viewMessageModel.Message.Id, myUser, viewMessageModel.Reply)) {
                    TempData["Message"] = MessageHelper.SuccessMessage(REPLY_SUCCESS);
                    return RedirectToAction("View", new { id = viewMessageModel.Message.Id });
                }
            } catch (Exception e) {
                LogError(e, REPLY_ERROR);
                ViewData[ERROR_MESSAGE_VIEWDATA] = MessageHelper.ErrorMessage(REPLY_ERROR);
            }

            return View(VIEW_MESSAGE_VIEW, viewMessageModel);
        }

        private bool isSelf(int id) {
            return GetUserInformaton().Id == id;
        }
    }
}
