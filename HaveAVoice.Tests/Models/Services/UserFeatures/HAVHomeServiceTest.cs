using System;
using System.Web.Mvc;
using HaveAVoice.Exceptions;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Models;
using HaveAVoice.Models.View;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web;
using HaveAVoice.Repositories.AdminFeatures;
using System.Collections.Generic;

namespace HaveAVoice.Tests.Models.Services.UserFeatures {
    [TestClass]
    public class HAVHomeServiceTest {
        private static User USER = DatamodelFactory.createUserModelBuilder().Build();
        private static string ZIP_CODE = "60630";
        private static string CITY = "Chicago";
        private static string STATE = "IL";


        private ModelStateDictionary theModelState;
        private IHAVHomeService theService;
        private Mock<IHAVFanService> theFanService;
        private Mock<IHAVHomeRepository> theMockRepository;
        private Mock<IHAVBaseRepository> theBaseRepository;
        private Mock<IHAVBaseService> theBaseService;

        [TestInitialize]
        public void Initialize() {
            theModelState = new ModelStateDictionary();

            theMockRepository = new Mock<IHAVHomeRepository>();
            theBaseRepository = new Mock<IHAVBaseRepository>();
            theBaseService = new Mock<IHAVBaseService>();
            theFanService = new Mock<IHAVFanService>();

            theService = new HAVHomeService(new ModelStateWrapper(theModelState), theFanService.Object, theMockRepository.Object, theBaseRepository.Object);
        }

        [TestMethod]
        public void CreateValidZipCodeFilter() {
            bool myResult = theService.AddZipCodeFilter(USER, ZIP_CODE);

            Assert.IsTrue(myResult);
        }

        [TestMethod]
        public void CreateZipCodeFilter_InvalidCharacter() {
            bool myResult = theService.AddZipCodeFilter(USER, "60630a");

            Assert.IsFalse(myResult);
            var myError = theModelState["ZipCode"].Errors[0];
            Assert.AreEqual("Invalid zip code.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateZipCodeFilter_InvalidLength() {
            bool myResult = theService.AddZipCodeFilter(USER, "606300");

            Assert.IsFalse(myResult);
            var myError = theModelState["ZipCode"].Errors[0];
            Assert.AreEqual("Invalid zip code.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateZipCodeFilter_NoZipCode() {
            bool myResult = theService.AddZipCodeFilter(USER, string.Empty);

            Assert.IsFalse(myResult);
            var myError = theModelState["ZipCode"].Errors[0];
            Assert.AreEqual("Invalid zip code.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateExistingZipCode() {
            theMockRepository.Setup(r => r.ZipCodeFilterExists(USER, Int32.Parse(ZIP_CODE))).Returns(() => true);
            bool myResult = theService.AddZipCodeFilter(USER, ZIP_CODE);

            Assert.IsFalse(myResult);
            var myError = theModelState["ZipCode"].Errors[0];
            Assert.AreEqual("This zip code filter already exists.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateValidCityStateFilter() {
            bool myResult = theService.AddCityStateFilter(USER, CITY, STATE);

            Assert.IsTrue(myResult);
        }

        [TestMethod]
        public void CreateCityStateFilter_NoCity() {
            bool myResult = theService.AddCityStateFilter(USER, string.Empty, STATE);

            Assert.IsFalse(myResult);
            var myError = theModelState["City"].Errors[0];
            Assert.AreEqual("Invalid city.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateCityStateFilter_NoState() {
            bool myResult = theService.AddCityStateFilter(USER, CITY, string.Empty);

            Assert.IsFalse(myResult);
            var myError = theModelState["State"].Errors[0];
            Assert.AreEqual("Invalid state.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateCityStateFilter_ExistingCityStateFilter() {
            theMockRepository.Setup(r => r.CityStateFilterExists(USER, CITY, STATE)).Returns(() => true);
            bool myResult = theService.AddCityStateFilter(USER, CITY, STATE);

            Assert.IsFalse(myResult);
            var myError = theModelState["City"].Errors[0];
            Assert.AreEqual("That city/state filter already exists.", myError.ErrorMessage);
        }
    }
}
