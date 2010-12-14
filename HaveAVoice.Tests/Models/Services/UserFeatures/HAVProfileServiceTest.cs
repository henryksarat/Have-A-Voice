using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Moq;
using HaveAVoice.Models.Validation;
using HaveAVoice.Models.Services;
using HaveAVoice.Models.Repositories;
using HaveAVoice.Models;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;
using HaveAVoice.Models.Services.UserFeatures;
using HaveAVoice.Models.Repositories.UserFeatures;


namespace HaveAVoice.Tests.Models.Services.UserFeatures {
    [TestClass]
    public class HAVProfileServiceTest {
        private static User theUser = DatamodelFactory.createUserModelBuilder().Username("henryksarat").Build();
        private static User theViewingUser = DatamodelFactory.createUserModelBuilder().Username("nickL").Build();

        private ModelStateDictionary theModelState;
        private IHAVProfileService theService;
        private Mock<IHAVProfileRepository> theMockRepository;
        private Mock<IHAVBaseRepository> theBaseRepository;
        private Mock<IHAVBaseService> theMockedBaseService;
        /*

        [TestInitialize]
        public void Initialize() {
            theModelState = new ModelStateDictionary();
            theMockRepository = new Mock<IHAVProfileRepository>();
            theBaseRepository = new Mock<IHAVBaseRepository>();
            theMockedBaseService = new Mock<IHAVBaseService>();

            theService = new HAVProfileService(new ModelStateWrapper(theModelState),
                                                               theMockRepository.Object,
                                                               theBaseRepository.Object);

        }
        
        [TestMethod]
        public void TestPostToBoardSuccess() {
            bool myResult = theService.PostToBoard(theUser, theViewingUser, "Hey how are you?");
            theMockRepository.Verify(r => r.AddToBoard(It.IsAny<User>(), It.IsAny<User>(), It.IsAny<string>()), Times.Exactly(1));
            Assert.IsTrue(myResult);
        }

        [TestMethod]
        public void TestPostToBoardRequiredMessage() {
            bool myResult = theService.PostToBoard(theUser, theViewingUser, string.Empty);
            theMockRepository.Verify(r => r.AddToBoard(It.IsAny<User>(), It.IsAny<User>(), It.IsAny<string>()), Times.Never());
            Assert.IsFalse(myResult);
            var myError = theModelState["BoardMessage"].Errors[0];
            Assert.AreEqual("You must enter text to post on this user's board.", myError.ErrorMessage);
        }*/
    }
}
