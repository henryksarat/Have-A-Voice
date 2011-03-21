
namespace HaveAVoice.Tests.Helpers {
    /*
    [TestClass]
    public class HAVUserInformationTest {
        private static int RESTRICTION_ID = 5;
        private static Permission PERMISSION1 = Permission.CreatePermission(4, "Test", "default", false);
        private static Permission PERMISSION2 = Permission.CreatePermission(9, "Test", "default", false);

        private Mock<HttpContextBase> theContextBase;
        private IUserInformation theUserInformation;
        private User theUser;
        private List<Permission> thePermissions;
        private Restriction theRestriction;
        private Mock<IHAVWhoIsOnlineService> theMockedService;
        private UserInformationModelBuilder theUserInformationModelBuilder;
        private UserPrivacySetting theUserPrivacySettings;

        [TestInitialize]
        public void Initialize() {
            theContextBase = new Mock<HttpContextBase>();
            theMockedService = new Mock<IHAVWhoIsOnlineService>();
            theUserInformation = UserInformation.Instance(theContextBase.Object, theMockedService.Object);
            theUser = new User();
            thePermissions = new List<Permission>();
            thePermissions.Add(PERMISSION1);
            thePermissions.Add(PERMISSION2);
            theRestriction = new RestrictionModel.Builder(RESTRICTION_ID).Build().Restriction;


            theUserInformationModelBuilder = new UserInformationModelBuilder(theUser)
                .AddPermissions(thePermissions)
                .SetRestriction(theRestriction);

            //HAVUserInformationFactory.SetInstance(theUserInformation);
        }
        /*
        [TestMethod]
        public void ShouldBeAbleToPerformAction() {
            theContextBase.Setup(c => c.Session["UserInformation"]).Returns(() => theUserInformationModelBuilder.Build());

            bool hasPermission = HAVUserInformationFactory.AllowedToPerformAction(HAVPermission.Test);
            Assert.IsTrue(hasPermission);
            hasPermission = HAVUserInformationFactory.AllowedToPerformAction(HAVPermission.Delete_Any_Issue);
            Assert.IsFalse(hasPermission);
        }

        [TestMethod]
        public void TestWhoIsOnline_LoggedIn() {
            theContextBase.Setup(c => c.Request.UserHostAddress).Returns(() => "127.0.0.1");
            theContextBase.Setup(c => c.Session["UserInformation"]).Returns(() => theUserInformationModelBuilder.Build());
            theMockedService.Setup(s => s.IsOnline(It.IsAny<User>(), It.IsAny<string>())).Returns(() => true);

            HAVUserInformationFactory.SetInstance(theUserInformation);
            UserInformationModel user = HAVUserInformationFactory.GetUserInformation();

            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void TestWhoIsOnline_NotLoggedIn() {
            theContextBase.Setup(c => c.Request.UserHostAddress).Returns(() => "127.0.0.1");
            theContextBase.Setup(c => c.Session["UserInformation"]).Returns(() => theUserInformationModelBuilder.Build());
            theMockedService.Setup(s => s.IsOnline(It.IsAny<User>(), It.IsAny<string>())).Returns(() => false);

            HAVUserInformationFactory.SetInstance(theUserInformation);
            UserInformationModel user = HAVUserInformationFactory.GetUserInformation();

            Assert.IsNull(user);
        }
    }*/
}
