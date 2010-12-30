using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Moq;
using HaveAVoice.Validation;
using HaveAVoice.Services;
using HaveAVoice.Repositories;
using HaveAVoice.Models;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Models.View;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Models.View.Builders;
using HaveAVoice.Helpers;
using System.Collections.Generic;
using HaveAVoice.Tests.Helpers;


namespace HaveAVoice.Tests.Models.Services.UserFeatures {
    [TestClass]
    public class HAVIssueServiceTest {
        private static int ID = 1;
        private static string REPLY = "reply";
        private static Disposition DISPOSITION = Disposition.LIKE;
        private static string ISSUE_TITLE = "Ban on BP";
        private static string ISSUE_DESCRIPTION = "BP should not be able to drill offshore!";
        private static string ISSUE_REPLY = "I think that this is stupid because BP gives everyone money.";
        private static Disposition ISSUE_DISPOSITION = Disposition.DISLIKE;
        private static DateTime ISSUE_POST_DATE = new DateTime(2010, 05, 03);
        private static User USER = new User();
        public static Permission POST_ISSUE_PERMISSION = Permission.CreatePermission(0, HAVPermission.Post_Issue.ToString(), string.Empty, false);
        public static Permission POST_ISSUE_REPLY_PERMISSION = 
            Permission.CreatePermission(0, HAVPermission.Post_Issue_Reply.ToString(), string.Empty, false);

        private UserInformationModelBuilder theUserInformationBuilder = new UserInformationModelBuilder(USER);
        private ModelStateDictionary theModelState;
        private IHAVIssueService theService;
        private Mock<IHAVIssueRepository> theMockRepository;
        private Mock<IHAVBaseRepository> theBaseRepository;
        private Mock<IHAVBaseService> theMockedBaseService;
        private Issue theIssue;
        private IssueReply theIssueReply;
        private IssueReplyComment theComment;

        [TestInitialize]
        public void Initialize() {
            theModelState = new ModelStateDictionary();
            theMockRepository = new Mock<IHAVIssueRepository>();
            theBaseRepository = new Mock<IHAVBaseRepository>();
            theMockedBaseService = new Mock<IHAVBaseService>();

            theService = new HAVIssueService(new ModelStateWrapper(theModelState),
                                                               theMockRepository.Object,
                                                               theBaseRepository.Object);
            theIssue = Issue.CreateIssue(ID, ISSUE_TITLE, ISSUE_DESCRIPTION, "Chicago", "IL", DateTime.UtcNow, 0, false);
            theIssueReply = IssueReply.CreateIssueReply(ID, 0, 0, REPLY, "Chicago", "IL", 1, false, DateTime.UtcNow, false);
            theComment = IssueReplyComment.CreateIssueReplyComment(ID, ID, REPLY, DateTime.UtcNow, ID, false);
        }

        [TestMethod]
        public void ShouldCreateValidIssue() {
            List<Permission> myPermissions = new List<Permission>();
            myPermissions.Add(POST_ISSUE_PERMISSION);
            theUserInformationBuilder.AddPermissions(myPermissions);

            Issue issue = Issue.CreateIssue(0, ISSUE_TITLE, ISSUE_DESCRIPTION, "Chicago", "IL", ISSUE_POST_DATE, 0, false);
            bool result = theService.CreateIssue(theUserInformationBuilder.Build(), issue);
            theMockRepository.Verify(r => r.CreateIssue(It.IsAny<Issue>(), It.IsAny<User>()), Times.Exactly(1));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UnableToCreateIssueBecauseOfNoPermission() {
            List<Permission> myPermissions = new List<Permission>();
            theUserInformationBuilder.AddPermissions(myPermissions);
            Issue issue = Issue.CreateIssue(0, ISSUE_TITLE, ISSUE_DESCRIPTION, "Chicago", "IL", ISSUE_POST_DATE, 0, false);
            bool myResult = theService.CreateIssue(theUserInformationBuilder.Build(), issue);
            theMockRepository.Verify(r => r.CreateIssue(It.IsAny<Issue>(), It.IsAny<User>()), Times.Never());
            Assert.IsFalse(myResult);
        }

        [TestMethod]
        public void UnableToCreateCreateIssueBecauseNameIsRequired() {
            Issue issue = Issue.CreateIssue(0, string.Empty, ISSUE_DESCRIPTION, "Chicago", "IL", ISSUE_POST_DATE, 0, false);

            bool result = theService.CreateIssue(theUserInformationBuilder.Build(), issue);

            theMockRepository.Verify(r => r.CreateIssue(It.IsAny<Issue>(), It.IsAny<User>()), Times.Never());
            Assert.IsFalse(result);
            var error = theModelState["Title"].Errors[0];
            Assert.AreEqual("Title is required.", error.ErrorMessage);
        }

        [TestMethod]
        public void UnableToCreateCreateIssueBecauseDescriptionIsRequired() {
            Issue issue = Issue.CreateIssue(0, ISSUE_TITLE, string.Empty, "Chicago", "IL", ISSUE_POST_DATE, 0, false);

            bool result = theService.CreateIssue(theUserInformationBuilder.Build(), issue);

            theMockRepository.Verify(r => r.CreateIssue(It.IsAny<Issue>(), It.IsAny<User>()), Times.Never());
            Assert.IsFalse(result);
            var error = theModelState["Description"].Errors[0];
            Assert.AreEqual("Description is required.", error.ErrorMessage);
        }

        [TestMethod]
        public void ShouldCreateValidIssueReply() {
            List<Permission> myPermissions = new List<Permission>();
            myPermissions.Add(POST_ISSUE_REPLY_PERMISSION);
            theUserInformationBuilder.AddPermissions(myPermissions);

            IssueModel myIssueModel = new IssueModel(new Issue(), null, null);
            myIssueModel.Comment = ISSUE_REPLY;
            myIssueModel.Disposition = ISSUE_DISPOSITION;
            bool myResult = theService.CreateIssueReply(theUserInformationBuilder.Build(), myIssueModel);
            theMockRepository.Verify(r => r.CreateIssueReply(It.IsAny<Issue>(), It.IsAny<User>(),
                It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Disposition>()), Times.Exactly(1));
            Assert.IsTrue(myResult);
        }

        [TestMethod]
        public void UnableToCreateValidIssueReplyBecauseOfNoPermission() {
            IssueModel myIssueModel = new IssueModel(new Issue(), null, null);
            myIssueModel.Comment = ISSUE_REPLY;
            myIssueModel.Disposition = ISSUE_DISPOSITION;
            bool myResult = theService.CreateIssueReply(theUserInformationBuilder.Build(), myIssueModel);
            theMockRepository.Verify(r => r.CreateIssueReply(It.IsAny<Issue>(), It.IsAny<User>(),
                It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Disposition>()), Times.Never());
            Assert.IsFalse(myResult);
        }

        [TestMethod]
        public void UnableToCreateIssueReplyBecauseCommentIsRequired() {
            IssueModel myIssueModel = new IssueModel(new Issue(), null, null);
            myIssueModel.Comment = string.Empty;
            myIssueModel.Disposition = ISSUE_DISPOSITION;

            bool result = theService.CreateIssueReply(theUserInformationBuilder.Build(), myIssueModel);

            theMockRepository.Verify(r => r.CreateIssueReply(It.IsAny<Issue>(), It.IsAny<User>(),
                It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Disposition>()), Times.Never());
            Assert.IsFalse(result);
            var error = theModelState["Reply"].Errors[0];
            Assert.AreEqual("Reply is required.", error.ErrorMessage);
        }

        [TestMethod]
        public void UnableToCreateIssueReplyBecauseDispositionIsRequired() {
            IssueModel issueModel = new IssueModel(new Issue(), null, null);
            issueModel.Comment = ISSUE_REPLY;
            issueModel.Disposition = Disposition.NONE;

            bool result = theService.CreateIssueReply(theUserInformationBuilder.Build(), issueModel);

            theMockRepository.Verify(r => r.CreateIssueReply(It.IsAny<Issue>(), It.IsAny<User>(),
                It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Disposition>()), Times.Never());
            Assert.IsFalse(result);
            var error = theModelState["Disposition"].Errors[0];
            Assert.AreEqual("Disposition is required.", error.ErrorMessage);
        }
        
        [TestMethod]
        public void ShouldEditIssue() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Edit_Issue);

            bool myResult = theService.EditIssue(theUserInformationBuilder.Build(), theIssue);

            AssertEditIssue(Times.Exactly(1), false);
            Assert.IsTrue(myResult);
        }

        [TestMethod]
        public void ShouldEditIssueWithOverridePermission() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Edit_Any_Issue);

            bool myResult = theService.EditIssue(theUserInformationBuilder.Build(), theIssue);

            AssertEditIssue(Times.Exactly(1), true);
            Assert.IsTrue(myResult);
        }

        [TestMethod]
        public void ShouldNotEditIssueWithoutPermission() {
            bool myResult = theService.EditIssue(theUserInformationBuilder.Build(), theIssue);

            AssertEditIssue(Times.Never(), It.IsAny<bool>());
            Assert.IsFalse(myResult);
            var myError = theModelState["PerformAction"].Errors[0];
            Assert.IsNotNull(myError);
        }

        [TestMethod]
        public void ShouldNotEditIssueBecauseTitleIsRequired() {
            theIssue.Title = string.Empty;
            bool myResult = theService.EditIssue(theUserInformationBuilder.Build(), theIssue);

            AssertEditIssue(Times.Never(), It.IsAny<bool>());
            Assert.IsFalse(myResult);
            var myError = theModelState["Title"].Errors[0];
            Assert.IsNotNull(myError.ErrorMessage);
        }

        [TestMethod]
        public void ShouldNotEditIssueBecauseDescriptionIsRequired() {
            theIssue.Description = string.Empty;
            bool myResult = theService.EditIssue(theUserInformationBuilder.Build(), theIssue);

            AssertEditIssue(Times.Never(), It.IsAny<bool>());
            Assert.IsFalse(myResult);
            var myError = theModelState["Description"].Errors[0];
            Assert.IsNotNull(myError.ErrorMessage);
        }

        [TestMethod]
        public void ShouldEditIssueReply() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Edit_Issue_Reply);

            bool myResult = theService.EditIssueReply(theUserInformationBuilder.Build(), theIssueReply);

            AssertEditReply(Times.Exactly(1), false);
            Assert.IsTrue(myResult);
        }

        [TestMethod]
        public void ShouldEditIssueReplyWithOverridePermission() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Edit_Any_Issue_Reply);

            bool myResult = theService.EditIssueReply(theUserInformationBuilder.Build(), theIssueReply);

            AssertEditReply(Times.Exactly(1), true);
            Assert.IsTrue(myResult);
        }

        [TestMethod]
        public void ShouldNotEditIssueReplyWithoutPermission() {
            bool myResult = theService.EditIssueReply(theUserInformationBuilder.Build(), theIssueReply);

            AssertEditReply(Times.Never(), It.IsAny<bool>());
            Assert.IsFalse(myResult);
            var myError = theModelState["PerformAction"].Errors[0];
            Assert.IsNotNull(myError);
        }

        [TestMethod]
        public void ShouldNotEditIssueReplyBecauseReplyIsRequired() {
            theIssueReply.Reply = string.Empty;
            bool myResult = theService.EditIssueReply(theUserInformationBuilder.Build(), theIssueReply);

            AssertEditReply(Times.Never(), It.IsAny<bool>());
            Assert.IsFalse(myResult);
            var error = theModelState["Reply"].Errors[0];
            Assert.AreEqual("Reply is required.", error.ErrorMessage);
        }
        
        [TestMethod]
        public void ShouldEditIssueReplyComment() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Edit_Issue_Reply_Comment);

            bool myResult = theService.EditIssueReplyComment(theUserInformationBuilder.Build(), theComment);

            AssertEditReplComment(Times.Exactly(1), false);
            Assert.IsTrue(myResult);
        }

        [TestMethod]
        public void ShouldEditIssueReplyCommentWithOverridePermission() {
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.Edit_Any_Issue_Reply_Comment);

            bool myResult = theService.EditIssueReplyComment(theUserInformationBuilder.Build(), theComment);

            AssertEditReplComment(Times.Exactly(1), true);
            Assert.IsTrue(myResult);
        }

        [TestMethod]
        public void ShouldNotEditIssueReplyCommentWithoutPermission() {
            bool myResult = theService.EditIssueReplyComment(theUserInformationBuilder.Build(), theComment);

            AssertEditReplComment(Times.Never(), It.IsAny<bool>());
            Assert.IsFalse(myResult);
            var myError = theModelState["PerformAction"].Errors[0];
            Assert.IsNotNull(myError);
        }

        [TestMethod]
        public void ShouldNotEditIssueReplyCommentBecauseReplyIsRequired() {
            theComment.Comment = string.Empty;
            bool myResult = theService.EditIssueReplyComment(theUserInformationBuilder.Build(), theComment);

            AssertEditReplComment(Times.Never(), It.IsAny<bool>());
            Assert.IsFalse(myResult);
            var error = theModelState["Comment"].Errors[0];
            Assert.AreEqual("Comment is required.", error.ErrorMessage);
        }

        private void AssertEditIssue(Times aTimes, bool anAdminOverride) {
            theMockRepository.Verify(r => r.UpdateIssue(It.IsAny<User>(), It.IsAny<Issue>(), It.IsAny<Issue>(), anAdminOverride), aTimes);
        }

        private void AssertEditReply(Times aTimes, bool anAdminOverride) {
            theMockRepository.Verify(r => r.UpdateIssueReply(It.IsAny<User>(), It.IsAny<IssueReply>(), It.IsAny<IssueReply>(), anAdminOverride), aTimes);
        }

        private void AssertEditReplComment(Times aTimes, bool anAdminOverride) {
            theMockRepository.Verify(r => r.UpdateIssueReplyComment(It.IsAny<User>(), It.IsAny<IssueReplyComment>(), It.IsAny<IssueReplyComment>(), anAdminOverride), aTimes);
        }
    }
}
