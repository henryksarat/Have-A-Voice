using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HaveAVoice.Tests.Controllers.Admin {
    [TestClass]
    public class RoleControllerTest : ControllerTestCase  {
        /*
        private static int[] SELECTED_PERMISSIONS = new int[] {0};
        private static int[] SELECTED_USERS = new int[] { 0 };
        private static int ROLE_ID = 3;
        private static int FROM_ROLE_ID = 5;
        private static int TO_ROLE_ID = 6;
        private static Role ROLE = Role.CreateRole(0, "Admin", "Admin Role", false, 0, false);
        private static Restriction RESTRICTION = new RestrictionModel.Builder(0).Build().Restriction;
        private static Permission PERMISSION = Permission.CreatePermission(0, string.Empty, string.Empty, false);
        private static SwitchUserRoles theSwitchUserRolesModel;
        
        private List<Restriction> theRestrictions = new List<Restriction>();
        private List<Permission> thePermissions = new List<Permission>();
        private IHAVRoleService theService;
        private Mock<IHAVRoleService> theRoleService;
        private Mock<IHAVPermissionService> thePermissionService;
        private Mock<IHAVRoleRepository> theMockRepository;
        private Mock<IHAVRestrictionService> theRestrictionService;
        private RoleController theController;
        private RoleModel theRoleModel;

        [TestInitialize]
        public void Initialize() {
            theRestrictions.Add(RESTRICTION);
            thePermissions.Add(PERMISSION);

            theRoleService = new Mock<IHAVRoleService>();
            thePermissionService = new Mock<IHAVPermissionService>();
            theMockRepository = new Mock<IHAVRoleRepository>();
            theRestrictionService = new Mock<IHAVRestrictionService>();

            theService = new HAVRoleService(new ModelStateWrapper(theModelState), 
                                                               theMockRepository.Object,
                                                               theBaseRepository.Object);

            theController = new RoleController(theMockedBaseService.Object, theRoleService.Object, thePermissionService.Object, theRestrictionService.Object);
            theController.ControllerContext = GetControllerContext();

            theSwitchUserRolesModel = new SwitchUserRoles.Builder().Build();
            theRoleModel = new RoleModel(ROLE);
        }

        protected override Controller GetController() {
            return theController;
        }
        
        [TestMethod]
        public void ShouldLoadRoles() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.View_Roles);
            List<Role> myRoles = new List<Role>();
            myRoles.Add(ROLE);
            theRoleService.Setup(s => s.GetAllRoles()).Returns(() => myRoles);

            var myResult = theController.Index() as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(myResult, "Index", myRoles);
        }

        [TestMethod]
        public void ShouldNotLoadRolesNotLoggedIn() {
            MockNotLoggedIn();

            var myResult = theController.Index() as ViewResult;

            theRoleService.Verify(s => s.GetAllRoles(), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadRolesWithoutPermission() {
            var myResult = theController.Index() as ViewResult;

            theRoleService.Verify(s => s.GetAllRoles(), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadRolesBecauseOfException() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.View_Roles);
            theRoleService.Setup(s => s.GetAllRoles()).Throws<Exception>();

            var myResult = theController.Index() as ViewResult;

            theRoleService.Verify(s => s.GetAllRoles(), Times.Exactly(1));
            AssertAuthenticatedErrorLogWithRedirect(myResult);
        }

        [TestMethod]
        public void ShouldLoadRolesButThereAreNone() {     
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.View_Roles);
            List<Role> myRoles = new List<Role>();
            theRoleService.Setup(s => s.GetAllRoles()).Returns(() => myRoles);

            var myResult = theController.Index() as ViewResult;

            theRoleService.Verify(s => s.GetAllRoles(), Times.Exactly(1));
            AssertAuthenticatedSuccessWithMessage(myResult, "Index", myRoles);
        }

        [TestMethod]
        public void UnableToLoadCreateRoleIfNotLoggedIn() {
            MockNotLoggedIn();
            var myResult = theController.Create() as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldLoadCreateRole() {
            Permission myPermission = Permission.CreatePermission(0, HAVPermission.Create_Role.ToString(), string.Empty, false);
            thePermissions.Add(myPermission);
            theUserInformationBuilder.AddPermissions(thePermissions);
            theMockUserInformation.Setup(f => f.GetUserInformaton()).Returns(() => theUserInformationBuilder.Build());
            theRestrictionService.Setup(r => r.GetAllRestrictions()).Returns(() => theRestrictions);
            thePermissionService.Setup(p => p.GetAllPermissions()).Returns(() => thePermissions);

            var myResult = theController.Create() as ViewResult;

            Assert.IsNull(theController.ViewData["PermissionMessage"]);
            Assert.IsNull(theController.ViewData["RestrictionMessage"]);
            AssertAuthenticatedCleanSuccessWithReturn(myResult, "Create", theRoleModel);
        }

        [TestMethod]
        public void ShouldLoadCreateRoleButNoPermissions() {
            Permission myPermission = Permission.CreatePermission(0, HAVPermission.Create_Role.ToString(), string.Empty, false);
            thePermissions.Add(myPermission);
            theUserInformationBuilder.AddPermissions(thePermissions);
            theMockUserInformation.Setup(f => f.GetUserInformaton()).Returns(() => theUserInformationBuilder.Build());
            theRestrictionService.Setup(r => r.GetAllRestrictions()).Returns(() => theRestrictions);

            var myResult = theController.Create() as ViewResult;

            Assert.IsNull(theController.ViewData["RestrictionMessage"]);
            AssertAuthenticatedCleanSuccessWithReturn(myResult, "Create", theRoleModel);
        }

        [TestMethod]
        public void ShouldLoadCreateRoleButNoRestrictions() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Create_Role);

            thePermissionService.Setup(p => p.GetAllPermissions()).Returns(() => thePermissions);

            var myResult = theController.Create() as ViewResult;

            Assert.IsNull(theController.ViewData["PermissionMessage"]);
            AssertAuthenticatedCleanSuccessWithReturn(myResult, "Create", theRoleModel);
        }

        [TestMethod]
        public void ShouldNotLoadCreateRolePageWithoutPermission() {
            var myResult = theController.Create() as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }
        
        [TestMethod]
        public void ShouldNotLoadCreateRoleBecauseOfException() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Create_Role);
            thePermissionService.Setup(p => p.GetAllPermissions()).Throws<Exception>();

            var myResult = theController.Create() as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldCreateRole() {
            theRoleService.Setup(s => s.Create(It.IsAny<UserInformationModel>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>())).Returns(() => true);
            var myResult = theController.Create(theRoleModel) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void UnableToCreateRoleBecauseUserIsNotLoggedIn() {
            MockNotLoggedIn();
            var myResult = theController.Create(theRoleModel) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void UnableToCreateRoleBecauseOfException() {
            theRestrictionService.Setup(r => r.GetAllRestrictions()).Returns(() => theRestrictions);
            thePermissionService.Setup(p => p.GetAllPermissions()).Returns(() => thePermissions);
            theRoleService.Setup(s => s.Create(It.IsAny<UserInformationModel>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>())).Throws<Exception>();
            var myResult = theController.Create(theRoleModel) as ViewResult;

            AssertAuthenticatedErrorLogReturnBack(myResult, "Create", theRoleModel);
        }

        [TestMethod]
        public void UnableToCreateRoleBecauseOfExceptionOnReturn() {
            theRoleService.Setup(s => s.Create(It.IsAny<UserInformationModel>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>())).Returns(() => false);
            thePermissionService.Setup(p => p.GetAllPermissions()).Throws<Exception>();

            var myResult = theController.Create(theRoleModel) as ViewResult;

            AssertAuthenticatedErrorLogWithRedirect(myResult);
        }

        [TestMethod]
        public void UnableToCreateRoleBecauseOfInvalidInput() {
            theRoleService.Setup(s => s.Create(It.IsAny<UserInformationModel>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>())).Returns(() => false);
            theRestrictionService.Setup(r => r.GetAllRestrictions()).Returns(() => theRestrictions);
            thePermissionService.Setup(p => p.GetAllPermissions()).Returns(() => thePermissions);

            var myResult = theController.Create(theRoleModel) as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(myResult, "Create", theRoleModel);
        }

        [TestMethod]
        public void ShouldLoadEditRole() {
            Permission myPermission = Permission.CreatePermission(0, HAVPermission.Edit_Role.ToString(), string.Empty, false);
            thePermissions.Add(myPermission);
            theUserInformationBuilder.AddPermissions(thePermissions);
            theMockUserInformation.Setup(f => f.GetUserInformaton()).Returns(() => theUserInformationBuilder.Build());
            ROLE.Restriction = RESTRICTION;
            theRoleService.Setup(s => s.FindRole(It.IsAny<int>())).Returns(() => ROLE);
            
            var myResult = theController.Edit(ROLE_ID) as ViewResult;
            
            RoleModel myModel = (RoleModel)myResult.ViewData.Model;
            Assert.AreEqual(RESTRICTION.Id, myModel.SelectedRestrictionId); 
            AssertAuthenticatedCleanSuccessWithReturn(myResult, "Edit", theRoleModel);
        }

        [TestMethod]
        public void UnableToLoadEditRoleBecauseOfNoPermission() {
            var myResult = theController.Edit(ROLE_ID) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void UnableToLoadBecauseRoleNotFound() {
            var myResult = theController.Edit(ROLE_ID) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void UnableToLoadEditRoleBecauseUserIsNotLoggedIn() {
            MockNotLoggedIn();
            var myResult = theController.Edit(ROLE_ID) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadEditRoleBecauseOfException() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Edit_Role);            
            theRoleService.Setup(s => s.FindRole(It.IsAny<int>())).Throws<Exception>();

            var myResult = theController.Edit(ROLE_ID) as ViewResult;

            AssertAuthenticatedErrorLogWithRedirect(myResult);
        }

        [TestMethod]
        public void UnableToLoadEditRoleBecauseOfExceptionOnReturn() {
            theRoleService.Setup(s => s.FindRole(It.IsAny<int>())).Returns(() => ROLE);
            thePermissionService.Setup(p => p.GetAllPermissions()).Throws<Exception>();

            var myResult = theController.Create(theRoleModel) as ViewResult;

            AssertAuthenticatedErrorLogWithRedirect(myResult);
        }

        [TestMethod]
        public void ShouldEditRole() {
            theRoleService.Setup(s => s.Edit(It.IsAny<UserInformationModel>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>())).Returns(() => true);
            var myResult = theController.Edit(theRoleModel) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void UnableToEditRoleBecauseUserIsNotLoggedIn() {
            MockNotLoggedIn();
            var myResult = theController.Edit(theRoleModel) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void UnableToEditRoleBecauseOfException() {
            theRestrictionService.Setup(r => r.GetAllRestrictions()).Returns(() => theRestrictions);
            thePermissionService.Setup(p => p.GetAllPermissions()).Returns(() => thePermissions);
            theRoleService.Setup(s => s.Edit(It.IsAny<UserInformationModel>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>())).Throws<Exception>();
            var myResult = theController.Edit(theRoleModel) as ViewResult;

            AssertAuthenticatedErrorLogReturnBack(myResult, "Edit", theRoleModel);
        }

        [TestMethod]
        public void UnableToEditRoleBecauseOfExceptionOnReturn() {
            theRoleService.Setup(s => s.Edit(It.IsAny<UserInformationModel>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>())).Returns(() => false);
            thePermissionService.Setup(p => p.GetAllPermissions()).Throws<Exception>();

            var myResult = theController.Edit(theRoleModel) as ViewResult;

            AssertAuthenticatedErrorLogWithRedirect(myResult);
        }

        [TestMethod]
        public void UnableToEditRoleBecauseOfInvalidInput() {
            theRoleService.Setup(s => s.Edit(It.IsAny<UserInformationModel>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>())).Returns(() => false);
            theRestrictionService.Setup(r => r.GetAllRestrictions()).Returns(() => theRestrictions);
            thePermissionService.Setup(p => p.GetAllPermissions()).Returns(() => thePermissions);

            var myResult = theController.Edit(theRoleModel) as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(myResult, "Edit", theRoleModel);
        }

        [TestMethod]
        public void ShouldLoadDeleteRole() {
            Permission myPermission = Permission.CreatePermission(0, HAVPermission.Delete_Role.ToString(), string.Empty, false);
            thePermissions.Add(myPermission);
            theUserInformationBuilder.AddPermissions(thePermissions);
            theMockUserInformation.Setup(f => f.GetUserInformaton()).Returns(() => theUserInformationBuilder.Build());
            theRoleService.Setup(s => s.FindRole(It.IsAny<int>())).Returns(() => ROLE);

            var myResult = theController.Delete(ROLE_ID) as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(myResult, "Delete", ROLE);
        }

        [TestMethod]
        public void ShouldNotLoadDeleteRoleBecauseOfNoPermission() {
            var myResult = theController.Delete(ROLE_ID) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadDeleteRoleBecauseOfNotBeingLoggedIn() {
            MockNotLoggedIn();
            var myResult = theController.Delete(ROLE_ID) as ViewResult;
            AssertAuthenticatedRedirection(myResult);       
        }

        [TestMethod]
        public void ShouldLoadDeleteRolePageButNoRoleIsFound() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Delete_Role);
            theRoleService.Setup(s => s.FindRole(It.IsAny<int>())).Returns(() => null);
            var myResult = theController.Delete(ROLE_ID) as ViewResult;
            AssertAuthenticatedRedirection(myResult);
        }


        [TestMethod]
        public void ShouldNotLoadDeleteRolePageBecauseOfException() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Delete_Role);
            theRoleService.Setup(s => s.FindRole(It.IsAny<int>())).Throws<Exception>();
            var myResult = theController.Delete(ROLE_ID) as ViewResult;
            AssertAuthenticatedErrorLogWithRedirect(myResult);
        }

        [TestMethod]
        public void TestDeleteRole_Success() {
            var myResult = theController.Delete(ROLE) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void TestDeleteRole_NotLoggedIn() {
            MockNotLoggedIn();
            var myResult = theController.Delete(ROLE) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void TestDeleteRole_Exception() {
            theRoleService.Setup(s => s.Delete(It.IsAny<UserInformationModel>(), It.IsAny<Role>())).Throws<Exception>();

            var myResult = theController.Delete(ROLE) as ViewResult;

            AssertAuthenticatedErrorLogWithRedirect(myResult);
        }

        [TestMethod]
        public void ShouldLoadSwitchUsersRolesPage() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Switch_Users_Role);
            var myResult = theController.SwitchUserRoles() as ViewResult;
            AssertAuthenticatedCleanSuccessWithReturn(myResult, "SwitchUserRoles", theSwitchUserRolesModel);
        }

        [TestMethod]
        public void ShouldNotLoadSwitchUsersRolesPageWithoutPermission() {
            var myResult = theController.SwitchUserRoles() as ViewResult;
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void TestSwitchUserRolesNonPostBack_NotLoggedIn() {
            MockNotLoggedIn();
            var myResult = theController.SwitchUserRoles() as ViewResult;
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void TestSwitchUserRolesNonPostBack_Exception() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Switch_Users_Role);
            theRoleService.Setup(s => s.GetAllRoles()).Throws<Exception>();
            var myResult = theController.SwitchUserRoles() as ViewResult;
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldGetUsersToSwitchRoles() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Switch_Users_Role);
            var myResult = theController.SwitchUserRoles(FROM_ROLE_ID, TO_ROLE_ID) as ViewResult;
            AssertAuthenticatedCleanSuccessWithReturn(myResult, "SwitchUserRoles", theSwitchUserRolesModel);
        }

        [TestMethod]
        public void ShouldNotGetUsersToSwitchRolesBecauseNotLoggedIn() {
            MockNotLoggedIn();
            var myResult = theController.SwitchUserRoles(FROM_ROLE_ID, TO_ROLE_ID) as ViewResult;
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotGetUsersToSwitchRolesBecauseOfException() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Switch_Users_Role);
            theRoleService.Setup(s => s.GetAllRoles()).Throws<Exception>();
            var myResult = theController.SwitchUserRoles(FROM_ROLE_ID, TO_ROLE_ID) as ViewResult;
            AssertAuthenticatedErrorLogWithRedirect(myResult);
        }

        [TestMethod]
        public void ShouldNotGetUsersToSwitchRolesBecauseOfNoPermission() {
            var myResult = theController.SwitchUserRoles(FROM_ROLE_ID, TO_ROLE_ID) as ViewResult;
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldMoveUsersToRole() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Switch_Users_Role);
            var myResult = theController.SwitchUserRoles(SELECTED_USERS, FROM_ROLE_ID, TO_ROLE_ID) as ViewResult;
            AssertAuthenticatedCleanSuccessWithReturn(myResult, "SwitchUserRoles", theSwitchUserRolesModel);
        }

        [TestMethod]
        public void ShouldNotMoveUsersToRoleBecauseOfNoPermission() {
            var myResult = theController.SwitchUserRoles(SELECTED_USERS, FROM_ROLE_ID, TO_ROLE_ID) as ViewResult;
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotMoveUsersToRoleBecauseNotLoggedIn() {
            MockNotLoggedIn();
            var myResult = theController.SwitchUserRoles(SELECTED_USERS, FROM_ROLE_ID, TO_ROLE_ID) as ViewResult;
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotMoveUsersToRoleBecauseOfException() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Switch_Users_Role);
            theRoleService.Setup(s => s.GetAllRoles()).Throws<Exception>();
            var myResult = theController.SwitchUserRoles(SELECTED_USERS, FROM_ROLE_ID, TO_ROLE_ID) as ViewResult;
            AssertAuthenticatedErrorLogWithRedirect(myResult);
        }*/
    }
}
