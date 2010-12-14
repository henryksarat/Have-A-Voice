using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HaveAVoice.Controllers.Users;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Models.Repositories.UserFeatures;
using HaveAVoice.Models.Services.UserFeatures;
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

        [TestMethod]
        public void TestLogin_NoMatch() {
            theMockedService.Setup(s => s.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>())).Returns(() => null);
         
            var result = theController.Login(string.Empty, string.Empty, false) as ViewResult;

            Assert.AreEqual("Incorrect aUsername and aPassword combination.", result.ViewData["Message"]);
            Assert.AreEqual("Login", result.ViewName);
        }

        [TestMethod]
        public void TestLoginRememberMe_Success() {
            theMockedService.Setup(s => s.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>())).Returns(() => theUserInformationModel);

            theController.Login(string.Empty, string.Empty, true);

            theMockedService.Setup(s => s.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>())).Returns(() => theUserInformationModel);
            theMockedService.Verify(s => s.CreateRememberMeCredentials(It.IsAny<User>()), Times.Exactly(1));
        }

        [TestMethod]
        public void TestUserListController_NoUsers() {
            theMockedService.Setup(s => s.GetUserList(It.IsAny<User>())).Returns(() => new List<UserDetailsModel>());
           
            var result = theController.UserList() as ViewResult;

            theMockedService.Verify(s => s.GetUserList(It.IsAny<User>()), Times.AtLeastOnce());
            Assert.AreEqual("There are no registered users.", result.ViewData["Message"]);
            Assert.AreEqual("UserList", result.ViewName);
        }

        [TestMethod]
        public void TestUserListController_MoreThanZeroUsers() {
            List<UserDetailsModel> users = new List<UserDetailsModel>();
            users.Add(new UserDetailsModel());
            theMockedService.Setup(s => s.GetUserList(It.IsAny<User>())).Returns(() => users);
         
            var result = theController.UserList() as ViewResult;

            theMockedService.Verify(s => s.GetUserList(It.IsAny<User>()), Times.AtLeastOnce());
            Assert.AreEqual(null, result.ViewData["Message"]);
            Assert.AreEqual("UserList", result.ViewName);
        }

        [TestMethod]
        public void TestUserPictures_NotLoggedIn() {
            theMockUserInformation.Setup(u => u.GetUserInformaton()).Returns(() => null);

            var result = theController.UserPictures() as ViewResult;

            theMockedService.Verify(s => s.GetUserPictures(It.IsAny<int>()), Times.Never());
            theMockedService.Verify(s => s.GetProfilePicture(It.IsAny<int>()), Times.Never());
            AssertRedirection(result);
        }

        [TestMethod]
        public void TestUserPictures_LoggedInUser() {
            theMockedService.Setup(s => s.GetProfilePicture(It.IsAny<int>())).Returns(() => new UserPicture());

            var result = theController.UserPictures() as ViewResult;

            theMockedService.Verify(s => s.GetUserPictures(It.IsAny<int>()), Times.Exactly(1));
            theMockedService.Verify(s => s.GetProfilePicture(It.IsAny<int>()), Times.Exactly(1));
            Assert.AreEqual("UserPictures", result.ViewName);;
        }

        [TestMethod]
        public void TestSetProfilePicture_OneSelected() {
            UserPicture userPicture = new UserPicture();
            IEnumerable<UserPicture> userPictures = new List<UserPicture>();
            List<int> selectedUserPictures = new List<int>();
            selectedUserPictures.Add(1);

            UserPicturesModel userPictureModel = new UserPicturesModel(userPicture, userPictures, selectedUserPictures);

            var result = theController.UserPictures_SetProfilePicture(userPictureModel) as ViewResult;

            theMockedService.Verify(s => s.SetToProfilePicture(It.IsAny<int>(), It.IsAny<User>()), Times.Exactly(1));
        }

        [TestMethod]
        public void TestSetProfilePicture_NoneSelected() {
            UserPicture userPicture = new UserPicture();
            IEnumerable<UserPicture> userPictures = new List<UserPicture>();
            List<int> selectedUserPictures = new List<int>();
            UserPicturesModel userPictureModel = new UserPicturesModel(userPicture, userPictures, selectedUserPictures);

            var result = theController.UserPictures_SetProfilePicture(userPictureModel) as ViewResult;

            theMockedService.Verify(s => s.SetToProfilePicture(It.IsAny<int>(), It.IsAny<User>()), Times.Never());
            Assert.AreEqual("UserPictures", result.ViewName);
            Assert.IsNotNull(result.ViewData["Message"]);
        }

        [TestMethod]
        public void TestSetProfilePicture_MoreThanOneSelected() {
            UserPicture userPicture = new UserPicture();
            IEnumerable<UserPicture> userPictures = new List<UserPicture>();
            List<int> selectedUserPictures = new List<int>();
            selectedUserPictures.Add(1);
            selectedUserPictures.Add(2);

            UserPicturesModel userPictureModel = new UserPicturesModel(userPicture, userPictures, selectedUserPictures);

            var result = theController.UserPictures_SetProfilePicture(userPictureModel) as ViewResult;

            theMockedService.Verify(s => s.SetToProfilePicture(It.IsAny<int>(), It.IsAny<User>()), Times.Never());
            Assert.AreEqual("UserPictures", result.ViewName);
            Assert.IsNotNull(result.ViewData["Message"]);
        }

        [TestMethod]
        public void TestSetProfilePicture_ErrorThrown() {
            UserPicture userPicture = new UserPicture();
            IEnumerable<UserPicture> userPictures = new List<UserPicture>();
            List<int> selectedUserPictures = new List<int>();

            selectedUserPictures.Add(1);

            UserPicturesModel userPictureModel = new UserPicturesModel(userPicture, userPictures, selectedUserPictures);

            theMockedService.Setup(s => s.SetToProfilePicture(It.IsAny<int>(), It.IsAny<User>())).Throws<Exception>();

            var result = theController.UserPictures_SetProfilePicture(userPictureModel) as ViewResult;

            theMockedService.Verify(s => s.SetToProfilePicture(It.IsAny<int>(), It.IsAny<User>()), Times.Exactly(1));
            AssertErrorLogReturnBack("UserPictures", result);
        }

        #region "Activate Account"

        [TestMethod]
        public void TestActivateAccount_NoUserFound() {
            theMockedService.Setup(s => s.ActivateNewUser(It.IsAny<string>())).Throws<NullUserException>();
            var result = theController.ActivateAccount(string.Empty) as ViewResult;

            theMockedBaseService.Verify(r => r.LogError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Never());
            Assert.IsNotNull(result.ViewData["Message"]);
            Assert.AreEqual("ActivateAccount", result.ViewName);
        }

        [TestMethod]
        public void TestActivateAccount_RoleError() {
            theMockedService.Setup(s => s.ActivateNewUser(It.IsAny<string>())).Throws<NullRoleException>();
            var result = theController.ActivateAccount(string.Empty) as ViewResult;

            AssertErrorLogReturnBack("ActivateAccount", result);
        }

        [TestMethod]
        public void TestActivateAccount_UserActivated() {
            theMockedService.Setup(s => s.ActivateNewUser(It.IsAny<string>())).Returns(() => theUserInformationModel);
            var result = theController.ActivateAccount(string.Empty) as ViewResult;
            Assert.IsNull(result.ViewData["Message"]);
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void TestActivateAccount_Exception() {
            theMockedService.Setup(s => s.ActivateNewUser(It.IsAny<string>())).Throws<Exception>();

            var result = theController.ActivateAccount(string.Empty) as ViewResult;

            AssertErrorLogReturnBack("ActivateAccount", result);
        }
                
        #endregion

        [TestMethod]
        public void TestBecomeFan_NotLoggedIn() {
            theMockUserInformation.Setup(u => u.GetUserInformaton()).Returns(() => null);

            var myResult = theController.BecomeAFan(theUser.Id) as ViewResult;

            theMockedService.Verify(s => s.AddFan(It.IsAny<User>(), It.IsAny<int>()), Times.Never());
            AssertRedirection(myResult);
        }

        [TestMethod]
        public void TestBecomeFan() {
            var myResult = theController.BecomeAFan(theUser.Id) as ViewResult;

            theMockedService.Verify(s => s.AddFan(It.IsAny<User>(), It.IsAny<int>()), Times.AtLeastOnce());
            AssertRedirection(myResult);
        }

        [TestMethod]
        public void TestBecomeFan_Exception() {
            theMockedService.Setup(s => s.AddFan(It.IsAny<User>(), It.IsAny<int>())).Throws<Exception>();

            var myResult = theController.BecomeAFan(theUser.Id) as ViewResult;

            theMockedBaseService.Verify(r => r.LogError(It.IsAny<Exception>(), It.IsAny<string>()), Times.AtLeastOnce());
            AssertRedirection(myResult);
        }

        protected override Controller GetController() {
            return theController;
        }
    }
}
