using System.Web.Mvc;
using HaveAVoice.Models;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Services;
using HaveAVoice.Services.UserFeatures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HaveAVoice.Tests.Models.Services.UserFeatures {
    [TestClass]
    public class HAVComplaintServiceTest {
        private static int VALID_COMPLAINT_SOURCE_ID = 45;
        private static string VALID_COMPLAINT = "This sucks.";
        private static User USER = new User();

        private ModelStateDictionary theModelState;
        private IHAVComplaintService theService;
        private Mock<IHAVComplaintRepository> theMockRepository;
        private Mock<IHAVBaseRepository> theBaseRepository;
        private Mock<IHAVBaseService> theMockedBaseService;

        /*
        [TestInitialize]
        public void Initialize() {
            theModelState = new ModelStateDictionary();
            theMockRepository = new Mock<IHAVComplaintRepository>();
            theBaseRepository = new Mock<IHAVBaseRepository>();
            theMockedBaseService = new Mock<IHAVBaseService>();

            theService = new HAVComplaintService(new ModelStateWrapper(theModelState),
                                                               theMockRepository.Object,
                                                               theBaseRepository.Object);
        }

        #region "Issue Complaint"

        [TestMethod]
        public void CreateValidIssueComplaint() {
            bool myResult = theService.IssueComplaint(USER, VALID_COMPLAINT, VALID_COMPLAINT_SOURCE_ID);
            Assert.IsTrue(myResult);
        }

        [TestMethod]
        public void CreateIssueComplaint_InvalidSourceId() {
            bool myResult = theService.IssueComplaint(USER, VALID_COMPLAINT, 0);
            Assert.IsFalse(myResult);
            var myError = theModelState["ComplaintSourceId"].Errors[0];
            Assert.AreEqual("Can't tell what you are reporting about. Please go back and try again.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateIssueComplaint_RequiredComplaint() {
            bool myResult = theService.IssueComplaint(USER, string.Empty, VALID_COMPLAINT_SOURCE_ID);
            Assert.IsFalse(myResult);
            var myError = theModelState["Complaint"].Errors[0];
            Assert.AreEqual("Complaint is required.", myError.ErrorMessage);
        }

        #endregion

        #region "Issue Reply Complaint"

        [TestMethod]
        public void CreateValidIssueReplyComplaint() {
            bool myResult = theService.IssueReplyComplaint(USER, VALID_COMPLAINT, VALID_COMPLAINT_SOURCE_ID);
            Assert.IsTrue(myResult);
        }

        [TestMethod]
        public void CreateIssueReplyComplaint_InvalidSourceId() {
            bool myResult = theService.IssueReplyComplaint(USER, VALID_COMPLAINT, 0);
            Assert.IsFalse(myResult);
            var myError = theModelState["ComplaintSourceId"].Errors[0];
            Assert.AreEqual("Can't tell what you are reporting about. Please go back and try again.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateIssueReplyComplaint_RequiredComplaint() {
            bool myResult = theService.IssueReplyComplaint(USER, string.Empty, VALID_COMPLAINT_SOURCE_ID);
            Assert.IsFalse(myResult);
            var myError = theModelState["Complaint"].Errors[0];
            Assert.AreEqual("Complaint is required.", myError.ErrorMessage);
        }

        #endregion


        #region "Issue Reply Comment Complaint"

        [TestMethod]
        public void CreateValidIssueReplyCommentComplaint() {
            bool myResult = theService.IssueReplyCommentComplaint(USER, VALID_COMPLAINT, VALID_COMPLAINT_SOURCE_ID);
            Assert.IsTrue(myResult);
        }

        [TestMethod]
        public void CreateIssueReplyCommentComplaint_InvalidSourceId() {
            bool myResult = theService.IssueReplyCommentComplaint(USER, VALID_COMPLAINT, 0);
            Assert.IsFalse(myResult);
            var myError = theModelState["ComplaintSourceId"].Errors[0];
            Assert.AreEqual("Can't tell what you are reporting about. Please go back and try again.", myError.ErrorMessage);
        }

        [TestMethod]
        public void CreateIssueReplyCommentComplaint_RequiredComplaint() {
            bool myResult = theService.IssueReplyCommentComplaint(USER, string.Empty, VALID_COMPLAINT_SOURCE_ID);
            Assert.IsFalse(myResult);
            var myError = theModelState["Complaint"].Errors[0];
            Assert.AreEqual("Complaint is required.", myError.ErrorMessage);
        }

        #endregion
        */

    }
}
