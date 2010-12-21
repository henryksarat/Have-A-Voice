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
    public class MessageController : HAVBaseController
    {
        private IHAVMessageService theService;
        private IHAVUserService theUserService;

        public MessageController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
            IValidationDictionary myValidationDictionary = new ModelStateWrapper(this.ModelState);
            theService = new HAVMessageService(myValidationDictionary);
            theUserService = new HAVUserService(myValidationDictionary);
        }

        public MessageController(IHAVBaseService aBaseService, IHAVMessageService aMessageService, IHAVUserService aUserService)
            : base(aBaseService) {
            theService = aMessageService;
            theUserService = aUserService;
        }

        public ActionResult Index() {
            User user = GetUserInformaton();
            if (user == null) {
                return RedirectToLogin();
            }

            try {
                List<InboxMessage> messages = theService.GetMessagesForUser(user).ToList<InboxMessage>();

                if (messages.Count == 0) {
                    ViewData["Message"] = "You have no messages.";
                }

                return View("Index", messages);
            } catch (Exception exception) {
                LogError(exception, "Error getting messages for the myUser id " + user.Id + ".");
                return SendToErrorPage("An error occurred when we tried to load your inbox. " +
                "Don't worry, it's not gone! Please try again.");
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(List<Int32> selectedMessages) {
            User user = GetUserInformaton();
            if (user == null) {
                return RedirectToLogin();
            }

            try {
                theService.DeleteMessages(selectedMessages, user);
            } catch (Exception e) {
                LogError(e, "Error deleting the messages for the myUser id " + user.Id + ".");
                ViewData["Message"] = "An error occurred, please try again.";
            }

            return View("Index");
        }

        public ActionResult SendMessage(int id) {
            if (GetUserInformaton() == null) {
                return RedirectToLogin();
            }

            try {
                ViewData["ToUser"] = theUserService.GetUser(id);
            } catch (Exception e) {
                LogError(e, "Unable to get myUser data for the myUser with the id " + id);
                return SendToErrorPage("Unable to get the information for that myUser. Please try again.");
            }

            return View("SendMessage");
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendMessage(Message messageToCreate) {
            User user = GetUserInformaton();
            if (user == null) {
                return RedirectToLogin();
            }
            
            try {
                if (!theService.CreateMessage(user.Id, messageToCreate)) {
                    ViewData["ToUser"] = theUserService.GetUser(messageToCreate.ToUserId);
                    return View("SendMessage", messageToCreate.ToUserId);
                }
            } catch (Exception e) {
                LogError(e, "Unable to send a message from the myUser with the id " + user.Id + " to the myUser with the id with " + 32);
                ViewData["Message"] = "Unable to send the message! Please try again in a few minutes.";
               // return View("SendMessage", toUser);
            }

            return SendToResultPage("Message sent successfully!");
        }

        public ActionResult ViewMessage(int messageId) {
            User user = GetUserInformaton();

            if (user == null) {
                return RedirectToLogin();
            }

            try {
                if (!theService.AllowedToViewMessageThread(user, messageId)) {
                    return Redirect("Index");
                }
            } catch (Exception e) {
                LogError(e, "Error deciding if the myUser is allowed to view the message. [UserId="+ user.Id + ";MessageId="+messageId+"]");
                return SendToErrorPage("Error retrieving the message, please try again.");
            }
            
            Message messageToDisplay;
            try {
                messageToDisplay = theService.GetMessage(messageId, user);
            } catch (Exception e) {
                LogError(e, "Error retrieving the message. [MessageId=" + messageId + "]");
                return SendToErrorPage("Error retrieving the message, please try again.");
            }

            return View("ViewMessage", new ViewMessageModel(messageToDisplay));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ViewMessage(ViewMessageModel viewMessageModel) {
            User user = GetUserInformaton();
            if (user == null) {
                return RedirectToLogin();
            }

            try {
                if (theService.CreateReply(viewMessageModel.Message.Id, user, viewMessageModel.Reply)) {
                    return SendToResultPage("Message Sent!");
                }

            } catch (Exception e) {
                LogError(e, "Error posting a reply. [UserId=" + user.Id + ";MessageId=" + viewMessageModel.Message.Id + "]");
                ViewData["Message"] = "Error sending comment. Please try again.";
            }

            return View("ViewMessage", viewMessageModel);
        }

        protected override ActionResult SendToResultPage(string title, string details) {
            return SendToResultPage(SiteSectionsEnum.Message, title, details);
        }

    }
}
