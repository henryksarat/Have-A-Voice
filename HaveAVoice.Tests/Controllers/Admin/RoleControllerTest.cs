using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Moq;
using HaveAVoice.Repositories;
using HaveAVoice.Services;
using HaveAVoice.Validation;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Controllers.Admin;
using HaveAVoice.Models.View;
using HaveAVoice.Services.AdminFeatures;
using HaveAVoice.Repositories.AdminFeatures;
using HaveAVoice.Tests.Helpers;

namespace HaveAVoice.Tests.Controllers.Admin {
    [TestClass]
    public class RoleControllerTest : ControllerTestCase  {
        private static int[] SELECTED_PERMISSIONS = new int[] {0};
        private static int[] SELECTED_USERS = new int[] { 0 };
        private static int ROLE_ID = 3;
        private static int FROM_ROLE_ID = 5;
        private static int TO_ROLE_ID = 6;
        private static Role ROLE = Role.CreateRole(0, "Admin", "Admin Role", false, false);
        private static Restriction RESTRICTION = new RestrictionModel.Builder(0).Build().Restriction;
        private static Permission PERMISSION = Permission.CreatePermission(0, string.Empty, string.Empty, false);
        private static SwitchUserRoles theSwitchUserRolesModel;
        
        private List<Restriction> theRestrictions = new List<Restriction>();
        private List<Permission> thePermissions = new List<Permission>();
        private IHAVRoleService theService;
        private Mock<IHAVRoleService> theMockedService;
        private Mock<IHAVRoleRepository> theMockRepository;
        private Mock<IHAVRestrictionService> theMockedRestrictionService;
        private RoleController theController;
        private RoleModel theRoleModel;

        [TestInitialize]
        public void Initialize() {
            theRestrictions.Add(RESTRICTION);
            thePermissions.Add(PERMISSION);

            theMockedService = new Mock<IHAVRoleService>();
            theMockRepository = new Mock<IHAVRoleRepository>();
            theMockedRestrictionService = new Mock<IHAVRestrictionService>();

            theService = new HAVRoleService(new ModelStateWrapper(theModelState), 
                                                               theMockRepository.Object,
                                                               theBaseRepository.Object);
 
            theController = new RoleController(theMockedService.Object, theMockedBaseService.Object, theMockedRestrictionService.Object);
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
            theMockedService.Setup(s => s.GetAllRoles()).Returns(() => myRoles);

            var myResult = theController.Index() as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(myResult, "Index", myRoles);
        }

        [TestMethod]
        public void ShouldNotLoadRolesNotLoggedIn() {
            MockNotLoggedIn();

            var myResult = theController.Index() as ViewResult;

            theMockedService.Verify(s => s.GetAllRoles(), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadRolesWithoutPermission() {
            var myResult = theController.Index() as ViewResult;

            theMockedService.Verify(s => s.GetAllRoles(), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadRolesBecauseOfException() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.View_Roles);
            theMockedService.Setup(s => s.GetAllRoles()).Throws<Exception>();

            var myResult = theController.Index() as ViewResult;

            theMockedService.Verify(s => s.GetAllRoles(), Times.Exactly(1));
            AssertAuthenticatedErrorLogWithRedirect(myResult);
        }

        [TestMethod]
        public void ShouldLoadRolesButThereAreNone() {     
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.View_Roles);
            List<Role> myRoles = new List<Role>();
            theMockedService.Setup(s => s.GetAllRoles()).Returns(() => myRoles);

            var myResult = theController.Index() as ViewResult;

            theMockedService.Verify(s => s.GetAllRoles(), Times.Exactly(1));
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
            theMockedRestrictionService.Setup(r => r.GetAllRestrictions()).Returns(() => theRestrictions);
            theMockedService.Setup(r => r.GetAllPermissions()).Returns(() => thePermissions);

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
            theMockedRestrictionService.Setup(r => r.GetAllRestrictions()).Returns(() => theRestrictions);

            var myResult = theController.Create() as ViewResult;

            Assert.IsNull(theController.ViewData["RestrictionMessage"]);
            AssertAuthenticatedCleanSuccessWithReturn(myResult, "Create", theRoleModel);
        }

        [TestMethod]
        public void ShouldLoadCreateRoleButNoRestrictions() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Create_Role);

            theMockedService.Setup(r => r.GetAllPermissions()).Returns(() => thePermissions);

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
            theMockedService.Setup(s => s.GetAllPermissions()).Throws<Exception>();

            var myResult = theController.Create() as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldCreateRole() {
            theMockedService.Setup(s => s.CreateRole(It.IsAny<UserInformationModel>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>())).Returns(() => true);
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
            theMockedRestrictionService.Setup(r => r.GetAllRestrictions()).Returns(() => theRestrictions);
            theMockedService.Setup(s => s.GetAllPermissions()).Returns(() => thePermissions);
            theMockedService.Setup(s => s.CreateRole(It.IsAny<UserInformationModel>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>())).Throws<Exception>();
            var myResult = theController.Create(theRoleModel) as ViewResult;

            AssertAuthenticatedErrorLogReturnBack(myResult, "Create", theRoleModel);
        }

        [TestMethod]
        public void UnableToCreateRoleBecauseOfExceptionOnReturn() {
            theMockedService.Setup(s => s.CreateRole(It.IsAny<UserInformationModel>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>())).Returns(() => false);
            theMockedService.Setup(s => s.GetAllPermissions()).Throws<Exception>();

            var myResult = theController.Create(theRoleModel) as ViewResult;

            AssertAuthenticatedErrorLogWithRedirect(myResult);
        }

        [TestMethod]
        public void UnableToCreateRoleBecauseOfInvalidInput() {
            theMockedService.Setup(s => s.CreateRole(It.IsAny<UserInformationModel>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>())).Returns(() => false);
            theMockedRestrictionService.Setup(r => r.GetAllRestrictions()).Returns(() => theRestrictions);
            theMockedService.Setup(s => s.GetAllPermissions()).Returns(() => thePermissions);

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
            theMockedService.Setup(s => s.GetRole(It.IsAny<int>())).Returns(() => ROLE);
            
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
            theMockedService.Setup(s => s.GetRole(It.IsAny<int>())).Throws<Exception>();

            var myResult = theController.Edit(ROLE_ID) as ViewResult;

            AssertAuthenticatedErrorLogWithRedirect(myResult);
        }

        [TestMethod]
        public void UnableToLoadEditRoleBecauseOfExceptionOnReturn() {
            theMockedService.Setup(s => s.GetRole(It.IsAny<int>())).Returns(() => ROLE);
            theMockedService.Setup(s => s.GetAllPermissions()).Throws<Exception>();

            var myResult = theController.Create(theRoleModel) as ViewResult;

            AssertAuthenticatedErrorLogWithRedirect(myResult);
        }

        [TestMethod]
        public void ShouldEditRole() {
            theMockedService.Setup(s => s.EditRole(It.IsAny<UserInformationModel>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>())).Returns(() => true);
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
            theMockedRestrictionService.Setup(r => r.GetAllRestrictions()).Returns(() => theRestrictions);
            theMockedService.Setup(s => s.GetAllPermissions()).Returns(() => thePermissions);
            theMockedService.Setup(s => s.EditRole(It.IsAny<UserInformationModel>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>())).Throws<Exception>();
            var myResult = theController.Edit(theRoleModel) as ViewResult;

            AssertAuthenticatedErrorLogReturnBack(myResult, "Edit", theRoleModel);
        }

        [TestMethod]
        public void UnableToEditRoleBecauseOfExceptionOnReturn() {
            theMockedService.Setup(s => s.EditRole(It.IsAny<UserInformationModel>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>())).Returns(() => false);
            theMockedService.Setup(s => s.GetAllPermissions()).Throws<Exception>();

            var myResult = theController.Edit(theRoleModel) as ViewResult;

            AssertAuthenticatedErrorLogWithRedirect(myResult);
        }

        [TestMethod]
        public void UnableToEditRoleBecauseOfInvalidInput() {
            theMockedService.Setup(s => s.EditRole(It.IsAny<UserInformationModel>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>())).Returns(() => false);
            theMockedRestrictionService.Setup(r => r.GetAllRestrictions()).Returns(() => theRestrictions);
            theMockedService.Setup(s => s.GetAllPermissions()).Returns(() => thePermissions);

            var myResult = theController.Edit(theRoleModel) as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(myResult, "Edit", theRoleModel);
        }

        [TestMethod]
        public void ShouldLoadDeleteRole() {
            Permission myPermission = Permission.CreatePermission(0, HAVPermission.Delete_Role.ToString(), string.Empty, false);
            thePermissions.Add(myPermission);
            theUserInformationBuilder.AddPermissions(thePermissions);
            theMockUserInformation.Setup(f => f.GetUserInformaton()).Returns(() => theUserInformationBuilder.Build());
            theMockedService.Setup(s => s.GetRole(It.IsAny<int>())).Returns(() => ROLE);

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
            theMockedService.Setup(s => s.GetRole(It.IsAny<int>())).Returns(() => null);
            var myResult = theController.Delete(ROLE_ID) as ViewResult;
            AssertAuthenticatedRedirection(myResult);
        }


        [TestMethod]
        public void ShouldNotLoadDeleteRolePageBecauseOfException() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Delete_Role);
            theMockedService.Setup(s => s.GetRole(It.IsAny<int>())).Throws<Exception>();
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
            theMockedService.Setup(s => s.DeleteRole(It.IsAny<UserInformationModel>(), It.IsAny<Role>())).Throws<Exception>();

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
            theMockedService.Setup(s => s.GetAllRoles()).Throws<Exception>();
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
            theMockedService.Setup(s => s.GetAllRoles()).Throws<Exception>();
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
            theMockedService.Setup(s => s.GetAllRoles()).Throws<Exception>();
            var myResult = theController.SwitchUserRoles(SELECTED_USERS, FROM_ROLE_ID, TO_ROLE_ID) as ViewResult;
            AssertAuthenticatedErrorLogWithRedirect(myResult);
        }
    }
}
