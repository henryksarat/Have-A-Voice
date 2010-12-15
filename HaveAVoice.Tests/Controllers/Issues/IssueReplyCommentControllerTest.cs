using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HaveAVoice.Services.UserFeatures;
using Moq;
using HaveAVoice.Controllers.Issues;
using HaveAVoice.Models;
using HaveAVoice.Helpers.Enums;
using System.Web.Mvc;
using HaveAVoice.Tests.Helpers;
using HaveAVoice.Helpers;
using HaveAVoice.Tests.Models;
using HaveAVoice.Models.View;

namespace HaveAVoice.Tests.Controllers.Issues {
    [TestClass]
    public class IssueReplyCommentControllerTest : ControllerTestCase {
        private static int ID = 1;
        private static string COMMENT = "Whatever";

        private IssueReplyCommentController theController;
        private Mock<IHAVIssueService> theMockedService;
        private IssueReplyComment theComment;

        [TestInitialize]
        public void Initialize() {
            theComment = IssueReplyComment.CreateIssueReplyComment(0, 0, COMMENT, DateTime.UtcNow, 0, false);
            theComment.User = theUserInformationBuilder.Build().Details;
            theMockedService = new Mock<IHAVIssueService>();
            theController = new IssueReplyCommentController(theMockedService.Object, theMockedBaseService.Object);
            theController.ControllerContext = GetControllerContext();
        }

        [TestMethod]
        public void ShouldLoadEditIssueReplyCommentPageForCommentAuthor() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Edit_Issue_Reply_Comment);
            theMockedService.Setup(s => s.GetIssueReplyComment(It.IsAny<int>())).Returns(() => theComment);

            var myResult = theController.Edit(ID) as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(myResult, "Edit", theComment);
        }

        [TestMethod]
        public void ShouldNotLoadEditIssueReplyCommentPageWithoutPermission() {
            var myResult = theController.Edit(ID) as ViewResult;

            theMockedService.Verify(s => s.GetIssueReplyComment(It.IsAny<int>()), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadEditIssueReplyCommentPageNotLoggedIn() {
            MockNotLoggedIn();
            var myResult = theController.Edit(ID) as ViewResult;

            theMockedService.Verify(s => s.GetIssueReplyComment(It.IsAny<int>()), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldLoadEditIssueReplyCommentPageForAdminWithPermission() {
            User myUser = DatamodelFactory.createUserModelBuilder().Build();
            myUser.Id = 100;
            theComment.User = myUser;
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Edit_Any_Issue_Reply_Comment);
            theMockedService.Setup(s => s.GetIssueReplyComment(It.IsAny<int>())).Returns(() => theComment);

            var myResult = theController.Edit(ID) as ViewResult;

            theMockedService.Verify(s => s.GetIssueReplyComment(It.IsAny<int>()), Times.Exactly(1));
            AssertAuthenticatedCleanSuccessWithReturn(myResult, "Edit", theComment);
        }

        [TestMethod]
        public void ShouldEditComment() {
            theMockedService.Setup(s => s.EditIssueReplyComment(It.IsAny<UserInformationModel>(), It.IsAny<IssueReplyComment>())).Returns(() => true);

            var myResult = theController.Edit(theComment) as ViewResult;

            VerifyEdit(Times.Exactly(1));
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotEditCommentNotLoggedIn() {
            MockNotLoggedIn();
            var myResult = theController.Edit(theComment) as ViewResult;

            VerifyEdit(Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotEditCommentBecauseOfException() {
            theMockedService.Setup(s => s.EditIssueReplyComment(It.IsAny<UserInformationModel>(), It.IsAny<IssueReplyComment>())).Throws<Exception>();
            var myResult = theController.Edit(theComment) as ViewResult;

            VerifyEdit(Times.Exactly(1));
            AssertAuthenticatedErrorLogReturnBack(myResult, "Edit", theComment);
        }

        [TestMethod]
        public void ShouldNotEditCommentBecauseOfInvalidation() {
            theMockedService.Setup(s => s.EditIssueReplyComment(It.IsAny<UserInformationModel>(), It.IsAny<IssueReplyComment>())).Returns(() => false);
            var myResult = theController.Edit(theComment) as ViewResult;

            VerifyEdit(Times.Exactly(1));
            AssertAuthenticatedFailWithReturnBack(myResult, "Edit", theComment);
        }

        private void VerifyEdit(Times aTimes) {
            theMockedService.Verify(s => s.EditIssueReplyComment(It.IsAny<UserInformationModel>(), It.IsAny<IssueReplyComment>()), aTimes);
        }

        protected override Controller GetController() {
            return theController;
        }
    }
}
