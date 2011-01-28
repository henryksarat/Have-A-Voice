using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Moq;
using HaveAVoice.Repositories;
using HaveAVoice.Services;
using HaveAVoice.Validation;
using HaveAVoice.Helpers;
using HaveAVoice.Models;
using HaveAVoice.Controllers.Issues;
using HaveAVoice.Models.View;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Tests.Helpers;
using HaveAVoice.Tests.Models;

namespace HaveAVoice.Tests.Controllers.Issues {
    [TestClass]
    public class IssueControllerTest : ControllerTestCase {
        private static int ISSUE_ID = 1;
        private static int CORRECT_ISSUE_ID = 3;
        private static int INCORRECT_ISSUE_ID = 8;
        private static int CORRECT_ISSUE_REPLY_ID = 5;
        private static int INCORRECT_ISSUE_REPLY_ID = 6;

        private IHAVIssueService theService;
        private Mock<IHAVIssueService> theMockedService;
        private Mock<IHAVIssueRepository> theMockRepository;
        private IssueController theController;
        private Issue theIssue;
        private IssueModel theIssueModel;
        private IssueReplyDetailsModel theIssueReplyDetailsModel;

        /*
        [TestInitialize]
        public void Initialize() {
            theIssue = Issue.CreateIssue(2, "Issue Title", "Description of Issue", "Chicago", "IL", new DateTime(1987, 05, 03), 0, false);
            theIssue.User = theUserInformationBuilder.Build().Details;
            theIssueModel = new IssueModel(theIssue, null, null);
            theIssueReplyDetailsModel = new IssueReplyDetailsModel(new IssueReply(), new List<IssueReplyComment>());
            theMockedService = new Mock<IHAVIssueService>();
            theMockRepository = new Mock<IHAVIssueRepository>();

            theService = new HAVIssueService(new ModelStateWrapper(theModelState),
                                                               theMockRepository.Object,
                                                               theBaseRepository.Object);

            theController = new IssueController(theMockedService.Object, theMockedBaseService.Object);
            theController.ControllerContext = GetControllerContext();
        }

        #region "Index"

        [TestMethod]
        public void TestIssueIndex() {
            List<IssueWithDispositionModel> myIssues = new List<IssueWithDispositionModel>();
            IssueWithDispositionModel myModel = new IssueWithDispositionModel();
            myModel.Issue = theIssue;
            myModel.HasDisposition = false;
            myIssues.Add(myModel);
            theMockedService.Setup(s => s.GetIssues(It.IsAny<User>())).Returns(() => myIssues);

            var myResult = theController.Index() as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(myResult, "Index", myIssues);
        }

        [TestMethod]
        public void TestIssueIndex_Exception() {
            theMockedService.Setup(s => s.GetIssues(It.IsAny<User>())).Throws<Exception>();

            var myResult = theController.Index() as ViewResult;

            AssertAuthenticatedErrorLogWithRedirect(myResult);
        }

        [TestMethod]
        public void TestIssueIndex_NoLatestIssues() {
            List<IssueWithDispositionModel> myIssues = new List<IssueWithDispositionModel>();
            theMockedService.Setup(s => s.GetIssues(It.IsAny<User>())).Returns(() => myIssues);

            var myResult = theController.Index() as ViewResult;

            AssertAuthenticatedSuccessWithMessage(myResult, "Index", myIssues);
        }

        #endregion

        #region "Delete"

        [TestMethod]
        public void ShouldDeleteIssue() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Delete_Issue);
            var myResult = theController.Delete(ISSUE_ID) as ViewResult;

            theMockedService.Verify(s => s.DeleteIssue(It.IsAny<UserInformationModel>(), It.IsAny<int>()), Times.Exactly(1));
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotDeleteIssueNotLoggedInt() {
            MockNotLoggedIn();
            var myResult = theController.Delete(ISSUE_ID) as ViewResult;

            theMockedService.Verify(s => s.DeleteIssue(It.IsAny<UserInformationModel>(), It.IsAny<int>()), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotDeleteIssueWithoutPermission() {
            var myResult = theController.Delete(ISSUE_ID) as ViewResult;

            theMockedService.Verify(s => s.DeleteIssue(It.IsAny<UserInformationModel>(), It.IsAny<int>()), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldDeleteIssueWithAdminPermission() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Delete_Any_Issue);
            User myUser = DatamodelFactory.createUserModelBuilder().Build();
            myUser.Id = 100;
            theIssue.User = myUser;
            var myResult = theController.Delete(ISSUE_ID) as ViewResult;

            theMockedService.Verify(s => s.DeleteIssue(It.IsAny<UserInformationModel>(), It.IsAny<int>()), Times.Exactly(1));
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotDeleteIssueBecauseOfException() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Delete_Any_Issue);
            theMockedService.Setup(s => s.DeleteIssue(It.IsAny<UserInformationModel>(), It.IsAny<int>())).Throws<Exception>();

            var myResult = theController.Delete(ISSUE_ID) as ViewResult;

            theMockedService.Verify(s => s.DeleteIssue(It.IsAny<UserInformationModel>(), It.IsAny<int>()), Times.Exactly(1));
            AssertAuthenticatedErrorLogWithRedirect(myResult);
        }

        #endregion

        [TestMethod]
        public void ShouldLoadCreateIssuePage() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Post_Issue);

            var myResult = theController.Create() as ViewResult;

            AssertAuthenticatedCleanSuccess(myResult, "Create");
        }

        [TestMethod]
        public void ShouldNotLoadCreateIssuePageWithoutPermission() {
            var myResult = theController.Create() as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadCreateIssuePageNotLoggedIn() {
            theMockUserInformation.Setup(f => f.GetUserInformaton()).Returns(() => null);

            var myResult = theController.Create() as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }
        /*
        [TestMethod]
        public void ShouldCreateIssue() {
            theMockedService.Setup(s => s.CreateIssue(It.IsAny<UserInformationModel>(), It.IsAny<Issue>())).Returns(() => true);

            var myResult = theController.Create(theIssue) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotCreateIssueNotLoggedIn() {
            theMockUserInformation.Setup(f => f.GetUserInformaton()).Returns(() => null);

            var myResult = theController.Create(theIssue) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotCreateIssueBecauseOfInvalidation() {
            theMockedService.Setup(s => s.CreateIssue(It.IsAny<UserInformationModel>(), It.IsAny<Issue>())).Returns(() => false);

            var myResult = theController.Create(theIssue) as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(myResult, "Create", theIssue);
        }

        [TestMethod]
        public void ShouldNotCreateIssueBecauseOfException() {
            theMockedService.Setup(s => s.CreateIssue(It.IsAny<UserInformationModel>(), It.IsAny<Issue>())).Throws<Exception>();

            var myResult = theController.Create(theIssue) as ViewResult;

            AssertAuthenticatedErrorLogReturnBack(myResult, "Create");
        }
        [TestMethod]
        public void ShouldLoadViewIssuePage() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.View_Issue);
            theMockedService.Setup(s => s.GetIssue(CORRECT_ISSUE_ID)).Returns(() => theIssue);
            List<IssueReplyModel> issueReplys = new List<IssueReplyModel>();
            theMockedService.Setup(s => s.GetReplysToIssue(It.IsAny<User>(), It.IsAny<Issue>(), It.IsAny<List<string>>())).Returns(() => issueReplys);

            var myResult = theController.View(CORRECT_ISSUE_ID) as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(myResult, "View", theIssueModel);
        }

        [TestMethod]
        public void ShouldNotLoadViewIssuePageNotLoggedIn() {
            MockNotLoggedIn();

            var myResult = theController.View(CORRECT_ISSUE_ID) as ViewResult;
            
            theMockedService.Verify(s => s.GetIssue(It.IsAny<int>()), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadViewIssuePageWithoutPermission() {
            var myResult = theController.View(CORRECT_ISSUE_ID) as ViewResult;

            theMockedService.Verify(s => s.GetIssue(It.IsAny<int>()), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadViewIssuePageBecauseOfException() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.View_Issue);
            theMockedService.Setup(s => s.GetIssue(CORRECT_ISSUE_ID)).Throws<Exception>();

            var myResult = theController.View(CORRECT_ISSUE_ID) as ViewResult;

            AssertAuthenticatedErrorLogWithRedirect(myResult);
        }

        [TestMethod]
        public void ShouldPostIssueReply() {
            theMockedService.Setup(s => s.CreateIssueReply(It.IsAny<UserInformationModel>(), It.IsAny<IssueModel>())).Returns(() => true);

            var myResult = theController.View(theIssueModel) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadViewIssuePageBecauseIssueDoesntExist() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.View_Issue);
            theMockedService.Setup(s => s.GetIssue(CORRECT_ISSUE_ID)).Returns(() => theIssue);

            var myResult = theController.View(INCORRECT_ISSUE_ID) as ViewResult;

            theMockedService.Verify(s => s.GetReplysToIssue(It.IsAny<User>(), It.IsAny<Issue>(), It.IsAny<List<string>>()), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }
         * 
        [TestMethod]
        public void ShouldNotPostIssueReplyNotLoggedIn() {
            theMockUserInformation.Setup(f => f.GetUserInformaton()).Returns(() => null);

            var myResult = theController.View(theIssueModel) as ViewResult;

            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotPostIssueReplyBecuaseOfInvalidation() {
            theMockedService.Setup(s => s.CreateIssueReply(It.IsAny<UserInformationModel>(), It.IsAny<IssueModel>())).Returns(() => false);

            var myResult = theController.View(theIssueModel) as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(myResult, "View", theIssueModel);
        }

        [TestMethod]
        public void ShouldNotPostIssueReplyBecuaseOfException() {
            theMockedService.Setup(s => s.CreateIssueReply(It.IsAny<UserInformationModel>(), It.IsAny<IssueModel>())).Throws<Exception>();

            var myResult = theController.View(theIssueModel) as ViewResult;

            AssertAuthenticatedErrorLogReturnBack(myResult, "View");
        }

        [TestMethod]
        public void ShouldLoadEditIssuePageForReplyAuthor() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Edit_Issue);
            theMockedService.Setup(s => s.GetIssue(It.IsAny<int>())).Returns(() => theIssue);

            var myResult = theController.Edit(theIssue.Id) as ViewResult;

            AssertAuthenticatedCleanSuccessWithReturn(myResult, "Edit", theIssue);
        }

        [TestMethod]
        public void ShouldNotLoadEditIssuePageWithoutPermission() {
            var myResult = theController.Edit(theIssue.Id) as ViewResult;

            theMockedService.Verify(s => s.GetIssueReply(It.IsAny<int>()), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotLoadEditIssuePageNotLoggedIn() {
            MockNotLoggedIn();
            var myResult = theController.Edit(theIssue.Id) as ViewResult;

            theMockedService.Verify(s => s.GetIssueReply(It.IsAny<int>()), Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldLoadEditIssuePageForAdminWithPermission() {
            User myUser = DatamodelFactory.createUserModelBuilder().Build();
            myUser.Id = 100;
            theIssue.User = myUser;
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Edit_Any_Issue);
            theMockedService.Setup(s => s.GetIssue(It.IsAny<int>())).Returns(() => theIssue);

            var myResult = theController.Edit(theIssue.Id) as ViewResult;

            theMockedService.Verify(s => s.GetIssue(It.IsAny<int>()), Times.Exactly(1));
            AssertAuthenticatedCleanSuccessWithReturn(myResult, "Edit", theIssue);
        }
        /*
        [TestMethod]
        public void ShouldEditIssue() {
            theMockedService.Setup(s => s.EditIssue(It.IsAny<UserInformationModel>(), It.IsAny<Issue>())).Returns(() => true);

            var myResult = theController.Edit(theIssue) as ViewResult;

            VerifyEdit(Times.Exactly(1));
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotEditIssueNotLoggedIn() {
            MockNotLoggedIn();
            var myResult = theController.Edit(theIssue) as ViewResult;

            VerifyEdit(Times.Never());
            AssertAuthenticatedRedirection(myResult);
        }

        [TestMethod]
        public void ShouldNotEditIssueBecauseOfException() {
            theMockedService.Setup(s => s.EditIssue(It.IsAny<UserInformationModel>(), It.IsAny<Issue>())).Throws<Exception>();
            var myResult = theController.Edit(theIssue) as ViewResult;

            VerifyEdit(Times.Exactly(1));
            AssertAuthenticatedErrorLogReturnBack(myResult, "Edit", theIssue);
        }

        [TestMethod]
        public void ShouldNotEditBecauseOfInvalidation() {
            theMockedService.Setup(s => s.EditIssue(It.IsAny<UserInformationModel>(), It.IsAny<Issue>())).Returns(() => false);
            var myResult = theController.Edit(theIssue) as ViewResult;

            VerifyEdit(Times.Exactly(1));
            AssertAuthenticatedFailWithReturnBack(myResult, "Edit", theIssue);
        }
         * */

        private void VerifyEdit(Times aTimes) {
            theMockedService.Verify(s => s.EditIssue(It.IsAny<UserInformationModel>(), It.IsAny<Issue>()), aTimes);
        }

        protected override Controller GetController() {
            return theController;
        }
    }
}
