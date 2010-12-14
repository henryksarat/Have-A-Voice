using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Moq;
using HaveAVoice.Models.Repositories;
using HaveAVoice.Models.Services;
using HaveAVoice.Models.Validation;
using HaveAVoice.Models;
using System.Collections.Generic;
using HaveAVoice.Models.Services.AdminFeatures;
using HaveAVoice.Models.Repositories.AdminFeatures;
using HaveAVoice.Models.View.Builders;
using HaveAVoice.Models.View;
using HaveAVoice.Tests.Helpers;
using HaveAVoice.Helpers;

namespace HaveAVoice.Tests.Models.Services.AdminFeatures {
    [TestClass]
    public class HAVRestrictionServiceTest {
        private static string RESTRICTION_NAME = "Regular Users";
        private static string RESTRICTION_DESCRIPTION = "Restriction for regular users";
        private static int RESTRICTION_ID = 0;

        private static List<int> SELECTED_ROLES = new List<int>();
        private static User USER = new User();


        private UserInformationModelBuilder theUserInformationBuilder = new UserInformationModelBuilder(USER);
        private RestrictionModel.Builder theRestrictionModelBuilder = new RestrictionModel.Builder(RESTRICTION_ID);
        private ModelStateDictionary theModelState;
        private IHAVRestrictionService theService;
        private Mock<IHAVRestrictionRepository> theMockRepository;
        private Mock<IHAVBaseRepository> theBaseRepository;
        private Mock<IHAVBaseService> theMockedBaseService;


        [TestInitialize]
        public void Initialize() {
            theModelState = new ModelStateDictionary();
            theMockRepository = new Mock<IHAVRestrictionRepository>();
            theBaseRepository = new Mock<IHAVBaseRepository>();
            theMockedBaseService = new Mock<IHAVBaseService>();

            theService = new HAVRestrictionService(new ModelStateWrapper(theModelState),
                                                                       theMockRepository.Object,
                                                                       theBaseRepository.Object);
           
        }

        [TestMethod]
        public void ShouldCreateValidRestriction() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Create_Restriction);
            Restriction myRestriction = theRestrictionModelBuilder.Name(RESTRICTION_NAME).Description(RESTRICTION_DESCRIPTION).Build().Restriction;

            bool myResult = theService.CreateRestriction(theUserInformationBuilder.Build(), myRestriction);

            theMockRepository.Verify(r => r.CreateRestriction(It.IsAny<User>(), It.IsAny<Restriction>()), Times.Exactly(1));
            Assert.IsTrue(myResult);
        }

        [TestMethod]
        public void ShouldNotCreateRestrictionWithoutPermission() {
            Restriction myRestriction = theRestrictionModelBuilder.Name(RESTRICTION_NAME).Description(RESTRICTION_DESCRIPTION).Build().Restriction;

            bool myResult = theService.CreateRestriction(theUserInformationBuilder.Build(), myRestriction);
            theMockRepository.Verify(r => r.CreateRestriction(It.IsAny<User>(), It.IsAny<Restriction>()), Times.Never());
            Assert.IsFalse(myResult);
            var error = theModelState["PerformAction"].Errors[0];
            Assert.AreEqual("You are not allowed to perform that action.", error.ErrorMessage);
        }

        [TestMethod]
        public void ShouldNotCreateRestrictionBecauseNameIsRequired() {
            Restriction myRestriction = theRestrictionModelBuilder.Description(RESTRICTION_DESCRIPTION).Build().Restriction;

            bool myResult = theService.CreateRestriction(theUserInformationBuilder.Build(), myRestriction);

            theMockRepository.Verify(r => r.CreateRestriction(It.IsAny<User>(), It.IsAny<Restriction>()), Times.Never());
            Assert.IsFalse(myResult);
            var error = theModelState["Name"].Errors[0];
            Assert.AreEqual("Restriction name is required.", error.ErrorMessage);
        }

        [TestMethod]
        public void ShouldNotCreateRestrictionBecauseDescriptionIsRequired() {
            Restriction myRestriction = theRestrictionModelBuilder.Name(RESTRICTION_NAME).Build().Restriction;
            
            bool result = theService.CreateRestriction(theUserInformationBuilder.Build(), myRestriction);

            theMockRepository.Verify(r => r.CreateRestriction(It.IsAny<User>(), It.IsAny<Restriction>()), Times.Never());
            Assert.IsFalse(result);
            var error = theModelState["Description"].Errors[0];
            Assert.AreEqual("Restriction description is required.", error.ErrorMessage);
        }

        [TestMethod]
        public void ShouldEditRestriction() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Edit_Restriction);
            Restriction myRestriction = theRestrictionModelBuilder.Name(RESTRICTION_NAME).Description(RESTRICTION_DESCRIPTION).Build().Restriction;

            bool myResult = theService.EditRestriction(theUserInformationBuilder.Build(), myRestriction);

            theMockRepository.Verify(r => r.EditRestriction(It.IsAny<User>(), It.IsAny<Restriction>()), Times.Exactly(1));
            Assert.IsTrue(myResult);
        }

        [TestMethod]
        public void ShouldNotEditRestrictionWithoutPermission() {
            Restriction myRestriction = theRestrictionModelBuilder.Name(RESTRICTION_NAME).Description(RESTRICTION_DESCRIPTION).Build().Restriction;

            bool myResult = theService.EditRestriction(theUserInformationBuilder.Build(), myRestriction);

            theMockRepository.Verify(r => r.EditRestriction(It.IsAny<User>(), It.IsAny<Restriction>()), Times.Never());
            Assert.IsFalse(myResult);
            var error = theModelState["PerformAction"].Errors[0];
            Assert.AreEqual("You are not allowed to perform that action.", error.ErrorMessage);
        }

        [TestMethod]
        public void ShouldNotEditRestrictionBecauseNameIsRequired() {
            Restriction myRestriction = theRestrictionModelBuilder.Description(RESTRICTION_DESCRIPTION).Build().Restriction;

            bool myResult = theService.EditRestriction(theUserInformationBuilder.Build(), myRestriction);

            theMockRepository.Verify(r => r.EditRestriction(It.IsAny<User>(), It.IsAny<Restriction>()), Times.Never());
            Assert.IsFalse(myResult);
            var error = theModelState["Name"].Errors[0];
            Assert.AreEqual("Restriction name is required.", error.ErrorMessage);
        }

        [TestMethod]
        public void ShouldNotEditRestrictionBecauseDescriptionIsRequired() {
            Restriction myRestriction = theRestrictionModelBuilder.Name(RESTRICTION_NAME).Build().Restriction;

            bool result = theService.EditRestriction(theUserInformationBuilder.Build(), myRestriction);

            theMockRepository.Verify(r => r.EditRestriction(It.IsAny<User>(), It.IsAny<Restriction>()), Times.Never());
            Assert.IsFalse(result);
            var error = theModelState["Description"].Errors[0];
            Assert.AreEqual("Restriction description is required.", error.ErrorMessage);
        }

        [TestMethod]
        public void ShouldDeleteRestriction() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Delete_Restriction);

            bool myResult = theService.DeleteRestriction(theUserInformationBuilder.Build(), theRestrictionModelBuilder.Build().Restriction);

            theMockRepository.Verify(r => r.DeleteRestriction(It.IsAny<User>(), It.IsAny<Restriction>()), Times.Exactly(1));
            Assert.IsTrue(myResult);
        }

        [TestMethod]
        public void ShouldNotDeleteRestrictionWithoutPermission() {
            bool myResult = theService.DeleteRestriction(theUserInformationBuilder.Build(), theRestrictionModelBuilder.Build().Restriction);

            theMockRepository.Verify(r => r.DeleteRestriction(It.IsAny<User>(), It.IsAny<Restriction>()), Times.Never());
            Assert.IsFalse(myResult);
            var error = theModelState["PerformAction"].Errors[0];
            Assert.AreEqual("You are not allowed to perform that action.", error.ErrorMessage);
        }
    }
}
