using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Moq;
using HaveAVoice.Validation;
using HaveAVoice.Repositories;
using HaveAVoice.Models;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services;


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
            theUserRepo = new Mock<IHAVProfileRepository>();
            theBaseRepository = new Mock<IHAVBaseRepository>();
            theMockedBaseService = new Mock<IHAVBaseService>();

            theAuthService = new HAVProfileService(new ModelStateWrapper(theModelState),
                                                               theUserRepo.Object,
                                                               theBaseRepository.Object);

        }
        
        [TestMethod]
        public void TestPostToBoardSuccess() {
            bool myResult = theAuthService.PostToBoard(theUser, theViewingUser, "Hey how are you?");
            theUserRepo.Verify(r => r.AddToBoard(It.IsAny<User>(), It.IsAny<User>(), It.IsAny<string>()), Times.Exactly(1));
            Assert.IsTrue(myResult);
        }

        [TestMethod]
        public void TestPostToBoardRequiredMessage() {
            bool myResult = theAuthService.PostToBoard(theUser, theViewingUser, string.Empty);
            theUserRepo.Verify(r => r.AddToBoard(It.IsAny<User>(), It.IsAny<User>(), It.IsAny<string>()), Times.Never());
            Assert.IsFalse(myResult);
            var myError = theModelState["BoardMessage"].Errors[0];
            Assert.AreEqual("You must enter text to post on this user's board.", myError.ErrorMessage);
        }*/
    }
}
