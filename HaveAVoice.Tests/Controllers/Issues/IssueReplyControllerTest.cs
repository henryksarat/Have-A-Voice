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
using HaveAVoice.Services.Issues;

namespace HaveAVoice.Tests.Controllers.Issues {
    [TestClass]
    public class IssueReplyControllerTest : ControllerTestCase {
        private static int ISSUE_ID = 4;
        private static int REPLY_ID = 1;
        private static string REPLY = "Whatever";
        private static string DISPOSITION = "LIKE";

        private IssueReplyController theController;
        private Mock<IHAVIssueService> theMockedService;
        private IssueReply theIssueReply;
        /*

        [TestInitialize]
        public void Initialize() {
            theIssueReply = IssueReply.CreateIssueReply(0, 0, 0, string.Empty, "Chicago", "IL", (int)Disposition.None, false, DateTime.UtcNow, false);
            theIssueReply.User = theUserInformationBuilder.Build().Details;
            theMockedService = new Mock<IHAVIssueService>();
            theController = new IssueReplyController(theMockedService.Object, theMockedBaseService.Object);
            theController.ControllerContext = GetControllerContext();
        }

        #region "Delete"

        [TestMethod]
        public void ShouldDeleteReply() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Delete_Issue_Reply);
            var myResult = theController.Delete(ISSUE_ID, REPLY_ID) as ViewResult;

            theMockedService.Verify(s => s.DeleteIssueReply(It.IsAny<UserInformationModel>(), It.IsAny<int>()), Times.Exactly(1));
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotDeleteReplyNotLoggedInt() {
            MockNotLoggedIn();
            var myResult = theController.Delete(ISSUE_ID, REPLY_ID) as ViewResult;

            theMockedService.Verify(s => s.DeleteIssueReply(It.IsAny<UserInformationModel>(), It.IsAny<int>()), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotDeleteReplyWithoutPermission() {
            var myResult = theController.Delete(ISSUE_ID, REPLY_ID) as ViewResult;

            theMockedService.Verify(s => s.DeleteIssueReply(It.IsAny<UserInformationModel>(), It.IsAny<int>()), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldDeleteIssueReplyWithAdminPermission() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Delete_Any_Issue_Reply);
            User myUser = DatamodelFactory.createUserModelBuilder().Build();
            myUser.Id = 100;
            theIssueReply.User = myUser;
            var myResult = theController.Delete(ISSUE_ID, REPLY_ID) as ViewResult;

            theMockedService.Verify(s => s.DeleteIssueReply(It.IsAny<UserInformationModel>(), It.IsAny<int>()), Times.Exactly(1));
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotDeleteIssueReplyBecauseOfException() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Delete_Any_Issue_Reply);
            theMockedService.Setup(s => s.DeleteIssueReply(It.IsAny<UserInformationModel>(), It.IsAny<int>())).Throws<Exception>();

            var myResult = theController.Delete(ISSUE_ID, REPLY_ID) as ViewResult;

            theMockedService.Verify(s => s.DeleteIssueReply(It.IsAny<UserInformationModel>(), It.IsAny<int>()), Times.Exactly(1));
            AssertAuthenticatedRedirectionWithTempData(myResult);
        }

        #endregion

        #region "Edit"

        [TestMethod]
        public void ShouldLoadEditIssueReplyPageForReplyAuthor() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Edit_Issue_Reply);
            theMockedService.Setup(s => s.GetIssueReply(It.IsAny<int>())).Returns(() => theIssueReply);

            var myResult = theController.Edit(REPLY_ID) as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(myResult, "Edit", theIssueReply);
        }

        [TestMethod]
        public void ShouldNotLoadEditIssueReplyPageWithoutPermission() {
            var myResult = theController.Edit(REPLY_ID) as ViewResult;

            theMockedService.Verify(s => s.GetIssueReply(It.IsAny<int>()), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadEditIssueReplyPageNotLoggedIn() {
            MockNotLoggedIn();
            var myResult = theController.Edit(REPLY_ID) as ViewResult;

            theMockedService.Verify(s => s.GetIssueReply(It.IsAny<int>()), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldLoadEditIssueReplyPageForAdminWithPermission() {
            User myUser = DatamodelFactory.createUserModelBuilder().Build();
            myUser.Id = 100;
            theIssueReply.User = myUser;
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Edit_Any_Issue_Reply);
            theMockedService.Setup(s => s.GetIssueReply(It.IsAny<int>())).Returns(() => theIssueReply);

            var myResult = theController.Edit(REPLY_ID) as ViewResult;

            theMockedService.Verify(s => s.GetIssueReply(It.IsAny<int>()), Times.Exactly(1));
            AssertAuthenticatedCleanSuccessWithReturn(myResult, "Edit", theIssueReply);
        }
        [TestMethod]
        public void ShouldEditReply() {
            theMockedService.Setup(s => s.EditIssueReply(It.IsAny<UserInformationModel>(), It.IsAny<IssueReply>())).Returns(() => true);

            var myResult = theController.Edit(theIssueReply) as ViewResult;

            VerifyEdit(Times.Exactly(1));
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotEditReplyNotLoggedIn() {
            MockNotLoggedIn();
            var myResult = theController.Edit(theIssueReply) as ViewResult;

            VerifyEdit(Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotEditReplyBecauseOfException() {
            theMockedService.Setup(s => s.EditIssueReply(It.IsAny<UserInformationModel>(), It.IsAny<IssueReply>())).Throws<Exception>();
            var myResult = theController.Edit(theIssueReply) as ViewResult;

            VerifyEdit(Times.Exactly(1));
            AssertAuthenticatedErrorLogReturnBack(myResult, "Edit", theIssueReply);
        }

        [TestMethod]
        public void ShouldNotEditReplyBecauseOfInvalidation() {
            theMockedService.Setup(s => s.EditIssueReply(It.IsAny<UserInformationModel>(), It.IsAny<IssueReply>())).Returns(() => false);
            var myResult = theController.Edit(theIssueReply) as ViewResult;

            VerifyEdit(Times.Exactly(1));
            AssertAuthenticatedFailWithReturnBack(myResult, "Edit", theIssueReply);
        }

        #region "View"

        [TestMethod]
        public void ShouldLoadIssueReplyPage() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.View_Issue_Reply);
            theMockedService.Setup(s => s.GetIssueReply(REPLY_ID)).Returns(() => theIssueReply);

            var myResult = theController.View(REPLY_ID) as ViewResult;

            theMockedService.Verify(s => s.GetIssueReply(REPLY_ID), Times.Exactly(1));
            AssertAuthenticatedCleanSuccessWithReturn(myResult, "View", theIssueReplyDetailsModel);
        }

        [TestMethod]
        public void ShouldNotLoadIssueReplyPageNotLoggedIn() {
            MockNotLoggedIn();
            var myResult = theController.View(REPLY_ID) as ViewResult;

            theMockedService.Verify(s => s.GetIssueReply(It.IsAny<int>()), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadIssueReplyPageWithoutPermission() {
            var myResult = theController.View(REPLY_ID) as ViewResult;

            theMockedService.Verify(s => s.GetIssueReply(It.IsAny<int>()), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadViewIssueReplyPageBecuaseIssueReplyDoesntExist() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.View_Issue_Reply);
            theMockedService.Setup(s => s.GetIssueReply(REPLY_ID)).Returns(() => new IssueReply());

            var myResult = theController.View(REPLY_ID+1) as ViewResult;

            theMockedService.Verify(s => s.GetIssueReply(It.IsAny<int>()), Times.Exactly(1));
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadViewIssueReplyPageBecauseOfException() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.View_Issue_Reply);
            theMockedService.Setup(s => s.GetIssueReply(REPLY_ID)).Throws<Exception>();

            var myResult = theController.View(REPLY_ID) as ViewResult;

            AssertAuthenticatedErrorLogWithRedirect(myResult);
        }

        [TestMethod]
        public void ShouldPostCommentToIssueReply() {
            theMockedService.Setup(s => s.CreateCommentToIssueReply(It.IsAny<UserInformationModel>(), It.IsAny<IssueReplyDetailsModel>())).Returns(() => true);
            var myResult = theController.View(theIssueReplyDetailsModel) as ViewResult;

            theMockedService.Verify(s => s.CreateCommentToIssueReply(It.IsAny<UserInformationModel>(), It.IsAny<IssueReplyDetailsModel>()), Times.Exactly(1));
            AssertAuthenticatedSuccessWithMessage(myResult, "View", theIssueReplyDetailsModel);
        }

        [TestMethod]
        public void ShouldNotPostCommentToIssueReplyNotLoggedIn() {
            MockNotLoggedIn();
            var myResult = theController.View(theIssueReplyDetailsModel) as ViewResult;

            theMockedService.Verify(s => s.CreateCommentToIssueReply(It.IsAny<UserInformationModel>(), It.IsAny<IssueReplyDetailsModel>()), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotPostCommentToIssueReplyBecauseOfInvalidation() {
            theMockedService.Setup(s => s.CreateCommentToIssueReply(It.IsAny<UserInformationModel>(), It.IsAny<IssueReplyDetailsModel>())).Returns(() => false);
            var myResult = theController.View(theIssueReplyDetailsModel) as ViewResult;

            AssertAuthenticatedFailWithReturnBack(myResult, "View", theIssueReplyDetailsModel);
        }

        [TestMethod]
        public void ShouldNotPostCommentToIssueReplyBecauseOfException() {
            theMockedService.Setup(s => s.CreateCommentToIssueReply(It.IsAny<UserInformationModel>(), It.IsAny<IssueReplyDetailsModel>())).Throws<Exception>();

            var myResult = theController.View(theIssueReplyDetailsModel) as ViewResult;

            AssertAuthenticatedErrorLogReturnBack(myResult, "View");
        }

        #endregion

        private void VerifyEdit(Times aTimes) {
            theMockedService.Verify(s => s.EditIssueReply(It.IsAny<UserInformationModel>(), It.IsAny<IssueReply>()), aTimes);
        }
        */

        protected override Controller GetController() {
            return theController;
        }
    }
}
