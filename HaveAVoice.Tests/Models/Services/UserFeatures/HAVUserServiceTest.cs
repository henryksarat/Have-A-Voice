using System;
using System.Web.Mvc;
using HaveAVoice.Exceptions;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web;
using HaveAVoice.Repositories.AdminFeatures;
using System.Collections.Generic;
using HaveAVoice.Models.View.Builders;

namespace HaveAVoice.Tests.Models.Services.UserFeatures {
    [TestClass]
    public class HAVUserServiceTest {
        private static string FIRST_NAME = "Henryk";
        private static string EMAIL = "henryksarat@yahoo.com";
        private static string PASSWORD = "aPassword";
        private static bool AGREEMENT = true;
        private static bool CAPTCHA_VALID = true;
        private static string IP_ADDRESS = "192.0.0.1";
        private static string FORGOT_PASSWORD_HASH = "KHBK*WY^DDBSUADGBUAAIDB";

        private ModelStateDictionary theModelState;
        private IHAVUserService theService;
        private Mock<IHAVUserService> theMockedService;
        private Mock<IHAVUserRepository> theUserRepo;
        private Mock<IHAVEmail> theMockedEmailService;
        private Mock<IUserInformation> theMockUserInformation;
        private Mock<IHAVBaseRepository> theBaseRepository;
        private Mock<IHAVBaseService> theBaseService;
        private Mock<IHAVAuthenticationService> theAuthService;
        private Mock<IHAVUserPictureService> theUserPictureService;
        private User theUser;
        private CreateUserModelBuilder theModelBuilder;
        private EditUserModel theEditUserModel;

        [TestInitialize]
        public void Initialize() {
            theModelState = new ModelStateDictionary();
            theAuthService = new Mock<IHAVAuthenticationService>();
            theUserPictureService = new Mock<IHAVUserPictureService>();
            theMockedService = new Mock<IHAVUserService>();
            theUserRepo = new Mock<IHAVUserRepository>();
            theMockedEmailService = new Mock<IHAVEmail>();
            theBaseRepository = new Mock<IHAVBaseRepository>();
            theBaseService = new Mock<IHAVBaseService>();

            theUserRepo.Setup(r => r.CreateUser(It.IsAny<User>())).Returns(() => theUser);
            theMockedEmailService.Setup(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            theService = new HAVUserService(new ModelStateWrapper(theModelState), theAuthService.Object, theUserPictureService.Object, theUserRepo.Object, theMockedEmailService.Object, theBaseRepository.Object);

            theMockUserInformation = new Mock<IUserInformation>();
            theMockUserInformation.Setup(f => f.GetUserInformaton()).Returns(() => new UserInformationModelBuilder(new User()).Build());

            theModelBuilder = DatamodelFactory.createUserModelBuilder();

            theUser = theModelBuilder.Build();
        }

        [TestMethod]
        public void CreateValidUser() {
            bool myResult = theService.CreateUser(theModelBuilder.Build(), CAPTCHA_VALID, AGREEMENT, IP_ADDRESS);

            Assert.IsTrue(myResult);
        }

        [TestMethod]
        public void CreateUserRequiredFirstName() {
            theModelBuilder.FullName(string.Empty);

            bool myResult = theService.CreateUser(theModelBuilder.Build(), CAPTCHA_VALID, AGREEMENT, IP_ADDRESS);

            Assert.IsFalse(myResult);
            var myError = theModelState["FirstName"].Errors[0];
            Assert.AreEqual("First name is required.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateUserRequiredLastName() {
            theModelBuilder.FullName(FIRST_NAME);

            bool myResult = theService.CreateUser(theModelBuilder.Build(), CAPTCHA_VALID, AGREEMENT, IP_ADDRESS);

            Assert.IsFalse(myResult);
            var myError = theModelState["LastName"].Errors[0];
            Assert.AreEqual("Last name is required.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateUserRequiredEmail() {
            theModelBuilder.Email(string.Empty);

            bool myResult = theService.CreateUser(theModelBuilder.Build(), CAPTCHA_VALID, AGREEMENT, IP_ADDRESS);

            Assert.IsFalse(myResult);
            var myError = theModelState["Email"].Errors[0];
            Assert.AreEqual("E-mail is required.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateUserRequiredUsername() {
            theModelBuilder.Username(string.Empty);

            bool myResult = theService.CreateUser(theModelBuilder.Build(), CAPTCHA_VALID, AGREEMENT, IP_ADDRESS);

            Assert.IsFalse(myResult);
            var myError = theModelState["Username"].Errors[0];
            Assert.AreEqual("Username is required.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateUserRequiredPassword() {
            theModelBuilder.Password(string.Empty);

            bool myResult = theService.CreateUser(theModelBuilder.Build(), CAPTCHA_VALID, AGREEMENT, IP_ADDRESS);

            Assert.IsFalse(myResult);
            var myError = theModelState["Password"].Errors[0];
            Assert.AreEqual("Password is required.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateUserRequiredOver18() {
            DateTime myBirthday = new DateTime(1995, 02, 03);
            theModelBuilder.DateOfBirth(myBirthday);

            bool myResult = theService.CreateUser(theModelBuilder.Build(), CAPTCHA_VALID, AGREEMENT, IP_ADDRESS);

            Assert.IsFalse(myResult);
            var myError = theModelState["DateOfBirth"].Errors[0];
            Assert.AreEqual("You must be at least 18 years old.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateUserRequiredAgreement() {
            theModelBuilder.Agreement(false);

            bool myResult = theService.CreateUser(theModelBuilder.Build(), CAPTCHA_VALID, false, IP_ADDRESS);

            Assert.IsFalse(myResult);
            var myError = theModelState["Agreement"].Errors[0];
            Assert.AreEqual("You must agree to the terms.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateUser_UsernameTaken() {
            theUserRepo.Setup(r => r.UsernameRegistered(It.IsAny<string>())).Returns(() => true);

            bool myResult = theService.CreateUser(theModelBuilder.Build(), CAPTCHA_VALID, AGREEMENT, IP_ADDRESS);

            Assert.IsFalse(myResult);
            var myError = theModelState["Username"].Errors[0];
            Assert.AreEqual("Someone already registered with that aUsername. Please try another one.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateUser_EmailTaken() {
            theUserRepo.Setup(r => r.EmailRegistered(It.IsAny<string>())).Returns(() => true);

            bool myResult = theService.CreateUser(theModelBuilder.Build(), CAPTCHA_VALID, AGREEMENT, IP_ADDRESS);

            Assert.IsFalse(myResult);
            var myError = theModelState["Email"].Errors[0];
            Assert.AreEqual("Someone already registered with that myException-mail. Please try another one.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateUser_UsernameAndEmailTaken() {
            theUserRepo.Setup(r => r.UsernameRegistered(It.IsAny<string>())).Returns(() => true);
            theUserRepo.Setup(r => r.EmailRegistered(It.IsAny<string>())).Returns(() => true);

            bool myResult = theService.CreateUser(theModelBuilder.Build(), CAPTCHA_VALID, AGREEMENT, IP_ADDRESS);

            Assert.IsFalse(myResult);
            var usernameError = theModelState["Username"].Errors[0];
            var emailError = theModelState["Email"].Errors[0];
            Assert.AreEqual("Someone already registered with that aUsername. Please try another one.", usernameError.ErrorMessage);
            Assert.AreEqual("Someone already registered with that myException-mail. Please try another one.", emailError.ErrorMessage);
        }

        [TestMethod]
        public void EditUser() {
            Mock<HttpPostedFileBase> myUploadedFile = new Mock<HttpPostedFileBase>();

            theEditUserModel = new EditUserModel.Builder(theUser)
                .setImageFile(myUploadedFile.Object)
                .Build();
            bool myResult = theService.EditUser(theEditUserModel);

            Assert.IsTrue(myResult);
        }

        [TestMethod]
        public void EditUserNewPasswordRequiredRetypedPassword() {
            Mock<HttpPostedFileBase> myUploadedFile = new Mock<HttpPostedFileBase>();

            theEditUserModel = new EditUserModel.Builder(theUser)
                .setNewPassword("newpass")
                .setImageFile(myUploadedFile.Object)
                .Build();
            bool myResult = theService.EditUser(theEditUserModel);

            Assert.IsFalse(myResult);
            var myError = theModelState["RetypedPassword"].Errors[0];
            Assert.AreEqual("Please type your password again.", myError.ErrorMessage);
        }

        [TestMethod]
        public void EditUserNewPasswordAndRetpedPassNoMatch() {
            Mock<HttpPostedFileBase> myUploadedFile = new Mock<HttpPostedFileBase>();

            theEditUserModel = new EditUserModel.Builder(theUser)
                .setNewPassword("newpass")
                .setRetypedPassword("wrongpass")
                .setImageFile(myUploadedFile.Object)
                .Build();
            bool myResult = theService.EditUser(theEditUserModel);

            Assert.IsFalse(myResult);
            var myError = theModelState["RetypedPassword"].Errors[0];
            Assert.AreEqual("Passwords must match.", myError.ErrorMessage);
        }

        [TestMethod]
        public void EditUserUsernameAndEmailTaken() {
            theUserRepo.Setup(r => r.UsernameRegistered(It.IsAny<string>())).Returns(() => true);
            theUserRepo.Setup(r => r.EmailRegistered(It.IsAny<string>())).Returns(() => true);

            Mock<HttpPostedFileBase> myUploadedFile = new Mock<HttpPostedFileBase>();

            theEditUserModel = new EditUserModel.Builder(theUser)
                .setNewPassword("newpass")
                .setRetypedPassword("wrongpass")
                .setOriginalEmail("AnotherEmail")
                .setOriginalUsername("AnotherUsername")
                .setImageFile(myUploadedFile.Object)
                .Build();
            bool myResult = theService.EditUser(theEditUserModel);

            Assert.IsFalse(myResult);
            var usernameError = theModelState["Username"].Errors[0];
            var emailError = theModelState["Email"].Errors[0];
            Assert.AreEqual("Someone already registered with that aUsername. Please try another one.", usernameError.ErrorMessage);
            Assert.AreEqual("Someone already registered with that myException-mail. Please try another one.", emailError.ErrorMessage);
        }

        [TestMethod]
        public void EditUserRequiredCity() {
            Mock<HttpPostedFileBase> myUploadedFile = new Mock<HttpPostedFileBase>();

            theUser.City = string.Empty;
            theEditUserModel = new EditUserModel.Builder(theUser)
                .setImageFile(myUploadedFile.Object)
                .Build();
            bool myResult = theService.EditUser(theEditUserModel);

            Assert.IsFalse(myResult);
            var myError = theModelState["City"].Errors[0];
            Assert.AreEqual("City is required.", myError.ErrorMessage);
        }

        [TestMethod]
        public void EditUserRequiredState() {
            Mock<HttpPostedFileBase> myUploadedFile = new Mock<HttpPostedFileBase>();

            theUser.State = string.Empty;
            theEditUserModel = new EditUserModel.Builder(theUser)
                .setImageFile(myUploadedFile.Object)
                .Build();
            bool myResult = theService.EditUser(theEditUserModel);

            Assert.IsFalse(myResult);
            var myError = theModelState["State"].Errors[0];
            Assert.AreEqual("State is required.", myError.ErrorMessage);
        }
        /*
        [TestMethod]
        public void TestCreateUserCantSendEmail() {
            theMockedEmailService.Setup(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();
            theUserPictureService = new HAVUserService(new ModelStateWrapper(theModelState), theUserRepo.Object, theMockRoleRepo.Object,
                                                               theMockedEmailService.Object, theBaseRepository.Object);
            theUserRepo.Setup(r => r.FindUserByActivationCode(It.IsAny<string>())).Returns(() => theUser);
            theMockRoleRepo.Setup(r => r.GetNotConfirmedUserRole()).Returns(() => new Role());
            theMockRoleRepo.Setup(r => r.GetDefaultRole()).Returns(() => new Role());
            bool myEmailException = false;
            try {
                bool myResult = theUserPictureService.CreateUser(theModelBuilder.Build(), CAPTCHA_VALID, AGREEMENT, IP_ADDRESS);
            } catch (EmailException) {
                myEmailException = true;
                theMockedEmailService.Verify(email => email.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
                theMockRoleRepo.Verify(r => r.GetNotConfirmedUserRole(), Times.AtLeastOnce());
                theMockRoleRepo.Verify(r => r.GetDefaultRole(), Times.AtLeastOnce());
                theUserRepo.Verify(r => r.RemoveUserFromRole(It.IsAny<User>(), It.IsAny<Role>()), Times.AtLeastOnce());
                theUserRepo.Verify(r => r.AddUserToRole(It.IsAny<User>(), It.IsAny<Role>()), Times.AtLeastOnce());
                theUserRepo.Verify(r => r.AddDefaultUserPrivacySettings(It.IsAny<User>()), Times.AtLeastOnce());
                theUserRepo.Verify(r => r.DeleteUser(It.IsAny<User>()), Times.Never());
            }

            Assert.IsTrue(myEmailException);
        }
         

        [TestMethod]
        public void TestCreateAccount_EmailExceptionAndActivationException() {
            theMockedEmailService.Setup(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();
            theUserRepo.Setup(r => r.FindUserByActivationCode(It.IsAny<string>())).Throws<Exception>();

            bool emailExceptionThrown = false;
            bool exceptionThrown = false;
            try {
                var result = theUserPictureService.CreateUser(theModelBuilder.Build(), CAPTCHA_VALID, AGREEMENT, IP_ADDRESS);
            } catch (EmailException) {
                emailExceptionThrown = true;
            } catch (Exception) {
                exceptionThrown = true;
            }

            theUserRepo.Verify(r => r.DeleteUser(It.IsAny<User>()), Times.Exactly(1));
            Assert.AreEqual(false, emailExceptionThrown);
            Assert.AreEqual(true, exceptionThrown);
        }
        * */
        /*
        [TestMethod]
        public void ShouldActivateAccount() {
            theUserRepo.Setup(r => r.FindUserByActivationCode(It.IsAny<string>())).Returns(() => new User());
            theMockRoleRepo.Setup(r => r.GetNotConfirmedUserRole()).Returns(() => new Role());
            theMockRoleRepo.Setup(r => r.GetDefaultRole()).Returns(() => new Role());
            theUserRepo.Setup(r => r.GetRestrictionsForUser(It.IsAny<User>())).Returns(() => new Restriction());
            theUserRepo.Setup(r => r.GetUserPrivacySettingsForUser(It.IsAny<User>())).Returns(() => new UserPrivacySetting());

            UserInformationModel myResult = theUserPictureService.ActivateNewUser(string.Empty);

            Assert.IsNotNull(myResult);
  
        }
        */


        /*
        [TestMethod]
        public void TestWhoIsOnline_IsOnline() {
            WhoIsOnline onlineEntry = WhoIsOnline.CreateWhoIsOnline(0, DateTime.UtcNow, IP_ADDRESS, false);
            theUserRepo.Setup(r => r.GetWhoIsOnlineEntry(It.IsAny<User>(), It.IsAny<string>())).Returns(() => onlineEntry);

            var result = theUserPictureService.IsOnline(theUser, IP_ADDRESS);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestWhoIsOnline_Timeout() {
            DateTime expirtyTime = DateTime.UtcNow.AddSeconds(-1 * HAVConstants.SECONDS_BEFORE_USER_TIMEOUT - 1);
            WhoIsOnline onlineEntry = WhoIsOnline.CreateWhoIsOnline(0, expirtyTime, IP_ADDRESS, false);
            theUserRepo.Setup(r => r.GetWhoIsOnlineEntry(It.IsAny<User>(), It.IsAny<string>())).Returns(() => onlineEntry);

            var result = theUserPictureService.IsOnline(theUser, IP_ADDRESS);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestWhoIsOnline_ForceLogout() {
            WhoIsOnline onlineEntry = WhoIsOnline.CreateWhoIsOnline(0, DateTime.UtcNow, IP_ADDRESS, true);
            theUserRepo.Setup(r => r.GetWhoIsOnlineEntry(It.IsAny<User>(), It.IsAny<string>())).Returns(() => onlineEntry);

            var result = theUserPictureService.IsOnline(theUser, IP_ADDRESS);

            Assert.IsFalse(result);
        }
        
        [TestMethod]
        public void TestForgotPasswordProcess_NotExpiredHash() {
            theUser.ForgotPasswordHashDateTimeStamp = DateTime.UtcNow.AddDays(-15);
            theUserRepo.Setup(r => r.GetUserByEmailAndForgotPasswordHash(It.IsAny<string>(), It.IsAny<string>())).Returns(() => theUser);
            var myResult = theService.ForgotPasswordProcess(EMAIL, FORGOT_PASSWORD_HASH);
            Assert.AreEqual(true, myResult);
        }

        [TestMethod]
        public void TestForgotPasswordProcess_ExpiredHash() {
            theUser.ForgotPasswordHashDateTimeStamp = DateTime.UtcNow.AddDays(-16);
            theUserRepo.Setup(r => r.GetUserByEmailAndForgotPasswordHash(It.IsAny<string>(), It.IsAny<string>())).Returns(() => theUser);
            var myResult = theService.ForgotPasswordProcess(EMAIL, FORGOT_PASSWORD_HASH);
            Assert.AreEqual(false, myResult);
        }

        [TestMethod]
        public void TestChangePassword_Valid() {
            theUserRepo.Setup(r => r.GetUserByEmailAndForgotPasswordHash(It.IsAny<string>(), It.IsAny<string>())).Returns(() => theUser);

            bool myResult = theService.ChangePassword(EMAIL, FORGOT_PASSWORD_HASH, PASSWORD, PASSWORD);

            Assert.IsTrue(myResult);
        }

        [TestMethod]
        public void TestChangePassword_RequiredPassword() {
            theUserRepo.Setup(r => r.GetUserByEmailAndForgotPasswordHash(It.IsAny<string>(), It.IsAny<string>())).Returns(() => theUser);

            bool myResult = theService.ChangePassword(EMAIL, FORGOT_PASSWORD_HASH, string.Empty, PASSWORD);

            Assert.IsFalse(myResult);
            var myError = theModelState["Password"].Errors[0];
            Assert.AreEqual("Password is required.", myError.ErrorMessage);
        }

        [TestMethod]
        public void TestChangePassword_RequiredRetypedPassword() {
            theUserRepo.Setup(r => r.GetUserByEmailAndForgotPasswordHash(It.IsAny<string>(), It.IsAny<string>())).Returns(() => theUser);

            bool myResult = theService.ChangePassword(EMAIL, FORGOT_PASSWORD_HASH, PASSWORD, string.Empty);

            Assert.IsFalse(myResult);
            var myError = theModelState["RetypedPassword"].Errors[0];
            Assert.AreEqual("Please type your password again.", myError.ErrorMessage);
        }

        [TestMethod]
        public void TestChangePassword_PasswordsDontMatch() {
            theUserRepo.Setup(r => r.GetUserByEmailAndForgotPasswordHash(It.IsAny<string>(), It.IsAny<string>())).Returns(() => theUser);

            bool myResult = theService.ChangePassword(EMAIL, FORGOT_PASSWORD_HASH, PASSWORD, "Another pass");

            Assert.IsFalse(myResult);
            var myError = theModelState["RetypedPassword"].Errors[0];
            Assert.AreEqual("Passwords must match.", myError.ErrorMessage);
        }
         * */


    }
}
