using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HaveAVoice.Tests.Models.Services.UserFeatures {
    [TestClass]
    public class HAVUserServiceTest {
        /*
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
        private Mock<IHAVPhotoService> thePhotoService;
        private Mock<IHAVAuthorityVerificationService> theAuthorityVerification;
        private Mock<IHAVUserRetrievalService> theUserRetrievalService;
        private User theUser;
        private CreateUserModelBuilder theModelBuilder;
        private EditUserModel theEditUserModel;

        [TestInitialize]
        public void Initialize() {
            theModelState = new ModelStateDictionary();
            theAuthService = new Mock<IHAVAuthenticationService>();
            thePhotoService = new Mock<IHAVPhotoService>();
            theMockedService = new Mock<IHAVUserService>();
            theUserRepo = new Mock<IHAVUserRepository>();
            theMockedEmailService = new Mock<IHAVEmail>();
            theBaseRepository = new Mock<IHAVBaseRepository>();
            theBaseService = new Mock<IHAVBaseService>();
            theUserRetrievalService = new Mock<IHAVUserRetrievalService>();
            theAuthorityVerification = new Mock<IHAVAuthorityVerificationService>();

            theUserRepo.Setup(r => r.CreateUser(It.IsAny<User>())).Returns(() => theUser);
            theMockedEmailService.Setup(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            theService = new HAVUserService(new ModelStateWrapper(theModelState), theUserRetrievalService.Object, theAuthorityVerification.Object, theAuthService.Object, thePhotoService.Object, theUserRepo.Object, theMockedEmailService.Object, theBaseRepository.Object);

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
            theModelBuilder.FirstName = string.Empty;

            bool myResult = theService.CreateUser(theModelBuilder.Build(), CAPTCHA_VALID, AGREEMENT, IP_ADDRESS);

            Assert.IsFalse(myResult);
            var myError = theModelState["FirstName"].Errors[0];
            Assert.AreEqual("First name is required.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateUserRequiredLastName() {
            theModelBuilder.LastName = string.Empty;

            bool myResult = theService.CreateUser(theModelBuilder.Build(), CAPTCHA_VALID, AGREEMENT, IP_ADDRESS);

            Assert.IsFalse(myResult);
            var myError = theModelState["LastName"].Errors[0];
            Assert.AreEqual("Last name is required.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateUserRequiredEmail() {
            theModelBuilder.Email = string.Empty;

            bool myResult = theService.CreateUser(theModelBuilder.Build(), CAPTCHA_VALID, AGREEMENT, IP_ADDRESS);

            Assert.IsFalse(myResult);
            var myError = theModelState["Email"].Errors[0];
            Assert.AreEqual("E-mail is required.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateUserRequiredPassword() {
            theModelBuilder.Password = string.Empty;

            bool myResult = theService.CreateUser(theModelBuilder.Build(), CAPTCHA_VALID, AGREEMENT, IP_ADDRESS);

            Assert.IsFalse(myResult);
            var myError = theModelState["Password"].Errors[0];
            Assert.AreEqual("Password is required.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateUserRequiredOver18() {
            DateTime myBirthday = new DateTime(1995, 02, 03);
            theModelBuilder.DateOfBirth = myBirthday;

            bool myResult = theService.CreateUser(theModelBuilder.Build(), CAPTCHA_VALID, AGREEMENT, IP_ADDRESS);

            Assert.IsFalse(myResult);
            var myError = theModelState["DateOfBirth"].Errors[0];
            Assert.AreEqual("You must be at least 18 years old.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateUserRequiredAgreement() {
            theModelBuilder.Agreement = false;

            bool myResult = theService.CreateUser(theModelBuilder.Build(), CAPTCHA_VALID, false, IP_ADDRESS);

            Assert.IsFalse(myResult);
            var myError = theModelState["Agreement"].Errors[0];
            Assert.AreEqual("You must agree to the terms.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateUser_EmailTaken() {
            theUserRepo.Setup(r => r.EmailRegistered(It.IsAny<string>())).Returns(() => true);

            bool myResult = theService.CreateUser(theModelBuilder.Build(), CAPTCHA_VALID, AGREEMENT, IP_ADDRESS);

            Assert.IsFalse(myResult);
            var myError = theModelState["Email"].Errors[0];
            Assert.AreEqual("Someone already registered with that myException-mail. Please try another one.", myError.ErrorMessage);
        }*/
    }
}
