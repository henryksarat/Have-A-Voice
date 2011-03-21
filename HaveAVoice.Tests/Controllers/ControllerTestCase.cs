using System;
using System.Web.Mvc;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Models;
using HaveAVoice.Repositories;
using HaveAVoice.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using HaveAVoice.Models.View;
using HaveAVoice.Helpers;
using System.Collections.Generic;

namespace HaveAVoice.Tests.Controllers {
    [TestClass]
    public abstract class ControllerTestCase {
        /*
        private static Object REDIRECT_NOT_MOCKED = null;

        protected UserInformationModelBuilder theUserInformationBuilder = new UserInformationModelBuilder(new User());
        protected ModelStateDictionary theModelState;
        protected Mock<IUserInformation> theMockUserInformation;
        protected Mock<IHAVBaseService> theMockedBaseService;
        protected Mock<IHAVBaseRepository> theBaseRepository;

        [TestInitialize]
        public void Initilize() {
            theModelState = new ModelStateDictionary();
            theMockUserInformation = new Mock<IUserInformation>();
            theMockedBaseService = new Mock<IHAVBaseService>();
            theBaseRepository = new Mock<IHAVBaseRepository>();
            User myUser = new User();
            theMockUserInformation.Setup(f => f.GetUserInformaton()).Returns(() => theUserInformationBuilder.Build());

            //HAVUserInformationFactory.SetInstance(theMockUserInformation.Object);
        }

        protected ControllerContext GetControllerContext() {
            var myContext = new Mock<ControllerContext>();
            myContext.SetupGet(c => c.HttpContext.Session["EVERYTHING"]);
            myContext.Setup(c => c.HttpContext.Request.UserHostAddress).Returns(() => "127.0.0.1");
            return myContext.Object;
        }

        protected abstract Controller GetController();

        protected void MockNotLoggedIn() {
            theMockUserInformation.Setup(u => u.GetUserInformaton()).Returns(() => null);
        }


        protected void AssertAuthenticatedSuccessWithMessage(ViewResult aResult, string aViewName, object aClass) {
            Assert.IsNotNull(GetController().ViewData["Message"]);
            AssertAuthenticatedSuccessWithReturn(aResult, aViewName, aClass);
        }

        protected void AssertAuthenticatedCleanSuccessWithReturn(ViewResult aResult, string aViewName, object aClass) {
            Assert.IsNull(GetController().ViewData["Message"]);
            AssertAuthenticatedSuccessWithReturn(aResult, aViewName, aClass);
        }

        protected void AssertAuthenticatedFailWithReturnBack(ViewResult aResult, string aViewName, object aClass) {
            Assert.IsNull(GetController().ViewData["Message"]);
            AssertAuthenticatedSuccessWithReturn(aResult, aViewName, aClass);
        }

        protected void AssertAuthenticatedCleanSuccess(ViewResult aResult, string aViewName) {
            Assert.IsNull(GetController().ViewData["Message"]);
            theMockUserInformation.Verify(u => u.GetUserInformaton(), Times.AtLeastOnce());
            Assert.AreEqual(aViewName, aResult.ViewName);
        }

        protected void AssertAuthenticatedRedirection(ViewResult aResult) {
            theMockUserInformation.Verify(u => u.GetUserInformaton(), Times.AtLeastOnce());
            Assert.AreEqual(REDIRECT_NOT_MOCKED, aResult);
            Assert.IsNull(GetController().ViewData["Message"]);
        }

        protected void AssertAuthenticatedRedirectionWithTempData(ViewResult aResult) {
            theMockUserInformation.Verify(u => u.GetUserInformaton(), Times.AtLeastOnce());
            Assert.AreEqual(REDIRECT_NOT_MOCKED, aResult);
            Assert.IsNotNull(GetController().TempData["Message"]);
        }

        protected void AssertAuthenticatedErrorLogWithRedirect(ViewResult aResult) {
            Assert.AreEqual(REDIRECT_NOT_MOCKED, aResult);
            Assert.IsNull(GetController().ViewData["Message"]);
            AssertAuthenticatedErrorLog();

        }

        protected void AssertAuthenticatedErrorLogReturnBack(ViewResult aResult, string aViewName) {
            Assert.AreEqual(aViewName, aResult.ViewName);
            Assert.IsNotNull(GetController().ViewData["Message"]);
            AssertAuthenticatedErrorLog();
        }

        protected void AssertAuthenticatedErrorLogReturnBack(ViewResult aResult, string aViewName, object aClass) {
            Assert.AreEqual(aViewName, aResult.ViewName);
            Assert.IsInstanceOfType(aResult.ViewData.Model, aClass.GetType());
            Assert.IsNotNull(GetController().ViewData["Message"]);
            AssertAuthenticatedErrorLog();
        }

        protected void AssertErrorLogReturnBack(string aViewName, ViewResult aResult) {
            Assert.AreEqual(aViewName, aResult.ViewName);
            Assert.IsNotNull(GetController().ViewData["Message"]);
            AssertErrorLog();
        }

        protected void AssertErrorWithRedirect(ViewResult aResult) {
            AssertRedirection(aResult);
            AssertErrorLog();
        }

        protected void AssertSuccessWithMessage(ViewResult aResult, string aViewName, object aClass) {
            Assert.IsNotNull(GetController().ViewData["Message"]);
            AssertSuccessWithReturn(aResult, aViewName, aClass);
        }

        protected void AssertCleanSuccess(ViewResult aResult, string aViewName, object aClass) {
            Assert.IsNull(GetController().ViewData["Message"]);
            AssertSuccessWithReturn(aResult, aViewName, aClass);
        }

        protected void AssertRedirection(ViewResult aResult) {
            Assert.AreEqual(REDIRECT_NOT_MOCKED, aResult);
            Assert.IsNull(GetController().ViewData["Message"]);
        }

        protected void AssertErrorLogWithRedirect(ViewResult aResult) {
            Assert.AreEqual(REDIRECT_NOT_MOCKED, aResult);
            Assert.IsNull(GetController().ViewData["Message"]);
            AssertErrorLog();
        }

        protected void AssertErrorLogWithReturn(string aViewName, ViewResult aResult) {
            Assert.AreEqual(aViewName, aResult.ViewName);
            Assert.IsNotNull(GetController().ViewData["Message"]);
            AssertErrorLog();
        }

        private void AssertAuthenticatedErrorLog() {
            theMockUserInformation.Verify(u => u.GetUserInformaton(), Times.AtLeastOnce());
            AssertErrorLog();
        }

        private void AssertErrorLog() {
            theMockedBaseService.Verify(b => b.LogError(It.IsAny<Exception>(), It.IsAny<string>()), Times.Exactly(1));
        }

        private void AssertAuthenticatedSuccessWithReturn(ViewResult aResult, string aViewName, object aClass) {
            theMockUserInformation.Verify(u => u.GetUserInformaton(), Times.AtLeastOnce());
            AssertSuccessWithReturn(aResult, aViewName, aClass);
        }

        private void AssertSuccessWithReturn(ViewResult aResult, string aViewName, object aClass) {
            Assert.IsInstanceOfType(aResult.ViewData.Model, aClass.GetType());
            AssertSuccess(aResult, aViewName);
        }

        private void AssertSuccess(ViewResult aResult, string aViewName) {
            Assert.AreEqual(aViewName, aResult.ViewName);
        }

        protected void AssertFailReturnBack(ViewResult aResult, string aViewName, object aClass) {
            Assert.IsNull(GetController().ViewData["Message"]);
            Assert.AreEqual(aViewName, aResult.ViewName);
            Assert.IsInstanceOfType(aResult.ViewData.Model, aClass.GetType());
        }*/
    }
}
