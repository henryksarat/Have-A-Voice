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
        private Mock<IHAVMessageRepository> theMockRepository;
        private Mock<IHAVUserRetrievalService> theUserRetrievalService;
        private MessageController theController;
        private static List<Int32> theSelectedMessages;

        [TestInitialize]
        public void Initialize() {
            theMockedService = new Mock<IHAVMessageService>();
            theMockRepository = new Mock<IHAVMessageRepository>();
            theUserRetrievalService = new Mock<IHAVUserRetrievalService>();

            theService = new HAVMessageService(new ModelStateWrapper(theModelState),
                                                                   theMockRepository.Object, theBaseRepository.Object);

            theController = new MessageController(theMockedBaseService.Object, theMockedService.Object, theUserRetrievalService.Object);
            theController.ControllerContext = GetControllerContext();

            theSelectedMessages = new List<Int32>();
            theLoggedInUser = new User();
            theSendToUser = new User();
            theMessage = new Message();
            theMessage.Id = 55;
            theViewMessageModel = new ViewMessageModel(theMessage);
        }

         protected override Controller GetController() {
            return theController;
        }
    }
}
