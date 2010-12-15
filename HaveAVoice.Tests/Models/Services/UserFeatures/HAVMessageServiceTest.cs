using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Moq;
using HaveAVoice.Repositories;
using HaveAVoice.Services;
using HaveAVoice.Models.Validation;
using HaveAVoice.Models;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Repositories.UserFeatures;

namespace HaveAVoice.Tests.Models.Services.UserFeatures {
    [TestClass]
    public class HAVMessageServiceTest {
        private static string MESSAGE_SUBJECT = "Hey what's up!";
        private static string MESSAGE_BODY = "Chikawawa";
        private static DateTime MESSAGE_DATE = new DateTime(2010, 05, 03);
        private static bool TO_VIEWED = false;
        private static bool FROM_VIEWED = false;
        private static bool TO_DELETED = false;
        private static bool FROM_DELETED = false;
        private static bool REPLIED_TO = false;
        private static int TO_USER_ID = 45;
        private static int FROM_USER_ID = 65;
        private static int MESSAGE_ID = 67;
        private static User USER = new User();
        private static string MESSAGE_REPLY = "Just replying back.";

        private ModelStateDictionary theModelState;
        private IHAVMessageService theService;
        private Mock<IHAVMessageRepository> theMockRepository;
        private Mock<IHAVBaseRepository> theBaseRepository;
        private Mock<IHAVBaseService> theMockedBaseService;


        [TestInitialize]
        public void Initialize() {
            theModelState = new ModelStateDictionary();
            theMockRepository = new Mock<IHAVMessageRepository>();
            theBaseRepository = new Mock<IHAVBaseRepository>();
            theMockedBaseService = new Mock<IHAVBaseService>();

            theService = new HAVMessageService(new ModelStateWrapper(theModelState),
                                                                   theMockRepository.Object, theBaseRepository.Object);

        }

        [TestMethod]
        public void CreateValidMessage() {
            Message message = Message.CreateMessage(0, MESSAGE_SUBJECT, MESSAGE_BODY,
                MESSAGE_DATE, TO_VIEWED, FROM_VIEWED, TO_DELETED, FROM_DELETED, REPLIED_TO);
            bool result = theService.CreateMessage(TO_USER_ID, FROM_USER_ID, message);
            theMockRepository.Verify(r => r.CreateMessage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Message>()), Times.Exactly(1));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CreateValidMessage_RequiredSubject() {
            Message message = Message.CreateMessage(0, string.Empty, MESSAGE_BODY,
                MESSAGE_DATE, TO_VIEWED, FROM_VIEWED, TO_DELETED, FROM_DELETED, REPLIED_TO);

            bool result = theService.CreateMessage(TO_USER_ID, FROM_USER_ID, message);

            theMockRepository.Verify(r => r.CreateMessage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Message>()), Times.Never());
            Assert.IsFalse(result);
            var error = theModelState["Subject"].Errors[0];
            Assert.AreEqual("Subject is required.", error.ErrorMessage);
        }

        [TestMethod]
        public void CreateValidMessage_RequiredBody() {
            Message message = Message.CreateMessage(0, MESSAGE_SUBJECT, string.Empty,
                MESSAGE_DATE, TO_VIEWED, FROM_VIEWED, TO_DELETED, FROM_DELETED, REPLIED_TO);

            bool result = theService.CreateMessage(TO_USER_ID, FROM_USER_ID, message);

            theMockRepository.Verify(r => r.CreateMessage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Message>()), Times.Never());
            Assert.IsFalse(result);
            var error = theModelState["Body"].Errors[0];
            Assert.AreEqual("Body is required.", error.ErrorMessage);
        }

        [TestMethod]
        public void CreateValidMessageReply() {
            theMockRepository.Setup(r => r.CreateReply(It.IsAny<int>(), It.IsAny<User>(), It.IsAny<string>())).Returns(() => new Message());
            
            bool result = theService.CreateReply(MESSAGE_ID, USER, MESSAGE_REPLY);
           
            theMockRepository.Verify(r => r.CreateReply(It.IsAny<int>(), It.IsAny<User>(), It.IsAny<string>()), Times.Exactly(1));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CreateValidMessageReply_ReplyRequired() {
            theMockRepository.Setup(r => r.CreateReply(It.IsAny<int>(), It.IsAny<User>(), It.IsAny<string>())).Returns(() => new Message());
            
            bool result = theService.CreateReply(MESSAGE_ID, USER, string.Empty);

            theMockRepository.Verify(r => r.CreateReply(It.IsAny<int>(), It.IsAny<User>(), It.IsAny<string>()), Times.Never());
            Assert.IsFalse(result);
            var error = theModelState["Reply"].Errors[0];
            Assert.AreEqual("Reply is required.", error.ErrorMessage);
        }
    }
}
