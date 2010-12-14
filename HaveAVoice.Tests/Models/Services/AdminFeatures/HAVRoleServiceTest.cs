using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Moq;
using HaveAVoice.Models.Validation;
using HaveAVoice.Models.Services;
using HaveAVoice.Models.Repositories;
using HaveAVoice.Models;
using System.Collections.Generic;
using HaveAVoice.Models.Services.AdminFeatures;
using HaveAVoice.Models.Repositories.AdminFeatures;
using HaveAVoice.Models.View;
using HaveAVoice.Models.View.Builders;
using HaveAVoice.Helpers;


namespace HaveAVoice.Tests.Models.Services.AdminFeatures {
    [TestClass]
    public class HAVRoleServiceTest {
        private static string ROLE_NAME = "Admin";
        private static string ROLE_DESCRIPTION = "Admin Role.";
        private static string PERMISSION_NAME = "View Message";
        private static string PERMISSION_DESCRIPTION = "Permission to view a message.";
        private static bool DEFAULT_ROLE = false;
        private static int SELECTED_RESTRICTION_ID = 45;
        private static User USER = new User();
        private static Permission PERMISSION = Permission.CreatePermission(0, PERMISSION_NAME, PERMISSION_DESCRIPTION, false);

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

            Role role = Role.CreateRole(0, ROLE_NAME, ROLE_DESCRIPTION, DEFAULT_ROLE, false);
            bool result = theService.CreateRole(theUserInformationBuilder.Build(), role, SELECTED_PERMISSIONS, SELECTED_RESTRICTION_ID);

            theMockRepository.Verify(r => r.CreateRole(It.IsAny<User>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>()), Times.Exactly(1));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestCreateRole_RequiredName() {
            Role myRole = Role.CreateRole(0, string.Empty, ROLE_DESCRIPTION, DEFAULT_ROLE, false);
            bool myResult = theService.CreateRole(theUserInformationBuilder.Build(), myRole, SELECTED_PERMISSIONS, SELECTED_RESTRICTION_ID);

            theMockRepository.Verify(r => r.CreateRole(It.IsAny<User>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>()), Times.Never());
            Assert.IsFalse(myResult);
            var myError = theModelState["Name"].Errors[0];
            Assert.AreEqual("Role name is required.", myError.ErrorMessage);
        }

        [TestMethod]
        public void TestCreateRole_RequiredDescriptione() {
            Role myRole = Role.CreateRole(0, ROLE_NAME, string.Empty, DEFAULT_ROLE, false);

            bool myResult = theService.CreateRole(theUserInformationBuilder.Build(), myRole, SELECTED_PERMISSIONS, SELECTED_RESTRICTION_ID);

            theMockRepository.Verify(r => r.CreateRole(It.IsAny<User>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>()), Times.Never());
            Assert.IsFalse(myResult);
            var myError = theModelState["Description"].Errors[0];
            Assert.AreEqual("Role description is required.", myError.ErrorMessage);
        }

        [TestMethod]
        public void TestCreateRole_RequiredRestriction() {
            Role myRole = Role.CreateRole(0, ROLE_NAME, ROLE_DESCRIPTION, DEFAULT_ROLE, false);

            bool myResult = theService.CreateRole(theUserInformationBuilder.Build(), myRole, SELECTED_PERMISSIONS, 0);

            theMockRepository.Verify(r => r.CreateRole(It.IsAny<User>(), It.IsAny<Role>(), It.IsAny<List<int>>(), It.IsAny<int>()), Times.Never());
            Assert.IsFalse(myResult);
            var myError = theModelState["Restriction"].Errors[0];
            Assert.AreEqual("Please select a restriction.", myError.ErrorMessage);
        }

        [TestMethod]
        public void ShouldCreateValidPermission() {
            Permission myPermission = Permission.CreatePermission(0, HAVPermission.Create_Permission.ToString(), string.Empty, false);
            List<Permission> myPermissions = new List<Permission>();
            myPermissions.Add(myPermission);
            theUserInformationBuilder.AddPermissions(myPermissions);

            bool result = theService.CreatePermission(theUserInformationBuilder.Build(), PERMISSION);

            theMockRepository.Verify(r => r.CreatePermission(It.IsAny<User>(), It.IsAny<Permission>()), Times.Exactly(1));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UnableToCreatePermissionBecauseOfNoCreatePermission() {
            bool result = theService.CreatePermission(theUserInformationBuilder.Build(), PERMISSION);

            theMockRepository.Verify(r => r.CreatePermission(It.IsAny<User>(), It.IsAny<Permission>()), Times.Never());
            Assert.IsFalse(result);
            var myError = theModelState["PerformAction"].Errors[0];
            Assert.IsNotNull(myError);
        }

        [TestMethod]
        public void UnableToCreatePermissionBecauseNameIsRequired() {
            Permission myPermission = Permission.CreatePermission(0, string.Empty, PERMISSION_DESCRIPTION, false);

            bool myResult = theService.CreatePermission(theUserInformationBuilder.Build(), myPermission);

            theMockRepository.Verify(r => r.CreatePermission(It.IsAny<User>(), It.IsAny<Permission>()), Times.Never());
            Assert.IsFalse(myResult);
            var myError = theModelState["Name"].Errors[0];
            Assert.AreEqual("Permission name is required.", myError.ErrorMessage);
        }

        [TestMethod]
        public void TestCreatePermission_RequiredDescription() {
            Permission myPermission = Permission.CreatePermission(0, PERMISSION_NAME, string.Empty, false);

            bool myResult = theService.CreatePermission(theUserInformationBuilder.Build(), myPermission);

            theMockRepository.Verify(r => r.CreatePermission(It.IsAny<User>(), It.IsAny<Permission>()), Times.Never());
            Assert.IsFalse(myResult);
            var myError = theModelState["Description"].Errors[0];
            Assert.AreEqual("Permission description is required.", myError.ErrorMessage);
        }
    }
}
