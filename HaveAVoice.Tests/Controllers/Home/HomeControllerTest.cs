using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HaveAVoice.Services.UserFeatures;
using Moq;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Controllers.Home;
using HaveAVoice.Models.View;
using HaveAVoice.Models;
using HaveAVoice.Helpers.Enums;
using System.Web.Mvc;

namespace HaveAVoice.Tests.Controllers.Home {
    [TestClass]
    public class HomeControllerTest : ControllerTestCase {
        private Mock<IHAVHomeService> theMockedService;
        private Mock<IHAVHomeRepository> theMockRepository;
        private HomeController theController;
        private NotLoggedInModel theModel;

        [TestInitialize]
        public void Initialize() {
            theMockedService = new Mock<IHAVHomeService>();
            theMockRepository = new Mock<IHAVHomeRepository>();
            theController = new HomeController(theMockedService.Object, theMockedBaseService.Object);

            theModel = CreateEmptyModel();
        }

        [TestMethod]
        public void TestDisplayNotLoggedIn() {
            theMockedService.Setup(s => s.NotLoggedIn()).Returns(() => theModel);

            var myResult = theController.NotLoggedIn() as ViewResult;

            NotLoggedInModel myModel = (NotLoggedInModel)myResult.ViewData.Model;
            Assert.AreEqual("NotLoggedIn", myResult.ViewName);
            Assert.IsNotNull(myModel);
        }

        [TestMethod]
        public void TestDisplayNotLoggedIn_Exception() {
            theMockedService.Setup(s => s.NotLoggedIn()).Throws<Exception>();

            var myResult = theController.NotLoggedIn() as ViewResult;

            AssertErrorLogReturnBack("NotLoggedIn", myResult);
        }

        private NotLoggedInModel CreateEmptyModel() {
            return new NotLoggedInModel() {
                LikedIssues = new List<IssueWithDispositionModel>(),
                DislikedIssues = new List<IssueWithDispositionModel>(),
                NewestIssueReplys = new List<IssueReply>(),
                MostPopularIssueReplys = new List<IssueReply>()
            };
        }

        private static NotLoggedInModel PopulateModel() {
            List<IssueWithDispositionModel> theIssues = new List<IssueWithDispositionModel>();
            Issue theIssue = new Issue();
            IssueReply theIssueReply = new IssueReply();
            List<IssueReply> theIssueReplys = new List<IssueReply>();

            NotLoggedInModel myModel = new NotLoggedInModel() {
                LikedIssues = new List<IssueWithDispositionModel>(),
                DislikedIssues = new List<IssueWithDispositionModel>(),
                NewestIssueReplys = new List<IssueReply>(),
                MostPopularIssueReplys = new List<IssueReply>()
            };
            return myModel;
        }

        protected override System.Web.Mvc.Controller GetController() {
            return theController;
        }
    }
}
