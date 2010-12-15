using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HaveAVoice.Models;
using System.Web.Mvc;
using Moq;
using HaveAVoice.Helpers;
using HaveAVoice.Models.Repositories;
using HaveAVoice.Services;
using HaveAVoice.Models.Validation;
using HaveAVoice.Controllers.Admin;
using HaveAVoice.Models.View;
using HaveAVoice.Services.AdminFeatures;
using HaveAVoice.Models.Repositories.AdminFeatures;
using HaveAVoice.Models.View.Builders;
using HaveAVoice.Tests.Helpers;

namespace HaveAVoice.Tests.Controllers.Admin {
    [TestClass]
    public class RestrictionControllerTest : ControllerTestCase {
        private static int[] SELECTED_PERMISSIONS = new int[] { 0 };

        private static Restriction theRestriction;
        private static int VALID_RESTRICTION_ID = 3;
        private static int INVALID_RESTRICTION_ID = 4;
        private static List<int> SELECTED_ROLES = new List<int>();

        private IHAVRestrictionService theService;
        private Mock<IHAVRestrictionService> theMockedService;
        private Mock<IHAVRestrictionRepository> theMockRepository;
        private Mock<IHAVRoleService> theMockedRoleService;
        private RestrictionController theController;

        private UserInformationModel USER_INFORMATION = new UserInformationModelBuilder(new User()).Build();
        private RestrictionModel theRestrictionModel;

        
        [TestInitialize]
        public void Initialize() {
            theModelState = new ModelStateDictionary();

            theMockedService = new Mock<IHAVRestrictionService>();
            theMockRepository = new Mock<IHAVRestrictionRepository>();
            theMockedRoleService = new Mock<IHAVRoleService>();

            theService = new HAVRestrictionService(new ModelStateWrapper(theModelState),
                                                               theMockRepository.Object,
                                                               theBaseRepository.Object);

            theController = new RestrictionController(theMockedService.Object, theMockedBaseService.Object, theMockedRoleService.Object);
            theController.ControllerContext = GetControllerContext();


            theRestrictionModel = new RestrictionModel.Builder(0).Name("Restriction Name").Description("This is a myRestriction.").Build();
            theRestriction = theRestrictionModel.Restriction;
        }

        protected override Controller GetController() {
            return theController;
        }

        [TestMethod]
        public void ShouldLoadRestrictions() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.View_Restrictions);
            List<Restriction>  myRestrictions = new List<Restriction>();
            myRestrictions.Add(theRestriction);
            theMockedService.Setup(s => s.GetAllRestrictions()).Returns(() => myRestrictions);

            var myResult = theController.Index() as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(myResult, "Index", myRestrictions);
        }

        [TestMethod]
        public void ShouldNotLoadRestrictionsNotLoggedIn() {
            theMockUserInformation.Setup(u => u.GetUserInformaton()).Returns(() => null);

            var myResult = theController.Index() as ViewResult;

            theMockedService.Verify(s => s.GetAllRestrictions(), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadRestrictionsWithoutPermission() {
            var myResult = theController.Index() as ViewResult;

            theMockedService.Verify(s => s.GetAllRestrictions(), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadRestrictionsBecauseOfException() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.View_Restrictions);
            theMockedService.Setup(s => s.GetAllRestrictions()).Throws<Exception>();

            var result = theController.Index() as ViewResult;

            theMockedService.Verify(s => s.GetAllRestrictions(), Times.Exactly(1));
            AssertAuthenticatedErrorLogWithRedirect(result);
        }

        [TestMethod]
        public void ShouldLoadRestrictionsButThereAreNone() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.View_Restrictions);
            List<Restriction> myRestrictions = new List<Restriction>();
            theMockedService.Setup(s => s.GetAllRestrictions()).Returns(() => myRestrictions);

            var result = theController.Index() as ViewResult;

            theMockedService.Verify(s => s.GetAllRestrictions(), Times.Exactly(1));
            AssertAuthenticatedSuccessWithMessage(result, "Index", myRestrictions);
        }

        [TestMethod]
        public void ShouldLoadCreateRestriction() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Create_Restriction);
            var result = theController.Create() as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(result, "Create", theRestrictionModel);
        }

        [TestMethod]
        public void ShouldNotLoadCreateRestrictionWithoutPermission() {
            var myResult = theController.Create() as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadCreateRestrictionBecauseNotLoggedIn() {
            theMockUserInformation.Setup(u => u.GetUserInformaton()).Returns(() => null);

            var result = theController.Create() as ViewResult;

            AssertAuthenticatedRedirection(result);
        }

        [TestMethod]
        public void ShouldCreateRestriction() {
            theMockUserInformation.Setup(u => u.GetUserInformaton()).Returns(() => USER_INFORMATION);
            theMockedService.Setup(s => s.CreateRestriction(It.IsAny<UserInformationModel>(), It.IsAny<Restriction>())).Returns(() => true);
            var result = theController.Create(theRestrictionModel) as ViewResult;

            AssertAuthenticatedRedirection(result);
        }

        [TestMethod]
        public void ShouldNotCreateRestrictionBecauseNotLoggedIn() {
            theMockUserInformation.Setup(u => u.GetUserInformaton()).Returns(() => null);
            theMockedService.Setup(s => s.CreateRestriction(It.IsAny<UserInformationModel>(), It.IsAny<Restriction>())).Returns(() => true);
            var result = theController.Create(theRestrictionModel) as ViewResult;

            AssertAuthenticatedRedirection(result);
        }

        [TestMethod]
        public void ShouldNotCreateRestrictionBecauseOfException() {
            theMockUserInformation.Setup(u => u.GetUserInformaton()).Returns(() => USER_INFORMATION);
            theMockedService.Setup(s => s.CreateRestriction(It.IsAny<UserInformationModel>(), It.IsAny<Restriction>())).Throws<Exception>();
            var myResult = theController.Create(theRestrictionModel) as ViewResult;

            AssertAuthenticatedErrorLogReturnBack(myResult, "Create");
        }

        [TestMethod]
        public void ShouldNotCreateRole() {
            theMockUserInformation.Setup(u => u.GetUserInformaton()).Returns(() => USER_INFORMATION);
            theMockedService.Setup(s => s.CreateRestriction(It.IsAny<UserInformationModel>(), It.IsAny<Restriction>())).Returns(() => false);

            var result = theController.Create(theRestrictionModel) as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(result, "Create", theRestrictionModel);
            Assert.AreEqual(theRestrictionModel, result.ViewData.Model);
        }

        [TestMethod]
        public void ShouldLoadDeleteRestrictionPage() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Delete_Restriction);
            theMockedService.Setup(s => s.GetRestriction(It.Is<int>(i => i == VALID_RESTRICTION_ID))).Returns(() => theRestriction);

            var result = theController.Delete(VALID_RESTRICTION_ID) as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(result, "Delete", theRestriction);
        }

        [TestMethod]
        public void ShouldNotLoadDeleteRestrictionPageWithoutPermission() {
            var myResult = theController.Delete(VALID_RESTRICTION_ID) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadDeleteRestrictionPageBecauseNotLoggedIn() {
            theMockUserInformation.Setup(u => u.GetUserInformaton()).Returns(() => null);

            var myResult = theController.Delete(VALID_RESTRICTION_ID) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldLoadDeleteRestrictionPageButNoRestrictionIsFound() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Delete_Restriction);
            theMockedService.Setup(s => s.GetRestriction(It.Is<int>(i => i == VALID_RESTRICTION_ID))).Returns(() => theRestriction);

            var result = theController.Delete(INVALID_RESTRICTION_ID) as ViewResult;

            theMockedService.Verify(s => s.GetRestriction(INVALID_RESTRICTION_ID), Times.AtLeastOnce());
            AssertAuthenticatedRedirection(result);
        }

        [TestMethod]
        public void ShouldNotLoadDeleteRestrictionPageBecauseOfException() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Delete_Restriction);
            theMockedService.Setup(s => s.GetRestriction(It.IsAny<int>())).Throws<Exception>();

            var result = theController.Delete(VALID_RESTRICTION_ID) as ViewResult;

            AssertAuthenticatedErrorLogWithRedirect(result);
        }

        [TestMethod]
        public void ShouldDeleteRestriction() {
            theMockedService.Setup(s => s.DeleteRestriction(It.IsAny<UserInformationModel>(), It.IsAny<Restriction>())).Returns(() => true);
            var myResult = theController.Delete(theRestriction) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotDeleteRestriction() {
            theMockedService.Setup(s => s.DeleteRestriction(It.IsAny<UserInformationModel>(), It.IsAny<Restriction>())).Returns(() => false);
            var myResult = theController.Delete(theRestriction) as ViewResult;

            AssertAuthenticatedFailWithReturnBack(myResult, "Delete", theRestriction);
        }

        [TestMethod]
        public void ShouldNotDeleteRestrictionNotLoggedIn() {
            theMockUserInformation.Setup(u => u.GetUserInformaton()).Returns(() => null);

            var myResult = theController.Delete(theRestriction) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotDeleteRestrictionBecauseOfException() {
            theMockedService.Setup(s => s.DeleteRestriction(It.IsAny<UserInformationModel>(), It.IsAny<Restriction>())).Throws<Exception>();

            var result = theController.Delete(theRestriction) as ViewResult;

            AssertAuthenticatedErrorLogWithRedirect(result);
        }

        [TestMethod]
        public void ShouldLoadEditRestrictionPage() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Edit_Restriction);
            List<Role> roles = new List<Role>();
            roles.Add(Role.CreateRole(1, "Role1", "Role1 Description", false, false));

            theMockedService.Setup(s => s.GetRestriction(It.Is<int>(i => i == VALID_RESTRICTION_ID))).Returns(() => theRestriction);
            theMockedRoleService.Setup(s => s.GetAllRoles()).Returns(() => roles);

            var myResult = theController.Edit(VALID_RESTRICTION_ID) as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(myResult, "Edit", theRestriction);
            Assert.AreEqual(theRestriction, myResult.ViewData.Model);
        }

        [TestMethod]
        public void ShouldNotLoadEditRestrictionPageWithoutPermission() {
            theMockedService.Setup(s => s.GetRestriction(It.IsAny<int>())).Returns(() => theRestriction);

            var myResult = theController.Edit(VALID_RESTRICTION_ID) as ViewResult;

            theMockedService.Verify(s => s.EditRestriction(It.IsAny<UserInformationModel>(), It.IsAny<Restriction>()), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadEditRestrictionNotLoggedIn() {
            theMockUserInformation.Setup(u => u.GetUserInformaton()).Returns(() => null);

            var result = theController.Edit(VALID_RESTRICTION_ID) as ViewResult;

            theMockedService.Verify(s => s.EditRestriction(It.IsAny<UserInformationModel>(), It.IsAny<Restriction>()), Times.Never());
            AssertAuthenticatedRedirection(result);
        }

        [TestMethod]
        public void ShouldNotLoadEditRestrictionBecauseOfGetRoleException() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Edit_Restriction);
            theMockedService.Setup(s => s.GetRestriction(It.IsAny<int>())).Throws<Exception>();
            
            var myResult = theController.Edit(VALID_RESTRICTION_ID) as ViewResult;

            AssertAuthenticatedErrorLogWithRedirect(myResult);
        }


        [TestMethod]
        public void ShouldNotLoadEditRestrictionPageBecuaseNoRestrictionFound() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Edit_Restriction);
            theMockedService.Setup(s => s.GetRestriction(It.Is<int>(i => i == VALID_RESTRICTION_ID))).Returns(() => theRestriction);

            var result = theController.Edit(INVALID_RESTRICTION_ID) as ViewResult;

            theMockedService.Verify(s => s.GetRestriction(It.IsAny<int>()), Times.Exactly(1));
            AssertAuthenticatedRedirection(result);
        }

        [TestMethod]
        public void ShouldEditRestriction() {
            theMockedService.Setup(s => s.EditRestriction(It.IsAny<UserInformationModel>(), It.IsAny<Restriction>())).Returns(() => true);

            var result = theController.Edit(theRestriction) as ViewResult;

            AssertAuthenticatedRedirection(result);
        }

        [TestMethod]
        public void ShouldNotEditRestriction() {
            theMockedService.Setup(s => s.EditRestriction(It.IsAny<UserInformationModel>(), It.IsAny<Restriction>()))
                .Returns(() => false);

            var result = theController.Edit(theRestriction) as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(result, "Edit", theRestriction);
        }

        [TestMethod]
        public void ShouldNotEditRestrictionBecauseOfException() {
            theMockedService.Setup(s => s.EditRestriction(It.IsAny<UserInformationModel>(), It.IsAny<Restriction>())).Throws<Exception>();

            var myResult = theController.Edit(theRestriction) as ViewResult;

            AssertAuthenticatedErrorLogReturnBack(myResult, "Edit");
        }
    }
}
