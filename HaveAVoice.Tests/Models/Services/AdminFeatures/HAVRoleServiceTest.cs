using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace HaveAVoice.Tests.Models.Services.AdminFeatures {
    [TestClass]
    public class HAVRoleServiceTest {
        /*
        private static string ROLE_NAME = "Admin";
        private static string ROLE_DESCRIPTION = "Admin Role.";
        private static bool DEFAULT_ROLE = false;
        private static int SELECTED_RESTRICTION_ID = 45;
        private static User USER = new User();

        public UserInformationModelBuilder theUserInformationBuilder = new UserInformationModelBuilder(USER);
        private ModelStateDictionary theModelState;
        private IHAVRoleService theService;
        private Mock<IHAVRoleRepository> theMockRepository;
        private Mock<IHAVBaseRepository> theBaseRepository;
        private Mock<IHAVBaseService> theMockedBaseService;

        private static List<int> SELECTED_PERMISSIONS = new List<int>();


        [TestInitialize]
        public void Initialize() {
            theModelState = new ModelStateDictionary();
            theMockRepository = new Mock<IHAVRoleRepository>();
            theBaseRepository = new Mock<IHAVBaseRepository>();
            theMockedBaseService = new Mock<IHAVBaseService>();

            theService = new HAVRoleService(new ModelStateWrapper(theModelState),
                                                               theMockRepository.Object,
                                                               theBaseRepository.Object);
        }

        [TestMethod]
        public void ShouldCreateRole() {
            Permission myPermission = Permission.CreatePermission(0, HAVPermission.Create_Role.ToString(), string.Empty, false);
            List<Permission> myPermissions = new List<Permission>();
            myPermissions.Add(myPermission);
            theUserInformationBuilder.AddPermissions(myPermissions);

            Role role = Role.CreateRole(0, ROLE_NAME, ROLE_DESCRIPTION, DEFAULT_ROLE, 0, false);
            bool result = theService.Create(theUserInformationBuilder.Build(), role, SELECTED_PERMISSIONS, SELECTED_RESTRICTION_ID);

            theMockRepository.Verify(r => r.Create(It.IsAny<User>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>()), Times.Exactly(1));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestCreateRole_RequiredName() {
            Role myRole = Role.CreateRole(0, string.Empty, ROLE_DESCRIPTION, DEFAULT_ROLE, 0, false);
            bool myResult = theService.Create(theUserInformationBuilder.Build(), myRole, SELECTED_PERMISSIONS, SELECTED_RESTRICTION_ID);

            theMockRepository.Verify(r => r.Create(It.IsAny<User>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>()), Times.Never());
            Assert.IsFalse(myResult);
            var myError = theModelState["Name"].Errors[0];
            Assert.AreEqual("Role name is required.", myError.ErrorMessage);
        }

        [TestMethod]
        public void TestCreateRole_RequiredDescriptione() {
            Role myRole = Role.CreateRole(0, ROLE_NAME, string.Empty, DEFAULT_ROLE, 0, false);

            bool myResult = theService.Create(theUserInformationBuilder.Build(), myRole, SELECTED_PERMISSIONS, SELECTED_RESTRICTION_ID);

            theMockRepository.Verify(r => r.Create(It.IsAny<User>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>()), Times.Never());
            Assert.IsFalse(myResult);
            var myError = theModelState["Description"].Errors[0];
            Assert.AreEqual("Role description is required.", myError.ErrorMessage);
        }

        [TestMethod]
        public void TestCreateRole_RequiredRestriction() {
            Role myRole = Role.CreateRole(0, ROLE_NAME, ROLE_DESCRIPTION, DEFAULT_ROLE, 0, false);

            bool myResult = theService.Create(theUserInformationBuilder.Build(), myRole, SELECTED_PERMISSIONS, 0);

            theMockRepository.Verify(r => r.Create(It.IsAny<User>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>()), Times.Never());
            Assert.IsFalse(myResult);
            var myError = theModelState["Restriction"].Errors[0];
            Assert.AreEqual("Please select a restriction.", myError.ErrorMessage);
        }*/
    }
}
