using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Models;
using HaveAVoice.Models.Validation;
using HaveAVoice.Models.View;
using HaveAVoice.Models.Services;
using HaveAVoice.Models.Repositories;
using HaveAVoice.Helpers;
using HaveAVoice.Models.Services.UserFeatures;

namespace HaveAVoice.Controllers.Users
{
    public class MessageController : HAVBaseController
    {
        private IHAVMessageService theService;
        private IHAVUserService theUserService;

        public MessageController() : 
            base(new HAVBaseService(new HAVBaseRepository())) {
            theService = new HAVMessageService(new ModelStateWrapper(this.ModelState));
        }

        public MessageController(IHAVMessageService service, IHAVBaseService baseService, IHAVUserService userService)
            : base(baseService) {
            theService = service;
            theUserService = userService;
        }

        public ActionResult Index() {
            User user = GetUserInformaton();
            if (user == null) {
                return RedirectToAction("Login", "User");
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
                return RedirectToAction("Login", "User");
            }

            try {
                theService.DeleteMessages(selectedMessages, user);
            } catch (Exception e) {
                LogError(e, "Error deleting the messages for the myUser id " + user.Id + ".");
                ViewData["Message"] = "An error occurred, please try again.";
            }

            return View("Index");
        }

        public ActionResult SendMessage(int toUserId) {
            if (GetUserInformaton() == null) {
                return RedirectToAction("Login", "User");
            }

            try {
                ViewData["ToUser"] = theUserService.GetUser(toUserId);
            } catch (Exception e) {
                LogError(e, "Unable to get myUser data for the myUser with the id " + toUserId);
                return SendToErrorPage("Unable to get the information for that myUser. Please try again.");
            }

            return View("SendMessage");
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendMessage([Bind(Exclude = "Id, ToUserId, FromUserId")] Message messageToCreate, User toUser) {
            User user = GetUserInformaton();
            if (user == null) {
                return RedirectToAction("Login", "User");
            }

            try {
                if (!theService.CreateMessage(toUser.Id, user.Id, messageToCreate)) {
                    ViewData["ToUser"] = theUserService.GetUser(toUser.Id);
                    return View("SendMessage", toUser);
                }
            } catch (Exception e) {
                LogError(e, "Unable to send a message from the myUser with the id " + user.Id + " to the myUser with the id with " + toUser.Id);
                ViewData["Message"] = "Unable to send the message! Please try again in a few minutes.";
                return View("SendMessage", toUser);
            }

            return SendToResultPage("Message sent successfully!");
        }

        public ActionResult ViewMessage(int messageId) {
            User user = GetUserInformaton();

            if (user == null) {
                return RedirectToAction("Login", "User");
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
                return RedirectToAction("Login", "User");
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

        public override ActionResult SendToResultPage(string title, string details) {
            return SendToResultPage(SiteSectionsEnum.Message, title, details);
        }

    }
}
