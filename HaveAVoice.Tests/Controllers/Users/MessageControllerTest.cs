using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HaveAVoice.Controllers.Users;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HaveAVoice.Tests.Controllers.Users {
    [TestClass]
    public class MessageControllerTest : ControllerTestCase {
        private static int SEND_TO_USER_ID = 45;

        private static User theLoggedInUser;
        private static User theSendToUser;
        private ViewMessageModel theViewMessageModel;
        private static Message theMessage;
        private IHAVMessageService theService;
        private Mock<IHAVMessageService> theMockedService;
        private Mock<IHAVUserService> theMockedUserService;
        private Mock<IHAVUserPictureService> theUserPicturesService;
        private Mock<IHAVMessageRepository> theMockRepository;
        private MessageController theController;
        private static List<Int32> theSelectedMessages;

        [TestInitialize]
        public void Initialize() {
            theMockedService = new Mock<IHAVMessageService>();
            theMockedUserService = new Mock<IHAVUserService>();
            theUserPicturesService = new Mock<IHAVUserPictureService>();
            theMockRepository = new Mock<IHAVMessageRepository>();

            theService = new HAVMessageService(new ModelStateWrapper(theModelState),
                                                                   theMockRepository.Object, theBaseRepository.Object);

            theController = new MessageController(theMockedBaseService.Object, theMockedService.Object, theMockedUserService.Object, theUserPicturesService.Object);
            theController.ControllerContext = GetControllerContext();

            theSelectedMessages = new List<Int32>();
            theLoggedInUser = new User();
            theSendToUser = new User();
            theMessage = new Message();
            theMessage.Id = 55;
            theViewMessageModel = new ViewMessageModel(theMessage);
        }

        #region "Message Inbox - NonPostBack"

        [TestMethod]
        public void TestMessageInbox() {
            List<InboxMessage> messages = new List<InboxMessage>();
            messages.Add(new InboxMessage());
            theMockedService.Setup(s => s.GetMessagesForUser(It.IsAny<User>())).Returns(() => messages);

            var result = theController.Index() as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(result, "Index", messages);
            Assert.AreEqual(((List<InboxMessage>)result.ViewData.Model).Count, 1);
        }

        [TestMethod]
        public void TestMessageInbox_NotLoggedIn() {
            MockNotLoggedIn();

            var result = theController.Index() as ViewResult;

            AssertAuthenticatedRedirection(result);
        }

        [TestMethod]
        public void TestMessageInbox_NoMessages() {
            List<InboxMessage> messages = new List<InboxMessage>();
            theMockedService.Setup(s => s.GetMessagesForUser(It.IsAny<User>())).Returns(() => messages);

            var result = theController.Index() as ViewResult;

            AssertAuthenticatedSuccessWithMessage(result, "Index", messages);
            Assert.AreEqual(((List<InboxMessage>)result.ViewData.Model).Count, 0);
        }

        [TestMethod]
        public void TestMessageInbox_Exception() {
            theMockedService.Setup(s => s.GetMessagesForUser(It.IsAny<User>())).Throws<Exception>();

            var result = theController.Index() as ViewResult;

            AssertAuthenticatedErrorLogWithRedirect(result);
        }

        #endregion

        #region "Message Inbox - Delete messages"

        [TestMethod]
        public void TestMessageInboxDeleteMessages() {
            theMockedService.Setup(s => s.DeleteMessages(It.IsAny<List<Int32>>(), It.IsAny<User>()));

            var result = theController.Index(theSelectedMessages) as ViewResult;

            AssertAuthenticatedCleanSuccess(result, "Index");
        }

        [TestMethod]
        public void TestMessageInboxDeleteMessages_NotLoggedIn() {
            MockNotLoggedIn();

            var result = theController.Index(theSelectedMessages) as ViewResult;

            AssertAuthenticatedRedirection(result);
        }

        [TestMethod]
        public void TestMessageInboxDeleteMessages_Exception() {
            theMockedService.Setup(s => s.DeleteMessages(It.IsAny<List<Int32>>(), It.IsAny<User>())).Throws<Exception>();

            var myResult = theController.Index(theSelectedMessages) as ViewResult;

            AssertAuthenticatedErrorLogReturnBack(myResult, "Index");
        }

        #endregion
        /*
        #region "Send Message - NonPostBack"

        [TestMethod]
        public void TestSendMessageNonPostBack_NotLoggedIn() {
            MockNotLoggedIn();

            var result = theController.SendMessage(SEND_TO_USER_ID) as ViewResult;

            AssertAuthenticatedRedirection(result);
        }

        [TestMethod]
        public void TestSendMessageNonPostBack() {
            theMockedUserService.Setup(u => u.GetUser(It.IsAny<int>())).Returns(() => theLoggedInUser);

            var result = theController.SendMessage(SEND_TO_USER_ID) as ViewResult;

            theMockUserInformation.Verify(u => u.GetUserInformaton(), Times.Exactly(1));
            Assert.AreEqual("SendMessage", result.ViewName);
            Assert.IsNotNull(result.ViewData["ToUser"]);
        }

        [TestMethod]
        public void TestSendMessageNonPostBack_Exception() {
            theMockedUserService.Setup(u => u.GetUser(It.IsAny<int>())).Throws<Exception>();

            var result = theController.SendMessage(SEND_TO_USER_ID) as ViewResult;

            AssertAuthenticatedErrorLogWithRedirect(result);
        }

        #endregion*/
        /*
        #region "Send Message"

        [TestMethod]
        public void TestSendMessage_NotLoggedIn() {
            MockNotLoggedIn();

            var result = theController.SendMessage(theMessage, theSendToUser) as ViewResult;

            AssertAuthenticatedRedirection(result);
        }

        [TestMethod]
        public void TestSendMessage() {
            theMockedService.Setup(s => s.CreateMessage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Message>())).Returns(() => true);

            var result = theController.SendMessage(theMessage, theSendToUser) as ViewResult;

            AssertAuthenticatedRedirection(result);
        }

        [TestMethod]
        public void TestSendMessage_Fail() {
            theMockedUserService.Setup(u => u.GetUser(It.IsAny<int>())).Returns(() => theLoggedInUser);
            theMockedService.Setup(s => s.CreateMessage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Message>())).Returns(() => false);

            var result = theController.SendMessage(theMessage, theSendToUser) as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(result, "SendMessage", theSendToUser);
            Assert.IsNotNull(result.ViewData["ToUser"]);
        }

        [TestMethod]
        public void TestSendMessage_Exception() {
            theMockedUserService.Setup(u => u.GetUser(It.IsAny<int>())).Returns(() => theLoggedInUser);
            theMockedService.Setup(s => s.CreateMessage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Message>())).Throws<Exception>();

            var myResult = theController.SendMessage(theMessage, theSendToUser) as ViewResult;

            AssertAuthenticatedErrorLogReturnBack(myResult, "SendMessage");
            Assert.IsInstanceOfType(myResult.ViewData.Model, typeof(User));
        }

        #endregion
        */
        /*
        #region "View Message - NonPostBack"

        [TestMethod]
        public void TestViewMessageNonPostBack() {
            theMockedService.Setup(s => s.AllowedToViewMessageThread(It.IsAny<User>(), It.IsAny<int>())).Returns(() => true);

            var result = theController.ViewMessage(theMessage.Id) as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(result, "ViewMessage", theViewMessageModel);
        }

        [TestMethod]
        public void TestViewMessageNonPostBack_NotAllowedToViewMessage() {
            theMockedService.Setup(s => s.AllowedToViewMessageThread(It.IsAny<User>(), It.IsAny<int>())).Returns(() => false);

            var result = theController.ViewMessage(theMessage.Id) as ViewResult;

            AssertAuthenticatedRedirection(result);
        }

        [TestMethod]
        public void TestViewMessageNonPostBack_Exception() {
            theMockedService.Setup(s => s.AllowedToViewMessageThread(It.IsAny<User>(), It.IsAny<int>())).Returns(() => true);
            theMockedService.Setup(s => s.GetMessage(It.IsAny<int>(), It.IsAny<User>())).Throws<Exception>();

            var result = theController.ViewMessage(theMessage.Id) as ViewResult;

            AssertAuthenticatedErrorLogWithRedirect(result);
        }

        #endregion

        #region "View Message - After Post Back and Posting a Reply"

        [TestMethod]
        public void TestViewMessageAndPostReply() {
            theMockedService.Setup(s => s.CreateReply(It.IsAny<int>(), It.IsAny<User>(), It.IsAny<string>())).Returns(() => true);

            var result = theController.ViewMessage(theViewMessageModel) as ViewResult;

            AssertAuthenticatedRedirection(result);
        }

        [TestMethod]
        public void TestViewMessageAndPostReply_Fail() {
            theMockedService.Setup(s => s.CreateReply(It.IsAny<int>(), It.IsAny<User>(), It.IsAny<string>())).Returns(() => false);

            var result = theController.ViewMessage(theViewMessageModel) as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(result, "ViewMessage", theViewMessageModel);
            Assert.AreEqual(theViewMessageModel.Message, ((ViewMessageModel)result.ViewData.Model).Message);
            Assert.AreEqual(theViewMessageModel.Reply, ((ViewMessageModel)result.ViewData.Model).Reply);
        }

        [TestMethod]
        public void TestViewMessageAndPostReply_Exception() {
            theMockedService.Setup(s => s.CreateReply(It.IsAny<int>(), It.IsAny<User>(), It.IsAny<string>())).Throws<Exception>();

            var myResult = theController.ViewMessage(theViewMessageModel) as ViewResult;


            AssertAuthenticatedErrorLogReturnBack(myResult, "ViewMessage");
            Assert.IsInstanceOfType(myResult.ViewData.Model, typeof(ViewMessageModel));
            Assert.AreEqual(theViewMessageModel.Message, ((ViewMessageModel)myResult.ViewData.Model).Message);
            Assert.AreEqual(theViewMessageModel.Reply, ((ViewMessageModel)myResult.ViewData.Model).Reply);
        }

        #endregion
        */
        protected override Controller GetController() {
            return theController;
        }
    }
}
