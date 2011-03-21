using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HaveAVoice.Tests.Controllers.Admin {
    [TestClass]
    public class PermissionControllerTest : ControllerTestCase {
        /*
        private static int VALID_PERMISSION_ID = 4;
        private static int INVALID_PERMISSION_ID = 5;
        
        private IHAVRoleService theService;
        private Mock<IHAVPermissionService> thePermissionService;
        private Mock<IHAVRoleRepository> theMockRepository;
        private static PermissionController theController;
        private static Permission thePermission;
        private static PermissionModel thePermissionModel;

        [TestInitialize]
        public void Initialize() {
            thePermission = Permission.CreatePermission(2, "Read Message", "Allows someone to read their messages.", false);
            thePermissionModel = new PermissionModel(thePermission);
            thePermissionService = new Mock<IHAVPermissionService>();
            theMockRepository = new Mock<IHAVRoleRepository>();

            theService = new HAVRoleService(new ModelStateWrapper(theModelState),
                                                               theMockRepository.Object,
                                                               theBaseRepository.Object);

            theController = new PermissionController(theMockedBaseService.Object, thePermissionService.Object);
            theController.ControllerContext = GetControllerContext();
        }

        [TestMethod]
        public void ShouldLoadPermissions() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.View_Permissions);
            List<Permission> myPermissions = new List<Permission>();
            myPermissions.Add(thePermission);
            thePermissionService.Setup(s => s.GetAllPermissions()).Returns(() => myPermissions);

            var result = theController.Index() as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(result, "Index", myPermissions);
        }

        [TestMethod]
        public void ShouldNotLoadPermissionsWithoutPermission() {
            var result = theController.Index() as ViewResult;

            thePermissionService.Verify(s => s.GetAllPermissions(), Times.Never());
            AssertAuthenticatedRedirection(result);
        }

        [TestMethod]
        public void ShouldNotLoadPermissionsBecauseOfException() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.View_Permissions);
            thePermissionService.Setup(s => s.GetAllPermissions()).Throws<Exception>();

            var result = theController.Index() as ViewResult;

            thePermissionService.Verify(s => s.GetAllPermissions(), Times.Exactly(1));
            AssertAuthenticatedErrorLogWithRedirect(result);
        }

        [TestMethod]
        public void ShouldLoadPermissionPageButThereAreNoPermissions() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.View_Permissions);
            List<Permission> permissions = new List<Permission>();
            thePermissionService.Setup(s => s.GetAllPermissions()).Returns(() => permissions);

            var result = theController.Index() as ViewResult;

            thePermissionService.Verify(s => s.GetAllPermissions(), Times.Exactly(1));
            AssertAuthenticatedSuccessWithMessage(result, "Index", permissions);
        }

        [TestMethod]
        public void ShouldLoadCreatePermission() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Create_Permission);
            var result = theController.Create() as ViewResult;

            Assert.AreEqual("Create", result.ViewName);
            Assert.IsNull(result.ViewData.Model);
        }

        public void ShouldNotLoadCreatePermissionWithoutPermission() {
            var result = theController.Create() as ViewResult;

            Assert.AreEqual("Create", result.ViewName);
            Assert.IsNull(result.ViewData.Model);
        }


        #region "Create Permission - After Post Back"

        [TestMethod]
        public void TestCreatePermission_Success() {
            thePermissionService.Setup(s => s.Create(It.IsAny<UserInformationModel>(), It.IsAny<Permission>())).Returns(() => true);

            var result = theController.Create(thePermissionModel) as ViewResult;

            AssertAuthenticatedRedirection(result);
        }

        [TestMethod]
        public void TestCreatePermission_Fail() {
            thePermissionService.Setup(s => s.Create(It.IsAny<UserInformationModel>(), It.IsAny<Permission>())).Returns(() => false);

            var result = theController.Create(thePermissionModel) as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(result, "Create", thePermissionModel);
        }

        [TestMethod]
        public void TestCreatePermission_Exception() {
            thePermissionService.Setup(s => s.Create(It.IsAny<UserInformationModel>(), It.IsAny<Permission>())).Throws<Exception>();

            var result = theController.Create(thePermissionModel) as ViewResult;

            AssertAuthenticatedErrorLogReturnBack(result, "Create");
        }

        #endregion

        #region "Edit Permission - NonPostBack"

        [TestMethod]
        public void ShouldLoadEditPermissionPage() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Edit_Permission);
            thePermissionService.Setup(s => s.FindPermission(VALID_PERMISSION_ID)).Returns(() => thePermission);

            var result = theController.Edit(VALID_PERMISSION_ID) as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(result, "Edit", thePermission);
        }

        [TestMethod]
        public void ShouldNotLoadEditPermissionPageWithoutPermission() {
            thePermissionService.Setup(s => s.FindPermission(VALID_PERMISSION_ID)).Returns(() => thePermission);

            var myResult = theController.Edit(VALID_PERMISSION_ID) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldLoadEditPermissionPageButNoPermissionIsFound() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Edit_Permission);
            thePermissionService.Setup(s => s.FindPermission(VALID_PERMISSION_ID)).Returns(() => thePermission);

            var result = theController.Edit(INVALID_PERMISSION_ID) as ViewResult;

            AssertAuthenticatedRedirection(result);
        }

        [TestMethod]
        public void ShouldNotLoadEditPermissionPageBecauseOfException() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Edit_Permission);
            var controller = new PermissionController(theMockedBaseService.Object, thePermissionService.Object);
            thePermissionService.Setup(s => s.FindPermission(VALID_PERMISSION_ID)).Throws<Exception>();
            controller.ControllerContext = GetControllerContext();

            var result = controller.Edit(VALID_PERMISSION_ID) as ViewResult;

            AssertAuthenticatedErrorLogWithRedirect(result);
        }

        #endregion

        #region "Edit Permission - After Post Back"

        [TestMethod]
        public void TestEditPermission_Success() {
            thePermissionService.Setup(s => s.Edit(It.IsAny<UserInformationModel>(), It.IsAny<Permission>())).Returns(() => true);

            var result = theController.Edit(thePermission) as ViewResult;

            AssertAuthenticatedRedirection(result);
        }

        [TestMethod]
        public void TestEditPermission_Exception() {
            thePermissionService.Setup(s => s.Edit(It.IsAny<UserInformationModel>(), It.IsAny<Permission>())).Throws<Exception>();

            var result = theController.Edit(thePermission) as ViewResult;

            AssertAuthenticatedErrorLogReturnBack(result, "Edit");
        }

        [TestMethod]
        public void TestEditPermission_Fail() {
            thePermissionService.Setup(s => s.Edit(It.IsAny<UserInformationModel>(), It.IsAny<Permission>())).Returns(() => false);

            var result = theController.Edit(thePermission) as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(result, "Edit", thePermission);
        }

        #endregion

        #region "Delete Permission - NonPostBack"

        [TestMethod]
        public void ShouldLoadDeletePermission() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Delete_Permission);
            thePermissionService.Setup(s => s.FindPermission(VALID_PERMISSION_ID)).Returns(() => thePermission);

            var result = theController.Delete(VALID_PERMISSION_ID) as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(result, "Delete", thePermission);
        }

        [TestMethod]
        public void ShouldNotLoadDeletePermissionWithoutPermission() {
            thePermissionService.Setup(s => s.FindPermission(VALID_PERMISSION_ID)).Returns(() => thePermission);

            var myResult = theController.Delete(VALID_PERMISSION_ID) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadDeletePermissionBecausePermissionNotFound() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Delete_Permission);
            thePermissionService.Setup(s => s.FindPermission(VALID_PERMISSION_ID)).Returns(() => thePermission);

            var myResult = theController.Delete(INVALID_PERMISSION_ID) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadDeletePermissionBecauseOfException() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Delete_Permission);
            thePermissionService.Setup(s => s.FindPermission(It.IsAny<int>())).Throws<Exception>();

            var myResult = theController.Delete(VALID_PERMISSION_ID) as ViewResult;

            AssertAuthenticatedErrorLogWithRedirect(myResult);
        }

        #endregion

        #region "Delete Role - After Post Back"

        [TestMethod]
        public void ShouldDeletePermission() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Delete_Permission);

            var myResult = theController.Delete(thePermission) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotDeletePermissionWithoutPermission() {
            var myResult = theController.Delete(thePermission) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotDeleteRoleBecauseOfException() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Delete_Role);
            thePermissionService.Setup(s => s.Delete(It.IsAny<UserInformationModel>(), It.IsAny<Permission>())).Throws<Exception>();

            var myResult = theController.Delete(thePermission) as ViewResult;

            AssertAuthenticatedErrorLogWithRedirect(myResult);
        }

        #endregion

        protected override Controller GetController() {
            return theController;
        }*/
    }
}
