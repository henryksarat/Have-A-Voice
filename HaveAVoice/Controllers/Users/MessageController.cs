using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Models;
using HaveAVoice.Validation;
using HaveAVoice.Models.View;
using HaveAVoice.Services;
using HaveAVoice.Repositories;
using HaveAVoice.Helpers;
using HaveAVoice.Services.UserFeatures;

namespace HaveAVoice.Controllers.Users
{
    public class MessageController : HAVBaseController {
        private const string NO_MESSAGES = "You have no messages";
        private const string SEND_SUCCESS = "Message sent successfully!";
        private const string REPLY_SUCCESS = "Reply sent successfully!";

        private const string INBOX_LOAD_ERROR = "An error occurred while trying to load your inbox. Please try again.";
        private const string USER_RETRIEVAL_ERROR = "Unable to retreieve the users information. Please try again.";
        private const string SEND_ERROR = "An error occurred while sending the message. Please try again.";
        private const string RETRIEVE_ERROR = "Error retrieving the message. Please try again.";
        private const string REPLY_ERROR = "An error occurred while sending the reply. Please try again.";

        private IHAVMessageService theService;
        private IHAVUserService theUserService;
        private IHAVUserPictureService theUserPicturesService;

        public MessageController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
            IValidationDictionary myValidationDictionary = new ModelStateWrapper(this.ModelState);
            theService = new HAVMessageService(myValidationDictionary);
            theUserService = new HAVUserService(myValidationDictionary);
            theUserPicturesService = new HAVUserPictureService();
        }

        public MessageController(IHAVBaseService aBaseService, IHAVMessageService aMessageService, IHAVUserService aUserService, IHAVUserPictureService aUserPicturesService)
            : base(aBaseService) {
            theService = aMessageService;
            theUserService = aUserService;
            theUserPicturesService = aUserPicturesService;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }
            try {
                User myUser = GetUserInformaton();
                List<InboxMessage> messages = theService.GetMessagesForUser(myUser).ToList<InboxMessage>();

                if (messages.Count == 0) {
                    ViewData["Message"] = NO_MESSAGES;
                }

                return View("Index", messages);
            } catch (Exception e) {
                LogError(e, INBOX_LOAD_ERROR);
                return SendToErrorPage(INBOX_LOAD_ERROR);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(List<Int32> selectedMessages) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                User myUser = GetUserInformaton();
                theService.DeleteMessages(selectedMessages, myUser);
            } catch (Exception e) {
                LogError(e, HAVConstants.ERROR);
                ViewData["Message"] = HAVConstants.ERROR;
            }

            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                User myUser = theUserService.GetUser(id);
                string myProfilePictureUrl = theUserPicturesService.GetProfilePictureURL(myUser);
                MessageWrapper myMessage = MessageWrapper.Build(myUser, myProfilePictureUrl);
                return View("Create", myMessage);
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
            
            try {
                User myUser = GetUserInformaton();
                if (theService.CreateMessage(myUser.Id, aMessage.ToModel())) {
                    return SendToResultPage(SEND_SUCCESS);
                }
            } catch (Exception e) {
                LogError(e, SEND_ERROR);
                ViewData["Message"] = SEND_ERROR;
            }

            return View("Create", aMessage);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult View(int id) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            User myUser = GetUserInformaton();

            try {
                if (!theService.AllowedToViewMessageThread(myUser, id)) {
                    return Redirect("Index");
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

            return View("View", new ViewMessageModel(myMessageToDisplay));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateReply(ViewMessageModel viewMessageModel) {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            try {
                User myUser = GetUserInformaton();
                if (theService.CreateReply(viewMessageModel.Message.Id, myUser, viewMessageModel.Reply)) {
                    return SendToResultPage(REPLY_SUCCESS);
                }
            } catch (Exception e) {
                LogError(e, REPLY_ERROR);
                ViewData["Message"] = REPLY_ERROR;
            }

            return View("View", viewMessageModel);
        }

        protected override ActionResult SendToResultPage(string title, string details) {
            return SendToResultPage(SiteSectionsEnum.Message, title, details);
        }
    }
}
