using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace HaveAVoice.Tests.Models.Services.AdminFeatures {
    [TestClass]
    public class HAVPermissionServiceTest {
        /*
        private static string PERMISSION_NAME = "View Message";
        private static string PERMISSION_DESCRIPTION = "Permission to view a message.";
        private static User USER = new User();
        private static Permission PERMISSION = Permission.CreatePermission(0, PERMISSION_NAME, PERMISSION_DESCRIPTION, false);

        public UserInformationModelBuilder theUserInformationBuilder = new UserInformationModelBuilder(USER);
        private ModelStateDictionary theModelState;
        private IHAVPermissionService thePermissionService;
        private Mock<IHAVPermissionRepository> thePermissionRepo;
        private Mock<IHAVBaseRepository> theBaseRepository;
        private Mock<IHAVBaseService> theMockedBaseService;

        private static List<int> SELECTED_PERMISSIONS = new List<int>();


        [TestInitialize]
        public void Initialize() {
            theModelState = new ModelStateDictionary();
            thePermissionRepo = new Mock<IHAVPermissionRepository>();
            theBaseRepository = new Mock<IHAVBaseRepository>();
            theMockedBaseService = new Mock<IHAVBaseService>();

            thePermissionService = new HAVPermissionService(new ModelStateWrapper(theModelState), theBaseRepository.Object, thePermissionRepo.Object);
        }
        
        [TestMethod]
        public void ShouldCreateValidPermission() {
            Permission myPermission = Permission.CreatePermission(0, HAVPermission.Create_Permission.ToString(), string.Empty, false);
            List<Permission> myPermissions = new List<Permission>();
            myPermissions.Add(myPermission);
            theUserInformationBuilder.AddPermissions(myPermissions);

            bool result = thePermissionService.Create(theUserInformationBuilder.Build(), PERMISSION);

            thePermissionRepo.Verify(r => r.Create(It.IsAny<User>(), It.IsAny<Permission>()), Times.Exactly(1));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UnableToCreatePermissionBecauseOfNoCreatePermission() {
            bool result = thePermissionService.Create(theUserInformationBuilder.Build(), PERMISSION);

            thePermissionRepo.Verify(r => r.Create(It.IsAny<User>(), It.IsAny<Permission>()), Times.Never());
            Assert.IsFalse(result);
            var myError = theModelState["PerformAction"].Errors[0];
            Assert.IsNotNull(myError);
        }

        [TestMethod]
        public void UnableToCreatePermissionBecauseNameIsRequired() {
            Permission myPermission = Permission.CreatePermission(0, string.Empty, PERMISSION_DESCRIPTION, false);

            bool myResult = thePermissionService.Create(theUserInformationBuilder.Build(), myPermission);

            thePermissionRepo.Verify(r => r.Create(It.IsAny<User>(), It.IsAny<Permission>()), Times.Never());
            Assert.IsFalse(myResult);
            var myError = theModelState["Name"].Errors[0];
            Assert.AreEqual("Permission name is required.", myError.ErrorMessage);
        }

        [TestMethod]
        public void TestCreatePermission_RequiredDescription() {
            Permission myPermission = Permission.CreatePermission(0, PERMISSION_NAME, string.Empty, false);

            bool myResult = thePermissionService.Create(theUserInformationBuilder.Build(), myPermission);

            thePermissionRepo.Verify(r => r.Create(It.IsAny<User>(), It.IsAny<Permission>()), Times.Never());
            Assert.IsFalse(myResult);
            var myError = theModelState["Description"].Errors[0];
            Assert.AreEqual("Permission description is required.", myError.ErrorMessage);
        }
         * */
    }
}
