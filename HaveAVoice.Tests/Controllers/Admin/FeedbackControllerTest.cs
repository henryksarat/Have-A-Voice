using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HaveAVoice.Controllers.Users;
using HaveAVoice.Services.UserFeatures;
using Moq;
using HaveAVoice.Tests.Helpers;
using HaveAVoice.Helpers;
using System.Web.Mvc;
using HaveAVoice.Models;

namespace HaveAVoice.Tests.Controllers.Admin {
    [TestClass]
    public class FeedbackControllerTest : ControllerTestCase {
        private static string FEEDBACK = "Best site ever!";

        private static FeedbackController theController;
        private Mock<IHAVFeedbackService> theMockedService;

        [TestInitialize]
        public void Initialize() {
            theMockedService = new Mock<IHAVFeedbackService>();
            theController = new FeedbackController(theMockedBaseService.Object, theMockedService.Object);
            theController.ControllerContext = GetControllerContext();
        }

        [TestMethod]
        public void ShouldLoadFeedback() {
            List<Feedback> myFeedback = new List<Feedback>();
            myFeedback.Add(Feedback.CreateFeedback(0, string.Empty, DateTime.UtcNow, 0));
            theMockedService.Setup(s => s.GetAllFeedback()).Returns(() => myFeedback);
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.View_Feedback);

            var myResult = theController.View() as ViewResult;

            AssertAuthenticatedCleanSuccess(myResult, "View");
        }

        [TestMethod]
        public void ShouldNotLoadFeedbackWithoutPermission() {
            var myResult = theController.View() as ViewResult;

            theMockedService.Verify(s => s.GetAllFeedback(), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadFeedbackNotLoggedIn() {
            MockNotLoggedIn();

            var myResult = theController.View() as ViewResult;

            theMockedService.Verify(s => s.GetAllFeedback(), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        public void ShouldLoadSubmitFeedbackPage() {
            var myResult = theController.Index() as ViewResult;

            AssertAuthenticatedCleanSuccess(myResult, "Index");
        }

        public void ShouldNotLoadSubmitFeedbackPageNotLoggedIn() {
            MockNotLoggedIn();

            var myResult = theController.Index() as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }
        /*
        public void ShouldSubmitFeedback() {
            var myResult = theController.Index(FEEDBACK) as ViewResult;

            theMockedService.Verify(s => s.AddFeedback(It.IsAny<User>(), It.IsAny<string>()), Times.Exactly(1));
            AssertAuthenticatedRedirection(myResult);
        }

        public void ShouldNotSubmitFeedbackNotLoggedIn() {
            MockNotLoggedIn();

            var myResult = theController.Index(FEEDBACK) as ViewResult;

            theMockedService.Verify(s => s.AddFeedback(It.IsAny<User>(), It.IsAny<string>()), Times.Never());
            AssertRedirection(myResult);
        }

        public void ShouldNotSubmitFeedBackBecauseOfException() {
            theMockedService.Setup(s => s.AddFeedback(It.IsAny<User>(), It.IsAny<string>())).Throws<Exception>();

            var myResult = theController.Index(FEEDBACK) as ViewResult;

            AssertAuthenticatedErrorLogReturnBack(myResult, "Index");
        }
         * */

        protected override Controller GetController() {
            return theController;
        }
    }
}
