using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HaveAVoice.Controllers.Users;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services.UserFeatures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using HaveAVoice.Exceptions;
using HaveAVoice.Models.View.Builders;

namespace HaveAVoice.Tests.Controllers.Users {
    [TestClass]
    public class UserControllerTest : ControllerTestCase {
        private static UserInformationModel theUserInformationModel;
        private Mock<IHAVUserService> theMockedService;
        private Mock<IHAVUserRepository> theMockRepository;
        private Mock<IHAVEmail> theMockedEmailService;
        private UserController theController;
        private User theUser = new User();
        private CreateUserModelBuilder theCreateUseModelBuilder;
        
        [TestInitialize]
        public void Initialize() {
            theMockedService = new Mock<IHAVUserService>();
            theMockRepository = new Mock<IHAVUserRepository>();
            theMockedEmailService = new Mock<IHAVEmail>();
            theCreateUseModelBuilder = new CreateUserModelBuilder();


            theUser.Email = "henryksarat@aol.com";
            theMockRepository.Setup(r => r.CreateUser(It.IsAny<User>())).Returns(() => theUser);
            theMockedEmailService.Setup(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            theController = new UserController(theMockedService.Object, theMockedBaseService.Object);
            theController.ControllerContext = GetControllerContext();

            theUserInformationModel = new UserInformationModelBuilder(new User()).Build();
        }


        [TestMethod]
        public void TestCreateUser() {
            theMockedService.Setup(s => s.CreateUser(It.IsAny<User>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>())).Returns(true);
            var myResult = theController.Create(theCreateUseModelBuilder, true) as ViewResult;
            AssertRedirection(myResult);
        }

        [TestMethod]
        public void TestCreateUser_EmailException() {
            theMockedService.Setup(s => s.CreateUser(It.IsAny<User>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>())).Throws<EmailException>();
            var myResult = theController.Create(theCreateUseModelBuilder, true) as ViewResult;
            AssertErrorWithRedirect(myResult);
        }

        [TestMethod]
        public void TestCreateUser_Fail() {
            theMockedService.Setup(s => s.CreateUser(It.IsAny<User>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>())).Returns(false);
            var myResult = theController.Create(theCreateUseModelBuilder, true) as ViewResult;

            AssertFailReturnBack(myResult, "Create", theCreateUseModelBuilder);
        }

        [TestMethod]
        public void TestCreateUser_Exception() {
            theMockedService.Setup(s => s.CreateUser(It.IsAny<User>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>())).Throws<Exception>();
            var myResult = theController.Create(theCreateUseModelBuilder, true) as ViewResult;

            AssertErrorLogWithReturn("Create", myResult);
        }
 
        protected override Controller GetController() {
            return theController;
        }
    }
}
